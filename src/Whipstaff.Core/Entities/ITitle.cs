// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Core.Entities
{
    /// <summary>
    /// Represents an entity that holds a title.
    /// </summary>
    public interface ITitle
    {
        /// <summary>
        /// Gets the title.
        /// </summary>
        string? Title { get; }
    }
}
