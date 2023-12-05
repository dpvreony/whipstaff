// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Whipstaff.Core.Entities;

namespace Whipstaff.EntityFramework.RowVersionSaving
{
    /// <summary>
    /// Save Changes Interceptor for setting the RowVersion on entities implementing <see cref="ILongRowVersion"/>. This is useful for providers such as Sqlite that don't
    /// natively support concurrency tokens.
    /// </summary>
    public sealed class RowVersionSaveChangesInterceptor : SaveChangesInterceptor
    {
        /// <inheritdoc/>
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            ArgumentNullException.ThrowIfNull(eventData);

            if (eventData.Context == null)
            {
                return result;
            }

            var changes = eventData.Context.ChangeTracker.Entries();

            foreach (var entityEntry in changes)
            {
                if (entityEntry.State is not EntityState.Added
                    and not EntityState.Modified)
                {
                    continue;
                }

                if (entityEntry.Entity is not ILongRowVersion unsignedLongRowVersion)
                {
                    continue;
                }

                unsignedLongRowVersion.RowVersion = (ulong)DateTime.UtcNow.Ticks;
            }

            return result;
        }

        /// <inheritdoc/>
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            return new ValueTask<InterceptionResult<int>>(
                Task.Run(
                    () => SavingChanges(
                        eventData,
                        result),
                    cancellationToken));
        }
    }
}
