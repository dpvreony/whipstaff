// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;
using Whipstaff.Core.Logging;

namespace Whipstaff.Couchbase
{
    /// <summary>
    /// Log message actions for <see cref="CouchbaseLockProvider"/>.
    /// </summary>
    public sealed class CouchbaseLockProviderLogMessageActions : ILogMessageActions<CouchbaseLockProvider>
    {
        private readonly Action<ILogger, string, Exception?> _startingAcquire;
        private readonly Action<ILogger, string, Exception?> _finishedAcquire;
        private readonly Action<ILogger, string, Exception?> _startingRelease;
        private readonly Action<ILogger, string, Exception?> _finishedRelease;
        private readonly Action<ILogger, string, Exception?> _startingRenew;
        private readonly Action<ILogger, string, Exception?> _finishedRenew;
        private readonly Action<ILogger, string, Exception?> _noLockToRenew;
        private readonly Action<ILogger, string, Exception?> _noMutexToRelease;
        private readonly Action<ILogger, string, Exception?> _acquiredKeyAlreadyExists;

        /// <summary>
        /// Initializes a new instance of the <see cref="CouchbaseLockProviderLogMessageActions"/> class.
        /// </summary>
        public CouchbaseLockProviderLogMessageActions()
        {
            _startingAcquire = LoggerMessage.Define<string>(
                LogLevel.Information,
                WhipstaffEventIdFactory.CouchbaseStartingAquire(),
                "Starting to acquire lock for resource: \"{Resource}\".");
            _finishedAcquire = LoggerMessage.Define<string>(
                LogLevel.Information,
                WhipstaffEventIdFactory.CouchbaseFinishedAquire(),
                "Finished acquiring of lock for resource: \"{Resource}\".");
            _startingRelease = LoggerMessage.Define<string>(
                LogLevel.Information,
                WhipstaffEventIdFactory.CouchbaseStartingRelease(),
                "Starting release of lock for resource: \"{Resource}\".");
            _finishedRelease = LoggerMessage.Define<string>(
                LogLevel.Information,
                WhipstaffEventIdFactory.CouchbaseFinishedRelease(),
                "Finished release of lock for resource: \"{Resource}\".");
            _startingRenew = LoggerMessage.Define<string>(
                LogLevel.Information,
                WhipstaffEventIdFactory.CouchbaseStartingRenew(),
                "Finished renewal of lock for resource: \"{Resource}\".");
            _finishedRenew = LoggerMessage.Define<string>(
                LogLevel.Information,
                WhipstaffEventIdFactory.CouchbaseFinishedRenew(),
                "Finished renewal of lock for resource: \"{Resource}\".");
            _noLockToRenew = LoggerMessage.Define<string>(
                LogLevel.Warning,
                WhipstaffEventIdFactory.CouchbaseNoLockToRenew(),
                "No lock to renew for resource: \"{Resource}\".");
            _noMutexToRelease = LoggerMessage.Define<string>(
                LogLevel.Warning,
                WhipstaffEventIdFactory.CouchbaseNoMutexToRelease(),
                "No mutex to release for resource: \"{Resource}\".");
            _acquiredKeyAlreadyExists = LoggerMessage.Define<string>(
                LogLevel.Warning,
                WhipstaffEventIdFactory.CouchbaseAcquiredKeyAlreadyExists(),
                "Acquired key already exists for resource: \"{Resource}\".");
        }

        /// <summary>
        /// Log event when starting to acquire a lock.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        /// <param name="resource">Resource identifier.</param>
        public void StartingAcquire(ILogger<CouchbaseLockProvider> logger, string resource)
        {
            _startingAcquire(logger, resource, null);
        }

        /// <summary>
        /// Log event when finished acquiring a lock.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        /// <param name="resource">Resource identifier.</param>
        public void FinishedAcquire(ILogger<CouchbaseLockProvider> logger, string resource)
        {
            _finishedAcquire(logger, resource, null);
        }

        /// <summary>
        /// Log event when starting to release a lock.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        /// <param name="resource">Resource identifier.</param>
        public void StartingRelease(ILogger<CouchbaseLockProvider> logger, string resource)
        {
            _startingRelease(logger, resource, null);
        }

        /// <summary>
        /// Log event when finished releasing a lock.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        /// <param name="resource">Resource identifier.</param>
        public void FinishedRelease(ILogger<CouchbaseLockProvider> logger, string resource)
        {
            _finishedRelease(logger, resource, null);
        }

        /// <summary>
        /// Log event when starting to renew a lock.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        /// <param name="resource">Resource identifier.</param>
        public void StartingRenew(ILogger<CouchbaseLockProvider> logger, string resource)
        {
            _startingRenew(logger, resource, null);
        }

        /// <summary>
        /// Log event when finished renewing a lock.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        /// <param name="resource">Resource identifier.</param>
        public void FinishedRenew(ILogger<CouchbaseLockProvider> logger, string resource)
        {
            _finishedRenew(logger, resource, null);
        }

        /// <summary>
        /// Log event when there is no lock to renew.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        /// <param name="resource">Resource identifier.</param>
        public void NoLockToRenew(ILogger<CouchbaseLockProvider> logger, string resource)
        {
            _noLockToRenew(logger, resource, null);
        }

        /// <summary>
        /// Log event when there is no mutex to release.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        /// <param name="resource">Resource identifier.</param>
        public void NoMutexToRelease(ILogger<CouchbaseLockProvider> logger, string resource)
        {
            _noMutexToRelease(logger, resource, null);
        }

        /// <summary>
        /// Log event when acquired key already exists.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        /// <param name="resource">Resource identifier.</param>
        public void AcquiredKeyAlreadyExists(ILogger<CouchbaseLockProvider> logger, string resource)
        {
            _acquiredKeyAlreadyExists(logger, resource, null);
        }
    }
}
