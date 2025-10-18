// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Foundatio.Caching;
using Foundatio.Lock;
using Foundatio.Messaging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Whipstaff.Core.Logging;

namespace Whipstaff.Mediator.Foundatio.DistributedLocking
{
    /// <summary>
    /// Process Manager for handling a distributed lock.
    /// </summary>
    public sealed class DistributedLockProcessManager : BackgroundService
    {
        private readonly BehaviorSubject<bool> _hasLockSubject = new(false);
        private readonly ILogger<DistributedLockProcessManager> _logger;
        private readonly IMessageBus _messageBus;
        private readonly string _lockName;
        private readonly LockLostBehavior _lockLostBehaviour;

        /// <summary>
        /// Initializes a new instance of the <see cref="DistributedLockProcessManager"/> class.
        /// </summary>
        /// <param name="messageBus">The message bus to attach to.</param>
        /// <param name="lockName">The name of the lock.</param>
        /// <param name="lockLostBehavior">The behavior to carry out if the lock is lost.</param>
        /// <param name="logger">Logging framework instance.</param>
        public DistributedLockProcessManager(
            IMessageBus messageBus,
            string lockName,
            LockLostBehavior lockLostBehavior,
            ILogger<DistributedLockProcessManager> logger)
        {
            ArgumentNullException.ThrowIfNull(messageBus);
            ArgumentNullException.ThrowIfNull(logger);
            if (string.IsNullOrWhiteSpace(lockName))
            {
                throw new ArgumentNullException(nameof(lockName));
            }

            _messageBus = messageBus;
            _lockName = lockName;
            _lockLostBehaviour = lockLostBehavior;
            _logger = logger;
        }

        /// <summary>
        /// Gets an observable for tracking whether the process manager has the lock.
        /// </summary>
        public IObservable<bool> HasLock => _hasLockSubject.AsObservable();

        /// <summary>
        /// Creates and starts a Distributed Lock Process Manager.
        /// </summary>
        /// <param name="messageBus">The message bus to attach to.</param>
        /// <param name="loggerFactory">The logging interface factory.</param>
        /// <param name="lockName">The name of the lock.</param>
        /// <param name="lockLostBehavior">The behavior to carry out if the lock is lost.</param>
        /// <param name="hasLockObserver">The observer for watching if the lock is aquired.</param>
        /// <param name="cancellationToken">The cancellation token used to stop the process manager.</param>
        /// <returns>Instance of the distributed lock process manager and the active task.</returns>
        public static (DistributedLockProcessManager Instance, Task Task, IDisposable HasLockSubscription) SubscribeAndStart(
            IMessageBus messageBus,
            ILoggerFactory loggerFactory,
            string lockName,
            LockLostBehavior lockLostBehavior,
            IObserver<bool> hasLockObserver,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(hasLockObserver);

            var instance = new DistributedLockProcessManager(
                messageBus,
                lockName,
                lockLostBehavior,
                loggerFactory.CreateLogger<DistributedLockProcessManager>());

            var hasLockSubscription = instance.HasLock.Subscribe(hasLockObserver);
            var task = instance.StartAsync(cancellationToken);

            return (instance, task, hasLockSubscription);
        }

        /// <inheritdoc/>
        public override void Dispose()
        {
            base.Dispose();

            if (!_hasLockSubject.IsDisposed)
            {
                _hasLockSubject.Dispose();
            }
        }

        /// <inheritdoc/>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return StartInternalAsync(
                _lockName,
                _lockLostBehaviour,
                stoppingToken);
        }

        private async Task StartInternalAsync(
            string lockName,
            LockLostBehavior lockLostBehavior,
            CancellationToken cancellationToken)
        {
            _logger.TraceMethodEntry();

            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            var lockTimeout = TimeSpan.FromSeconds(5);

            using (var inMemoryLockCacheClient = new InMemoryCacheClient())
            using (var distributedLockCacheClient = new HybridCacheClient(inMemoryLockCacheClient, _messageBus))
            {
                var lockProvider = new CacheLockProvider(distributedLockCacheClient, _messageBus);

                while (!cancellationToken.IsCancellationRequested)
                {
                    var lockHandle = await lockProvider.AcquireAsync(
                        lockName,
                        lockTimeout,
                        cancellationToken)
                        .ConfigureAwait(false);

                    if (lockHandle != null)
                    {
                        try
                        {
                            // we have the lock
                            // keep renewing it
                            await HoldAndRenewAsync(
                                lockHandle,
                                cancellationToken)
                                .ConfigureAwait(false);
                        }
                        catch (Exception e)
                        {
                            switch (lockLostBehavior)
                            {
                                case LockLostBehavior.Complete:
                                    return;
                                case LockLostBehavior.Error:
                                    throw new LockLostException(lockName, e);
                                case LockLostBehavior.Retry:
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private async Task HoldAndRenewAsync(
            ILock lockHandle,
            CancellationToken cancellationToken)
        {
            _logger.TraceMethodEntry();
            _hasLockSubject.OnNext(true);

            // however we keep in mind if the service bus dies
            // we can in theory lose the lock
            using (var lockHeldCancellationTokenSource = new CancellationTokenSource())
            {
                var lockHeldCancellationToken = lockHeldCancellationTokenSource.Token;
                using (var loopAndRenewCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, lockHeldCancellationToken))
                {
                    var loopAndRenewCancellationToken = loopAndRenewCancellationTokenSource.Token;
                    await LoopAndRenewAsync(
                        lockHandle,
                        loopAndRenewCancellationToken)
                        .ConfigureAwait(false);
                }

                await lockHandle.ReleaseAsync()
                    .ConfigureAwait(false);

                _hasLockSubject.OnNext(false);
                _hasLockSubject.OnCompleted();
            }
        }

        private async Task LoopAndRenewAsync(
            ILock lockHandle,
            CancellationToken cancellationToken)
        {
            _logger.TraceMethodEntry();
            const int timeout = 5000;
            await LoopUntilCancelledAsync(
                () => lockHandle.RenewAsync(),
                timeout,
                cancellationToken)
                .ConfigureAwait(false);
        }

        private async Task LoopUntilCancelledAsync(
            Func<Task> task,
            int timeout,
            CancellationToken cancellationToken)
        {
            _logger.TraceMethodEntry();

            while (!cancellationToken.IsCancellationRequested)
            {
                await task()
                    .ConfigureAwait(false);
                _ = cancellationToken.WaitHandle.WaitOne(timeout);
            }
        }
    }
}
