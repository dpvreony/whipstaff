// Copyright (c) 2020 DHGMS Solutions and Contributors. All rights reserved.
// DHGMS Solutions and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Whipstaff.Core.Entities
{
    /// <summary>
    /// Entity Interface for a record that supports tracking creation.
    /// </summary>
    public interface ICreateable
    {
        /// <summary>
        /// Gets when the entity was created.
        /// </summary>
        DateTime Created { get; }
    }
}