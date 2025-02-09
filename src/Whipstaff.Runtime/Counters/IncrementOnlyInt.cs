// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Threading;

namespace Whipstaff.Runtime.Counters
{
    /// <summary>
    /// Wrapper for an integer that can only be incremented. Useful for operation counters.
    /// </summary>
    public sealed class IncrementOnlyInt
    {
        private int _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="IncrementOnlyInt"/> class.
        /// </summary>
        public IncrementOnlyInt()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IncrementOnlyInt"/> class.
        /// </summary>
        /// <param name="value">Initial value to increment from.</param>
        public IncrementOnlyInt(int value)
        {
            _value = value;
        }

        /// <summary>
        /// Gets the current value of the counter.
        /// </summary>
        public int Value => _value;

        /// <summary>
        /// Increments the counter by one.
        /// </summary>
        /// <returns>
        /// The incremented value.
        /// </returns>
        public int Increment()
        {
            return Interlocked.Increment(ref _value);
        }
    }
}
