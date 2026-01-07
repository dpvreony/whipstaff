// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Couchbase;
using Couchbase.Extensions.DependencyInjection;
using Couchbase.Extensions.Locks;
using Couchbase.KeyValue;
using Foundatio.Lock;
using Microsoft.Extensions.Logging.Abstractions;
using Whipstaff.Core.Logging;

namespace Whipstaff.Couchbase
{
    /// <summary>
    /// A Couchbase Lock Distributed Log Provider.
    /// </summary>
    public sealed class CouchbaseLockProvider : ILockProvider
    {
        private readonly ConcurrentDictionary<string, ICouchbaseMutex> _mutexDictionary = new();
        private readonly ICouchbaseCollection _couchbaseCollection;
        private readonly CouchbaseLockProviderLogMessageActionsWrapper _logMessageActionsWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CouchbaseLockProvider"/> class.
        /// </summary>
        /// <param name="couchbaseCollection">The couchbase collection.</param>
        /// <param name="logMessageActionsWrapper">The logging framework wrapper instance.</param>
        public CouchbaseLockProvider(
            ICouchbaseCollection couchbaseCollection,
            CouchbaseLockProviderLogMessageActionsWrapper logMessageActionsWrapper)
        {
            ArgumentNullException.ThrowIfNull(couchbaseCollection);
            ArgumentNullException.ThrowIfNull(logMessageActionsWrapper);
            _couchbaseCollection = couchbaseCollection;
            _logMessageActionsWrapper = logMessageActionsWrapper;
        }

        /// <summary>
        /// Gets a Couchbase Lock Provider using a bucket provider.
        /// </summary>
        /// <param name="bucketProvider">Bucket Provider Instance to use.</param>
        /// <param name="logMessageActionsWrapper">The logging framework wrapper instance.</param>
        /// <returns>Instance of <see cref="CouchbaseLockProvider"/>.</returns>
        public static async Task<CouchbaseLockProvider> GetInstanceAsync(
            IBucketProvider bucketProvider,
            CouchbaseLockProviderLogMessageActionsWrapper logMessageActionsWrapper)
        {
            ArgumentNullException.ThrowIfNull(bucketProvider);
            ArgumentNullException.ThrowIfNull(logMessageActionsWrapper);

            var bucket = await bucketProvider.GetBucketAsync("default")
                .ConfigureAwait(false);

            return GetInstance(bucket, logMessageActionsWrapper);
        }

        /// <summary>
        /// Gets a Couchbase Lock Provider using a bucket provider.
        /// </summary>
        /// <param name="bucket">Couchbase Bucket to use.</param>
        /// <param name="logMessageActionsWrapper">The logging framework wrapper instance.</param>
        /// <returns>Instance of <see cref="CouchbaseLockProvider"/>.</returns>
        public static CouchbaseLockProvider GetInstance(
            IBucket bucket,
            CouchbaseLockProviderLogMessageActionsWrapper logMessageActionsWrapper)
        {
            ArgumentNullException.ThrowIfNull(bucket);

            var collection = bucket.DefaultCollection();

            return new CouchbaseLockProvider(
                collection,
                logMessageActionsWrapper);
        }

        /// <inheritdoc/>
        public async Task<ILock> AcquireAsync(
            string resource,
            TimeSpan? timeUntilExpires = null,
            bool releaseOnDispose = true,
            CancellationToken cancellationToken = default)
        {
            _logMessageActionsWrapper.StartingAcquire(resource);
            var attemptStarted = DateTime.UtcNow;

            var mutex = await _couchbaseCollection.RequestMutexAsync(
                resource,
                timeUntilExpires ?? TimeSpan.FromMinutes(1),
                cancellationToken)
                .ConfigureAwait(false);

            var dateTimeAcquired = DateTime.UtcNow;
            var timeWaited = dateTimeAcquired - attemptStarted;

            if (!_mutexDictionary.TryAdd(resource, mutex))
            {
                _logMessageActionsWrapper.AcquiredKeyAlreadyExists(resource);
            }

            _logMessageActionsWrapper.FinishedAcquire(resource);

            return new CouchbaseLock(
                mutex,
                resource,
                dateTimeAcquired,
                timeWaited);
        }

        /// <inheritdoc/>
        public async Task<bool> IsLockedAsync(string resource)
        {
            var existsResult = await _couchbaseCollection.ExistsAsync(resource)
                .ConfigureAwait(false);

            return existsResult.Exists;
        }

        /// <inheritdoc/>
        public Task ReleaseAsync(string resource)
        {
            _logMessageActionsWrapper.StartingRelease(resource);

            if (!_mutexDictionary.TryGetValue(resource, out var mutex))
            {
                _logMessageActionsWrapper.NoMutexToRelease(resource);
                return Task.CompletedTask;
            }

            mutex.Dispose();
            _logMessageActionsWrapper.FinishedRelease(resource);

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task ReleaseAsync(string resource, string lockId)
        {
            _logMessageActionsWrapper.StartingRelease(resource);

            if (!_mutexDictionary.TryGetValue(resource, out var mutex))
            {
                _logMessageActionsWrapper.NoMutexToRelease(resource);
                return Task.CompletedTask;
            }

            mutex.Dispose();
            _logMessageActionsWrapper.FinishedRelease(resource);

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task RenewAsync(string resource, string lockId, TimeSpan? timeUntilExpires = null)
        {
            _logMessageActionsWrapper.StartingRenew(resource);

            if (!_mutexDictionary.TryGetValue(resource, out var mutex))
            {
                _logMessageActionsWrapper.NoLockToRenew(resource);
                return;
            }

            await mutex.Renew(timeUntilExpires ?? TimeSpan.FromMinutes(1))
                .ConfigureAwait(false);

            _logMessageActionsWrapper.FinishedRenew(resource);
        }
    }
}
