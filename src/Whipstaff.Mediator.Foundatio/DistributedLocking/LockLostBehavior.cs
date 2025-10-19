// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Mediator.Foundatio.DistributedLocking
{
    /// <summary>
    /// Represents what happens if the lock is lost.
    /// </summary>
    public enum LockLostBehavior
    {
        /// <summary>
        /// No behaviour defined.
        /// </summary>
        None,

        /// <summary>
        /// The process should retry. Potentially acting as the inactive partner in the pool if this
        /// is a distributed \ high availability contingency process.
        /// </summary>
        Retry,

        /// <summary>
        /// The process should complete it's task. Use this when you want to use continuations or manage
        /// the process flow with external logic that doesn't care about an exception.
        /// </summary>
        Complete,

        /// <summary>
        /// The process should error. Use this when you want external logic to do exception handling \ error handling
        /// around the process.
        /// </summary>
        Error
    }
}
