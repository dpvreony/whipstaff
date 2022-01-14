// Copyright (c) 2020 DHGMS Solutions and Contributors. All rights reserved.
// DHGMS Solutions and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Couchbase.Extensions.Locks;

namespace Whipstaff.Couchbase
{
    /// <summary>
    /// Exclusive lock integration for Couchbase.
    /// </summary>
    public sealed class CouchbaseLock : global::Foundatio.Lock.ILock
    {
        private readonly ICouchbaseMutex _mutex;

        /// <summary>
        /// Initializes a new instance of the <see cref="CouchbaseLock"/> class.
        /// </summary>
        /// <param name="mutex">
        ///     Couchbase mutex manager.
        /// </param>
        /// <param name="resource">Name of the resource.</param>
        /// <param name="aquiredTimeUtc">The timestamp for when the lock was aquired.</param>
        /// <param name="timeWaitedForLock">The time waited for the lock</param>
        public CouchbaseLock(
            ICouchbaseMutex mutex,
            string resource,
            DateTime aquiredTimeUtc,
            TimeSpan timeWaitedForLock)
        {
            _mutex = mutex ?? throw new ArgumentNullException(nameof(mutex));
            Resource = resource ?? throw new ArgumentNullException(nameof(resource));
            AcquiredTimeUtc = aquiredTimeUtc;
            TimeWaitedForLock = timeWaitedForLock;
            RenewalCount = 0;
        }

        /// <inheritdoc/>
        public string LockId => _mutex.Name;

        /// <inheritdoc/>
        public string Resource { get; }

        /// <inheritdoc/>
        public DateTime AcquiredTimeUtc { get; }

        /// <inheritdoc/>
        public TimeSpan TimeWaitedForLock { get; }

        /// <inheritdoc/>
        public int RenewalCount { get; private set; }

        /// <inheritdoc/>
        public ValueTask DisposeAsync()
        {
            return default(ValueTask);
        }

        /// <inheritdoc/>
        public async Task RenewAsync(TimeSpan? timeUntilExpires = null)
        {
            await _mutex.Renew(timeUntilExpires ?? TimeSpan.FromMinutes(1))
                .ConfigureAwait(false);
            RenewalCount++;
        }

        /// <inheritdoc/>
        public Task ReleaseAsync()
        {
            _mutex.Dispose();
            return Task.CompletedTask;
        }
    }
}
