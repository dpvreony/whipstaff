// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Threading;

namespace Whipstaff.Runtime.Counters
{
    /// <summary>
    /// Wrapper for a long that can only be incremented. Useful for operation counters.
    /// </summary>
    public sealed class IncrementOnlyLong
    {
        private long _value;

        /// <summary>
        /// Gets the current value of the counter.
        /// </summary>
        public long Value => _value;

        /// <summary>
        /// Increments the counter by one.
        /// </summary>
        /// <returns>
        /// The incremented value.
        /// </returns>
        public long Increment()
        {
            return Interlocked.Increment(ref _value);
        }
    }
}
