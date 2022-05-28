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

namespace Whipstaff.Couchbase
{
    /// <summary>
    /// A Couchbase Lock Distributed Log Provider.
    /// </summary>
    public sealed class CouchbaseLockProvider : global::Foundatio.Lock.ILockProvider
    {
        private readonly ConcurrentDictionary<string, ICouchbaseMutex> _mutexDictionary = new ConcurrentDictionary<string, ICouchbaseMutex>();
        private readonly ICouchbaseCollection _couchbaseCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="CouchbaseLockProvider"/> class.
        /// </summary>
        /// <param name="couchbaseCollection">The couchbase collection.</param>
        public CouchbaseLockProvider(ICouchbaseCollection couchbaseCollection)
        {
            _couchbaseCollection = couchbaseCollection ?? throw new ArgumentNullException(nameof(couchbaseCollection));
        }

        /// <summary>
        /// Gets a Couchbase Lock Provider using a bucket provider.
        /// </summary>
        /// <param name="bucketProvider">Bucket Provider Instance to use.</param>
        /// <returns>Instance of <see cref="CouchbaseLockProvider"/>.</returns>
        public static async Task<CouchbaseLockProvider> GetInstance(IBucketProvider bucketProvider)
        {
            if (bucketProvider == null)
            {
                throw new ArgumentNullException(nameof(bucketProvider));
            }

            var bucket = await bucketProvider.GetBucketAsync("default")
                .ConfigureAwait(false);

            return GetInstance(bucket);
        }

        /// <summary>
        /// Gets a Couchbase Lock Provider using a bucket provider.
        /// </summary>
        /// <param name="bucket">Couchbase Bucket to use.</param>
        /// <returns>Instance of <see cref="CouchbaseLockProvider"/>.</returns>
        public static CouchbaseLockProvider GetInstance(IBucket bucket)
        {
            if (bucket == null)
            {
                throw new ArgumentNullException(nameof(bucket));
            }

            var collection = bucket.DefaultCollection();

            return new CouchbaseLockProvider(collection);
        }

        /// <inheritdoc/>
        public async Task<ILock> AcquireAsync(
            string resource,
            TimeSpan? timeUntilExpires,
            bool releaseOnDispose,
            CancellationToken cancellationToken)
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
