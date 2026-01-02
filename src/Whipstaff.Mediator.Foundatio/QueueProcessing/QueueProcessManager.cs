// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Foundatio.Queues;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Whipstaff.Core.ExceptionHandling;

namespace Whipstaff.Mediator.Foundatio.QueueProcessing
{
    /// <summary>
    /// A process manager for dealing with a Foundatio based queue mechanism.
    /// </summary>
    /// <typeparam name="TMessage">Type for the message being processed on the queue.</typeparam>
    public abstract class QueueProcessManager<TMessage> : BackgroundService
        where TMessage : class
    {
        private readonly IQueue<TMessage> _queue;
        private readonly ILogger<QueueProcessManager<TMessage>> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueProcessManager{TMessage}"/> class.
        /// </summary>
        /// <param name="queue">The queue to monitor.</param>
        /// <param name="logger">Logging framework instance.</param>
        protected QueueProcessManager(
            IQueue<TMessage> queue,
            ILogger<QueueProcessManager<TMessage>> logger)
        {
            _queue = queue ?? throw new ArgumentNullException(nameof(queue));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        protected sealed override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!stoppingToken.CanBeCanceled)
            {
                throw new ArgumentException("A valid cancellation token must be passed", nameof(stoppingToken));
            }

            return RunInternalAsync(stoppingToken);
        }

        /// <summary>
        /// Logic for processing the message when it is received.
        /// </summary>
        /// <param name="queueEntry">The message received from the queue.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        protected abstract Task OnMessageReceivedAsync(IQueueEntry<TMessage> queueEntry);

        /// <summary>
        /// Logic for dealing with a message that has failed to process.
        /// </summary>
        /// <param name="queueEntry">The queue entry to dead letter.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        protected abstract Task DoDeadLetterMessageAsync(IQueueEntry<TMessage> queueEntry);

        private static async Task HandleRenewalAsync(
            IQueueEntry<TMessage> queueEntry,
            CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await queueEntry.RenewLockAsync().ConfigureAwait(false);
                    await Task.Delay(5000, cancellationToken).ConfigureAwait(false);
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch
#pragma warning restore CA1031 // Do not catch general exception types
            {
                // no op
            }
        }

        private static async Task DoAbandonMessageAsync(IQueueEntry<TMessage> queueEntry)
        {
            await queueEntry.AbandonAsync()
                .ConfigureAwait(false);
        }

        private static async Task DoCompleteMessageAsync(IQueueEntry<TMessage> queueEntry)
        {
            await queueEntry.CompleteAsync()
                .ConfigureAwait(false);
        }

        private static Task<QueueMessageRecoveryStrategy> GetRecoveryStrategyAsync(Exception exception)
        {
            return Task.FromResult(QueueMessageRecoveryStrategy.Abandon);
        }

        private async Task RunInternalAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var queueEntry = await _queue.DequeueAsync(cancellationToken).ConfigureAwait(false);

                if (queueEntry == null)
                {
                    continue;
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    await queueEntry.AbandonAsync().ConfigureAwait(false);
                    return;
                }

                using (var innerCancellationTokenSource = new CancellationTokenSource())
                {
                    // this is here in case your task is long running
                    // queue locks release if they don't get renewed.
                    // dedicated thread to ensure renewal happens
                    var renewThread = new Thread(HandleRenewalAsync(
                        queueEntry,
                        innerCancellationTokenSource.Token)
                        .ConfigureAwait(false).GetAwaiter().GetResult);

                    renewThread.Start();

                    var attempt = 1;
                    var maxRetries = 2;
                    QueueMessageRecoveryStrategy recoveryStrategy;
                    do
                    {
                        recoveryStrategy = await OnProcessMessageAsync(queueEntry)
                            .ConfigureAwait(false);

                        if (recoveryStrategy != QueueMessageRecoveryStrategy.Retry)
                        {
                            break;
                        }

                        attempt++;
                    }
                    while (attempt <= maxRetries);

                    await innerCancellationTokenSource.CancelAsync()
                        .ConfigureAwait(false);

                    // we wait for renewal to stop so we can complete the message off, or act upon
                    // it without a race condition.
                    renewThread.Join();

                    await DoRecoveryStrategyAsync(recoveryStrategy, queueEntry)
                        .ConfigureAwait(false);
                }
            }
        }

        private async Task<QueueMessageRecoveryStrategy> OnProcessMessageAsync(IQueueEntry<TMessage> queueEntry)
        {
            try
            {
                await OnMessageReceivedAsync(queueEntry).ConfigureAwait(false);
                return QueueMessageRecoveryStrategy.Complete;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return await TaskHelpers.DefaultIfExceptionAsync(
                    static ex => GetRecoveryStrategyAsync(ex),
                    e,
                    QueueMessageRecoveryStrategy.Abandon).ConfigureAwait(false);
            }
        }

        private async Task DoRecoveryStrategyAsync(
            QueueMessageRecoveryStrategy recoveryStrategy,
            IQueueEntry<TMessage> queueEntry)
        {
            switch (recoveryStrategy)
            {
                case QueueMessageRecoveryStrategy.Abandon:
                    await DoAbandonMessageAsync(queueEntry)
                        .ConfigureAwait(false);
                    break;
                case QueueMessageRecoveryStrategy.Complete:
                    await DoCompleteMessageAsync(queueEntry)
                        .ConfigureAwait(false);
                    break;
                case QueueMessageRecoveryStrategy.DeadLetter:
                    await DoDeadLetterMessageAsync(queueEntry)
                        .ConfigureAwait(false);
                    break;
                case QueueMessageRecoveryStrategy.Requeue:
                    await DoRequeueMessageAsync(queueEntry)
                        .ConfigureAwait(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(recoveryStrategy),
                        $"Unexpected recovery strategy: {recoveryStrategy}");
            }
        }

        private async Task DoRequeueMessageAsync(IQueueEntry<TMessage> queueEntry)
        {
            // this won't work if duplicate detection is enabled and the message
            // is in the time frame.
            // we need to check the config moving forward.
            var enqueuedMessageId = await _queue.EnqueueAsync(queueEntry.Value)
                .ConfigureAwait(false);

            _logger.LogInformation("Re-queued Message {QueueEntryId} as {EnqueuedMessageId}", queueEntry.Id, enqueuedMessageId);

            // complete after enqueue, if enqueue fails, this should drop back to abandon or deadletter.
            await queueEntry.CompleteAsync()
                .ConfigureAwait(false);
        }
    }
}
