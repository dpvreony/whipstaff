// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Core.Entities
{
    /// <summary>
    /// Represents an entity that has a row version for concurrency checks.
    /// </summary>
    public interface ILongRowVersion
    {
        /// <summary>
        /// Gets or sets the row version.
        /// </summary>
        ulong RowVersion { get; set; }
    }
}
