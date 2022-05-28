// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// DHGMS Solutions and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Core.Entities
{
    /// <summary>
    /// Represents an object that holds the GB Post Code as a simple string.
    /// </summary>
    public interface IGbPostCodeAsString
    {
        /// <summary>
        /// Gets the postcode.
        /// </summary>
        string PostCode { get; }
    }
}