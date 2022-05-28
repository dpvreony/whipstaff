// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// DHGMS Solutions and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Core.Entities
{
    /// <summary>
    /// Represents a GB Post Code.
    /// </summary>
    public interface IGbPostCode
    {
        /// <summary>
        /// Gets the outward code section.
        /// </summary>
        string OutwardCode { get; }

        /// <summary>
        /// Gets the inward code section.
        /// </summary>
        string InwardCode { get; }
    }
}
