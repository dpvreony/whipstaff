// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Whipstaff.Core.Entities;

namespace Whipstaff.EntityFramework.MultiTenancy.SingleDatabase
{
    /// <summary>
    /// Intercepts save changes operations to enforce tenant constraints.
    /// </summary>
    /// <typeparam name="TTenantType">The type of the tenant id.</typeparam>
    public sealed partial class TenantGuardSaveChangesInterceptor<TTenantType> : SaveChangesInterceptor
        where TTenantType : struct
    {
        private readonly ITenantIdentifierProvider<TTenantType> _tenantIdentifierProvider;
        private readonly ILogger<TenantGuardSaveChangesInterceptor<TTenantType>> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TenantGuardSaveChangesInterceptor{TTenantType}"/> class.
        /// </summary>
        /// <param name="tenantIdentifierProvider">The tenant identifier provider.</param>
        /// <param name="logger">The logger.</param>
        public TenantGuardSaveChangesInterceptor(ITenantIdentifierProvider<TTenantType> tenantIdentifierProvider, ILogger<TenantGuardSaveChangesInterceptor<TTenantType>> logger)
        {
            ArgumentNullException.ThrowIfNull(tenantIdentifierProvider);
            ArgumentNullException.ThrowIfNull(logger);
            _tenantIdentifierProvider = tenantIdentifierProvider;
            _logger = logger;
        }

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

                if (entityEntry.Entity is not ITenantId<TTenantType> tenantEntity)
                {
                    continue;
                }

                if (!tenantEntity.TenantId.Equals(_tenantIdentifierProvider.TenantId))
                {
                    LogTenantMismatch(_logger, tenantEntity.TenantId, _tenantIdentifierProvider.TenantId);
                    throw new InvalidOperationException("Tenant ID mismatch.");
                }
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

        [LoggerMessage(
            EventId = 25,
            Level = LogLevel.Error,
            Message = "Tenant ID mismatch. Entity Tenant ID: {EntityTenantId}, Provider Tenant ID: {ProviderTenantId}")]
        private static partial void LogTenantMismatch(
            ILogger logger,
            TTenantType entityTenantId,
            TTenantType providerTenantId);
    }
}
