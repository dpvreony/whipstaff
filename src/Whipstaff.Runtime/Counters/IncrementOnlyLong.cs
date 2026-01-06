// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;

namespace Whipstaff.Runtime.Counters
{
    /// <summary>
    /// Wrapper for a long that can only be incremented. Useful for operation counters.
    /// </summary>
    public struct IncrementOnlyLong : IEquatable<IncrementOnlyLong>
    {
        private long _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="IncrementOnlyLong"/> struct.
        /// </summary>
        public IncrementOnlyLong()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IncrementOnlyLong"/> struct.
        /// </summary>
        /// <param name="value">Initial value to increment from.</param>
        public IncrementOnlyLong(int value)
        {
            _value = value;
        }

        /// <summary>
        /// Gets the current value of the counter.
        /// </summary>
        public long Value => _value;

        /// <summary>
        /// Determines whether two <see cref="IncrementOnlyLong"/> instances are equal.
        /// </summary>
        /// <param name="left">The first <see cref="IncrementOnlyLong"/> to compare.</param>
        /// <param name="right">The second <see cref="IncrementOnlyLong"/> to compare.</param>
        /// <returns>
        /// <c>true</c> if the specified instances are equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator ==(IncrementOnlyLong left, IncrementOnlyLong right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two <see cref="IncrementOnlyLong"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first <see cref="IncrementOnlyLong"/> to compare.</param>
        /// <param name="right">The second <see cref="IncrementOnlyLong"/> to compare.</param>
        /// <returns>
        /// <c>true</c> if the specified instances are not equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator !=(IncrementOnlyLong left, IncrementOnlyLong right)
        {
            return !(left == right);
        }

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

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is IncrementOnlyLong other && Equals(other);
        }

        /// <inheritdoc/>
        public bool Equals(IncrementOnlyLong other)
        {
            return _value == other._value;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}
