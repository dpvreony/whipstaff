// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Whipstaff.Core.Entities;

namespace Whipstaff.Testing.EntityFramework.DbSets
{
    /// <summary>
    /// Represents a table with a long primary key.
    /// </summary>
    public class FakeLongIdTableDbSet : ILongId, IModifiable, ILongRowVersion
    {
        /// <inheritdoc/>
        public long Id { get; set; }

        /// <inheritdoc/>
        public DateTimeOffset Created { get; set; }

        /// <inheritdoc/>
        public DateTimeOffset Modified { get; set; }

        /// <inheritdoc/>
        public long RowVersion { get; set; }
    }
}
