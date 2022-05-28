// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// DHGMS Solutions and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Core.Entities
{
    /// <summary>
    /// Represents an object that has an Identifier.
    /// </summary>
    /// <typeparam name="T">The type for the Id.</typeparam>
    public interface IId<out T>
    {
        /// <summary>
        /// Gets the Id.
        /// </summary>
        T Id { get; }
    }
}