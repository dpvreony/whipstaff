// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// DHGMS Solutions and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Whipstaff.MediatR.Foundatio.DistributedLocking
{
    /// <summary>
    /// Exception for when a lock has been lost and the behaviour is to error.
    /// </summary>
#pragma warning disable CA1032 // Implement standard exception constructors
    public sealed class LockLostException : Exception
#pragma warning restore CA1032 // Implement standard exception constructors
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LockLostException"/> class.
        /// </summary>
        /// <param name="lockName">Name of the lock.</param>
        /// <param name="innerException">Inner exception.</param>
        public LockLostException(
            string lockName,
            Exception innerException)
            : base($"Lock Lost : {lockName}", innerException)
        {
        }
    }
}
