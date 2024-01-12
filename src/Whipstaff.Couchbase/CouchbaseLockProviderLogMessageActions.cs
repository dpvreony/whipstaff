// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;
using Whipstaff.Core.Logging;

namespace Whipstaff.Couchbase
{
    /// <summary>
    /// Log message actions for <see cref="CouchbaseLockProvider"/>.
    /// </summary>
    public sealed class CouchbaseLockProviderLogMessageActions : ILogMessageActions<CouchbaseLockProvider>
    {
        /// <summary>
        /// Log event when starting to acquire a lock.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        /// <param name="resource">Resource identifier.</param>
        public void StartingAcquire(ILogger<CouchbaseLockProvider> logger, string resource)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Log event when finished acquiring a lock.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        /// <param name="resource">Resource identifier.</param>
        public void FinishedAcquire(ILogger<CouchbaseLockProvider> logger, string resource)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Log event when starting to release a lock.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        /// <param name="resource">Resource identifier.</param>
        public void StartingRelease(ILogger<CouchbaseLockProvider> logger, string resource)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Log event when finished releasing a lock.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        /// <param name="resource">Resource identifier.</param>
        public void FinishedRelease(ILogger<CouchbaseLockProvider> logger, string resource)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Log event when starting to renew a lock.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        /// <param name="resource">Resource identifier.</param>
        public void StartingRenew(ILogger<CouchbaseLockProvider> logger, string resource)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Log event when finished renewing a lock.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        /// <param name="resource">Resource identifier.</param>
        public void FinishedRenew(ILogger<CouchbaseLockProvider> logger, string resource)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Log event when there is no lock to renew.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        /// <param name="resource">Resource identifier.</param>
        public void NoLockToRenew(ILogger<CouchbaseLockProvider> logger, string resource)
        {
            throw new System.NotImplementedException();
        }
    }
}
