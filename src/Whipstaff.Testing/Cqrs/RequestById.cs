// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Mediator;
using Whipstaff.Core.Entities;

namespace Whipstaff.Testing.Cqrs
{
    /// <summary>
    /// Represents a Test Request by an int 32.
    /// </summary>
    public readonly struct RequestById : IQuery<int?>, IIntId, IEquatable<RequestById>
    {
        /// <inheritdoc />
        public int Id { get; init; }

        /// <summary>
        /// Determines whether two specified instances of <see cref="RequestById"/> are equal.
        /// </summary>
        /// <param name="left">The first instance to compare.</param>
        /// <param name="right">The second instance to compare.</param>
        /// <returns><c>true</c> if the instances are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(RequestById left, RequestById right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two specified instances of <see cref="RequestById"/> are not equal.
        /// </summary>
        /// <param name="left">The first instance to compare.</param>
        /// <param name="right">The second instance to compare.</param>
        /// <returns><c>true</c> if the instances are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(RequestById left, RequestById right)
        {
            return !(left == right);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is RequestById other && Equals(other);
        }

        /// <inheritdoc/>
        public bool Equals(RequestById other)
        {
            return Id == other.Id;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return Id;
        }
    }
}
