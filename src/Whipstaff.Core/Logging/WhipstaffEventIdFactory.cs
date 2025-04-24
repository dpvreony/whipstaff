// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;

namespace Whipstaff.Core.Logging
{
    /// <summary>
    /// Factory for Event Ids used within Whipstaff.
    /// </summary>
    public static class WhipstaffEventIdFactory
    {
        /// <summary>
        /// Gets the Event Id for the Db Context Save Result logging event.
        /// </summary>
        /// <returns>Event Id.</returns>
        public static EventId DbContextSaveResultEventId() => new(1, "DbContext Save Result");

        /// <summary>
        /// Gets the Event Id for the No MediatR Handlers Registered for type logging event.
        /// </summary>
        /// <returns>Event Id.</returns>
        public static EventId NoMediatRHandlersRegisteredForTypeEventId() => new(2, "No MediatR Handlers Registered for type");

        /// <summary>
        /// Gets the Event Id for the Number of MediatR Handlers Registered for type logging event.
        /// </summary>
        /// <returns>Event Id.</returns>
        public static EventId CountOfMediatRHandlersRegisteredEventId() => new(3, "Number of MediatR Handlers Registered for type");

        /// <summary>
        /// Gets the Event Id for the Method Entry logging event.
        /// </summary>
        /// <returns>Event Id.</returns>
        public static EventId MethodEntryEventId() => new(4, "Method Entry");

        /// <summary>
        /// Gets the Event Id for the Method Exit logging event.
        /// </summary>
        /// <returns>Event Id.</returns>
        public static EventId MethodExitEventId() => new(5, "Method Exit");

        /// <summary>
        /// Gets the Event Id for the Method Exit logging event.
        /// </summary>
        /// <returns>Event Id.</returns>
        public static EventId DefaultIfException() => new(6, "Defaulting Result due to exception");

        /// <summary>
        /// Gets the Event Id for the Middleware Starting event.
        /// </summary>
        /// <returns>Event Id.</returns>
        public static EventId MiddlewareStarting() => new(7, "Middleware Starting");

        /// <summary>
        /// Gets the Event Id for the Middleware Exception event.
        /// </summary>
        /// <returns>Event Id.</returns>
        public static EventId MiddlewareException() => new(8, "Middleware Exception");

        /// <summary>
        /// Gets the Event Id for the Middleware Finished event.
        /// </summary>
        /// <returns>Event Id.</returns>
        public static EventId MiddlewareFinished() => new(9, "Middleware Finished");

        /// <summary>
        /// Gets the Event Id for the "Starting Test Of DbSet" event which is used in the EF Core smoke test process.
        /// </summary>
        /// <returns>Event Id.</returns>
        public static EventId TestOfDbSetStarting() => new(10, "Starting Test Of DbSet");

        /// <summary>
        /// Gets the Event Id for the "Test Of DbSet Failed" event which is used in the EF Core smoke test process.
        /// </summary>
        /// <returns>Event Id.</returns>
        public static EventId TestOfDbSetFailed() => new(11, "Test Of DbSet Failed");

        /// <summary>
        /// Gets the Event Id for the "Completed Test Of DbSet" event which is used in the EF Core smoke test process.
        /// </summary>
        /// <returns>Event Id.</returns>
        public static EventId TestOfDbSetCompleted() => new(11, "Test Of DbSet Completed");

        /// <summary>
        /// Gets the Event Id for the Couchbase Starting Aquire event.
        /// </summary>
        /// <returns>Event Id.</returns>
        public static EventId CouchbaseStartingAquire() => new(12, "Starting to acquire lock for resource.");

        /// <summary>
        /// Gets the Event Id for the Couchbase Finished Aquire event.
        /// </summary>
        /// <returns>Event Id.</returns>
        public static EventId CouchbaseFinishedAquire() => new(13, "Finished acquiring of lock for resource.");

        /// <summary>
        /// Gets the Event Id for the Couchbase Starting Release event.
        /// </summary>
        /// <returns>Event Id.</returns>
        public static EventId CouchbaseStartingRelease() => new(14, "Starting release of lock for resource.");

        /// <summary>
        /// Gets the Event Id for the Couchbase Finished Release event.
        /// </summary>
        /// <returns>Event Id.</returns>
        public static EventId CouchbaseFinishedRelease() => new(15, "Finished release of lock for resource.");

        /// <summary>
        /// Gets the Event Id for the Couchbase Starting Renew event.
        /// </summary>
        /// <returns>Event Id.</returns>
        public static EventId CouchbaseStartingRenew() => new(16, "Finished renewal of lock for resource.");

        /// <summary>
        /// Gets the Event Id for the Couchbase Finished Renew event.
        /// </summary>
        /// <returns>Event Id.</returns>
        public static EventId CouchbaseFinishedRenew() => new(17, "Finished renewal of lock for resource.");

        /// <summary>
        /// Gets the Event Id for the Couchbase No Lock To Renew event.
        /// </summary>
        /// <returns>Event Id.</returns>
        public static EventId CouchbaseNoLockToRenew() => new(18, "No lock to renew for resource.");

        /// <summary>
        /// Gets the Event Id for the Couchbase No Mutex To Release event.
        /// </summary>
        /// <returns>Event Id.</returns>
        public static EventId CouchbaseNoMutexToRelease() => new(19, "No mutex to release for resource.");

        /// <summary>
        /// Gets the Event Id for the Couchbase Acquired Key Already Exists event.
        /// </summary>
        /// <returns>Event Id.</returns>
        public static EventId CouchbaseAcquiredKeyAlreadyExists() => new(20, "Acquired key already exists for resource.");

        /// <summary>
        /// Gets the Event Id for the No Jump List Registered for application, creating a new registration logging event.
        /// </summary>
        /// <returns>Event Id.</returns>
        public static EventId NoJumpListRegisteredCreatingNew() => new(21, "No Jump List Registered for application, creating a new registration");

        /// <summary>
        /// Gets the Event Id for the Adding Jump Path to Recent logging event.
        /// </summary>
        /// <returns>Event Id.</returns>
        public static EventId AddingJumpPathToRecentCategory() => new(22, "Adding Jump Path to Recent");

        /// <summary>
        /// Gets the Event Id for the Adding Jump Task to Recent logging event.
        /// </summary>
        /// <returns>Event Id.</returns>
        public static EventId AddingJumpTaskToRecentCategory() => new(23, "Adding Jump Task to Recent");

        /// <summary>
        /// Gets the Event Id for the Clearing Jump List logging event.
        /// </summary>
        /// <returns>Event Id.</returns>
        public static EventId ClearingJumpList() => new(24, "Clearing Jump List");
    }
}
