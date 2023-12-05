// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Core.Entities
{
    /// <summary>
    /// Wrapper for encapsulating a built in type behind validation and type safety.
    /// For example a lot of domain objects can be represented as string which can
    /// make method signatures and\or POCO objects harder to follow and validate.
    ///
    /// By adding a wrapper that carry out the validation once you can simplify your
    /// logic and make method signatures \ POCO objects clearer, easier to test
    /// and easier to track changes on.
    /// </summary>
    /// <typeparam name="TValue">Type for the wrapped value.</typeparam>
    public interface IEntityAsType<out TValue>
    {
        /// <summary>
        /// Gets the wrapped value.
        /// </summary>
        TValue Value { get; }
    }
}
