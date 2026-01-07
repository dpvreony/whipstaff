// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Whipstaff.Core.Entities;

namespace Whipstaff.Testing.EntityFramework.DbSets
{
    /// <summary>
    /// Represents a base db set.
    /// </summary>
    public abstract class AbstractDbSet : IIntId, IModifiable, ILongRowVersion
    {
        /// <inheritdoc/>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public int Value { get; set; }

        /// <inheritdoc/>
        public DateTimeOffset Created { get; set; }

        /// <inheritdoc/>
        public DateTimeOffset Modified { get; set; }

        /// <inheritdoc/>
        public ulong RowVersion { get; set; }
    }
}
