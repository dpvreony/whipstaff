// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using DnsClient.Internal;
using Microsoft.Extensions.Logging;
using Whipstaff.Core.Logging;

namespace Whipstaff.Couchbase
{
    /// <summary>
    /// Log message actions wrapper for <see cref="CouchbaseLockProvider"/>.
    /// </summary>
    public sealed class CouchbaseLockProviderLogMessageActionsWrapper : AbstractLogMessageActionsWrapper<CouchbaseLockProvider, CouchbaseLockProviderLogMessageActions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CouchbaseLockProviderLogMessageActionsWrapper"/> class.
        /// </summary>
        /// <param name="logMessageActions">Log Message Actions instance.</param>
        /// <param name="logger">Logging framework instance.</param>
        public CouchbaseLockProviderLogMessageActionsWrapper(
            CouchbaseLockProviderLogMessageActions logMessageActions,
#pragma warning disable S6672
            ILogger<CouchbaseLockProvider> logger)
#pragma warning restore S6672
            : base(
                logMessageActions,
                logger)
        {
        }

        /// <summary>
        /// Log event when starting to acquire a lock.
        /// </summary>
        /// <param name="resource">Resource identifier.</param>
        public void StartingAcquire(string resource)
        {
            BrowserInstanceLogMessageActions.StartingAcquire(Logger, resource);
        }

        /// <summary>
        /// Log event when finished acquiring a lock.
        /// </summary>
        /// <param name="resource">Resource identifier.</param>
        public void FinishedAcquire(string resource)
        {
            BrowserInstanceLogMessageActions.FinishedAcquire(Logger, resource);
        }

        /// <summary>
        /// Log event when starting to release a lock.
        /// </summary>
        /// <param name="resource">Resource identifier.</param>
        public void StartingRelease(string resource)
        {
            BrowserInstanceLogMessageActions.StartingRelease(Logger, resource);
        }

        /// <summary>
        /// Log event when finished releasing a lock.
        /// </summary>
        /// <param name="resource">Resource identifier.</param>
        public void FinishedRelease(string resource)
        {
            BrowserInstanceLogMessageActions.FinishedRelease(Logger, resource);
        }

        /// <summary>
        /// Log event when starting to renew a lock.
        /// </summary>
        /// <param name="resource">Resource identifier.</param>
        public void StartingRenew(string resource)
        {
            BrowserInstanceLogMessageActions.StartingRenew(Logger, resource);
        }

        /// <summary>
        /// Log event when finished renewing a lock.
        /// </summary>
        /// <param name="resource">Resource identifier.</param>
        public void NoLockToRenew(string resource)
        {
            BrowserInstanceLogMessageActions.NoLockToRenew(Logger, resource);
        }

        /// <summary>
        /// Log event when there is no lock to renew.
        /// </summary>
        /// <param name="resource">Resource identifier.</param>
        public void FinishedRenew(string resource)
        {
            BrowserInstanceLogMessageActions.FinishedRenew(Logger, resource);
        }

        /// <summary>
        /// Log event when there is no mutex to release.
        /// </summary>
        /// <param name="resource">Resource identifier.</param>
        public void NoMutexToRelease(string resource)
        {
            BrowserInstanceLogMessageActions.NoMutexToRelease(Logger, resource);
        }

        /// <summary>
        /// Log event when acquired key already exists.
        /// </summary>
        /// <param name="resource">Resource identifier.</param>
        public void AcquiredKeyAlreadyExists(string resource)
        {
            BrowserInstanceLogMessageActions.AcquiredKeyAlreadyExists(Logger, resource);
        }
    }
}
