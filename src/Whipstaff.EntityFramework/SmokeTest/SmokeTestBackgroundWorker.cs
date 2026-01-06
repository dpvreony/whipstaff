// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Whipstaff.EntityFramework.SmokeTest
{
    /// <summary>
    /// Background Worker for running smoke tests on a DBContext.
    /// </summary>
    /// <typeparam name="TDbContext">The type for the DbContext.</typeparam>
    /// <typeparam name="TDbSetChecker">The type for the DbSet checker.</typeparam>
    public sealed class SmokeTestBackgroundWorker<TDbContext, TDbSetChecker> : BackgroundService
        where TDbContext : DbContext
        where TDbSetChecker : AbstractDbSetChecker<TDbContext>
    {
        private readonly IDbContextFactory<TDbContext> _dbContextFactory;
        private readonly TDbSetChecker _dbSetChecker;
        private readonly SmokeTestBackgroundWorkerLogMessageActionsWrapper<TDbContext, TDbSetChecker> _logMessageActionsWrappper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmokeTestBackgroundWorker{TDbContext, TDbSetChecker}"/> class.
        /// </summary>
        /// <param name="dbContextFactory">DBContext factory for connecting to the database to test.</param>
        /// <param name="dbSetChecker">The logic for running the test of Db Sets.</param>
        /// <param name="logMessageActionsWrappper">Log Message Actions Wrapper instance.</param>
        public SmokeTestBackgroundWorker(
            IDbContextFactory<TDbContext> dbContextFactory,
            TDbSetChecker dbSetChecker,
            SmokeTestBackgroundWorkerLogMessageActionsWrapper<TDbContext, TDbSetChecker> logMessageActionsWrappper)
        {
            ArgumentNullException.ThrowIfNull(dbContextFactory);
            ArgumentNullException.ThrowIfNull(dbSetChecker);
            ArgumentNullException.ThrowIfNull(logMessageActionsWrappper);

            _dbContextFactory = dbContextFactory;
            _dbSetChecker = dbSetChecker;
            _logMessageActionsWrappper = logMessageActionsWrappper;
        }

        /// <inheritdoc/>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var dbContext = await _dbContextFactory.CreateDbContextAsync(stoppingToken).ConfigureAwait(false))
            {
                try
                {
                    _logMessageActionsWrappper.StartingDbSetChecker();
                    await _dbSetChecker.CheckDbSetsAsync(dbContext)
                        .ConfigureAwait(false);
                    _logMessageActionsWrappper.CompletedDbSetChecker();
                }
#pragma warning disable CA1031
                catch (Exception ex)
#pragma warning restore CA1031
                {
                    _logMessageActionsWrappper.FailureOfDbSetChecker(ex);
                }
            }
        }
    }
}
