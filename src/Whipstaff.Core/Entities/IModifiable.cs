// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// DHGMS Solutions and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Whipstaff.Core.Entities
{
    /// <summary>
    /// Entity Interface for a record that supports tracking modification.
    /// </summary>
    public interface IModifiable : ICreateable
    {
        /// <summary>
        /// Gets when a record was modified.
        /// </summary>
        DateTime Modified { get; }
    }
}