// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Whipstaff.Core.Entities;
using Whipstaff.EntityFramework.Extensions;

namespace Whipstaff.Healthchecks.EntityFramework
{
    /// <summary>
    /// Healthcheck to fetch the max row version from a table.
    /// </summary>
    /// <typeparam name="TDbContext">Type for the EF Core DB Context.</typeparam>
    /// <typeparam name="TEntity">Type for the EF Core DBSet Entity.</typeparam>
    public class FetchMaxRowVersionFromTableHealthCheck<TDbContext, TEntity> : IHealthCheck
        where TDbContext : DbContext
        where TEntity : class, ILongRowVersion
    {
        private readonly IDbContextFactory<TDbContext> _dbContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="FetchMaxRowVersionFromTableHealthCheck{TDbContext, TEntity}"/> class.
        /// </summary>
        /// <param name="dbContextFactory">Factory for the EF DBContext.</param>
#pragma warning disable GR0027 // Constructor should have a logging framework instance as the final parameter.
        public FetchMaxRowVersionFromTableHealthCheck(IDbContextFactory<TDbContext> dbContextFactory)
        {
            ArgumentNullException.ThrowIfNull(dbContextFactory);
            _dbContextFactory = dbContextFactory;
        }
#pragma warning restore GR0027 // Constructor should have a logging framework instance as the final parameter.

        /// <inheritdoc/>
        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            using (var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken)
                       .ConfigureAwait(false))
            {
                dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var maxRowVersion = await dbContext.Set<TEntity>()
                    .GetMaxRowVersionOrDefaultAsync()
                    .ConfigureAwait(false);

                return new HealthCheckResult(
                    maxRowVersion != null ? HealthStatus.Healthy : HealthStatus.Degraded,
                    $"MaxRowVersion - {nameof(TEntity)}:{maxRowVersion}");
            }
        }
    }
}
