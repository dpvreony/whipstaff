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
using Microsoft.Extensions.Logging;

namespace Whipstaff.Couchbase
{
    /// <summary>
    /// A Couchbase Lock Distributed Log Provider.
    /// </summary>
    public sealed class CouchbaseLockProvider : global::Foundatio.Lock.ILockProvider
    {
        private readonly ConcurrentDictionary<string, ICouchbaseMutex> _mutexDictionary = new ConcurrentDictionary<string, ICouchbaseMutex>();
        private readonly ICouchbaseCollection _couchbaseCollection;
        private readonly ILogger<CouchbaseLockProvider> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CouchbaseLockProvider"/> class.
        /// </summary>
        /// <param name="couchbaseCollection">The couchbase collection.</param>
        /// <param name="logger">The logging framework instance.</param>
        public CouchbaseLockProvider(
            ICouchbaseCollection couchbaseCollection,
            ILogger<CouchbaseLockProvider> logger)
        {
            ArgumentNullException.ThrowIfNull(couchbaseCollection);
            ArgumentNullException.ThrowIfNull(logger);
            _couchbaseCollection = couchbaseCollection;
            _logger = logger;
        }

        /// <summary>
        /// Gets a Couchbase Lock Provider using a bucket provider.
        /// </summary>
        /// <param name="bucketProvider">Bucket Provider Instance to use.</param>
        /// <param name="logger">The logging framework instance.</param>
        /// <returns>Instance of <see cref="CouchbaseLockProvider"/>.</returns>
        public static async Task<CouchbaseLockProvider> GetInstanceAsync(
            IBucketProvider bucketProvider,
            ILogger<CouchbaseLockProvider> logger)
        {
            ArgumentNullException.ThrowIfNull(bucketProvider);
            ArgumentNullException.ThrowIfNull(logger);

            var bucket = await bucketProvider.GetBucketAsync("default")
                .ConfigureAwait(false);

            return GetInstance(bucket, logger);
        }

        /// <summary>
        /// Gets a Couchbase Lock Provider using a bucket provider.
        /// </summary>
        /// <param name="bucket">Couchbase Bucket to use.</param>
        /// <param name="logger">The logging framework instance.</param>
        /// <returns>Instance of <see cref="CouchbaseLockProvider"/>.</returns>
        public static CouchbaseLockProvider GetInstance(
            IBucket bucket,
            ILogger<CouchbaseLockProvider> logger)
        {
            ArgumentNullException.ThrowIfNull(bucket);

            var collection = bucket.DefaultCollection();

            return new CouchbaseLockProvider(collection, logger);
        }

        /// <inheritdoc/>
        public async Task<ILock> AcquireAsync(
            string resource,
            TimeSpan? timeUntilExpires = null,
            bool releaseOnDispose = true,
            CancellationToken cancellationToken = default)
        {
            var attemptStarted = DateTime.UtcNow;

            var mutex = await _couchbaseCollection.RequestMutexAsync(
                resource,
                timeUntilExpires ?? TimeSpan.FromMinutes(1),
                cancellationToken)
                .ConfigureAwait(false);

            var dateTimeAquired = DateTime.UtcNow;
            var timeWaited = dateTimeAquired - attemptStarted;

            _ = _mutexDictionary.TryAdd(resource, mutex);

            return new CouchbaseLock(mutex, resource, dateTimeAquired, timeWaited);
        }

        /// <inheritdoc/>
        public async Task<bool> IsLockedAsync(string resource)
        {
            var existsResult = await _couchbaseCollection.ExistsAsync(resource)
                .ConfigureAwait(false);

            return existsResult.Exists;
        }

        /// <inheritdoc/>
        public Task ReleaseAsync(string resource, string lockId)
        {
            if (_mutexDictionary.TryGetValue(resource, out var mutex))
            {
                mutex.Dispose();
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task RenewAsync(string resource, string lockId, TimeSpan? timeUntilExpires = null)
        {
            if (!_mutexDictionary.TryGetValue(resource, out var mutex))
            {
                return;
            }

            await mutex.Renew(timeUntilExpires ?? TimeSpan.FromMinutes(1))
                .ConfigureAwait(false);
        }
    }
}
