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
    public sealed class SmokeTestBackgroundWorker<TDbContext, TDbSetChecker> : BackgroundService
        where TDbContext : DbContext
        where TDbSetChecker : AbstractDbSetChecker<TDbContext>
    {
        private readonly IDbContextFactory<TDbContext> _dbContextFactory;
        private readonly TDbSetChecker _dbSetChecker;

        public SmokeTestBackgroundWorker(IDbContextFactory<TDbContext> dbContextFactory, TDbSetChecker dbSetChecker)
        {
            ArgumentNullException.ThrowIfNull(dbContextFactory);
            ArgumentNullException.ThrowIfNull(dbSetChecker);
            _dbContextFactory = dbContextFactory;
            _dbSetChecker = dbSetChecker;
        }

        /// <inheritdoc/>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var dbContext = await _dbContextFactory.CreateDbContextAsync(stoppingToken).ConfigureAwait(false))
            {
                try
                {
                    _logMessagActionsWrappper.StartingDbSetChecker();
                    var dbSets = _dbSetChecker.CheckDbSets(dbContext);
                    _logMessagActionsWrappper.CompletedDbSetChecker();
                }
                catch (Exception ex)
                {
                    _logMessagActionsWrappper.FailureDbSetChecker(ex);
                }
            }
        }
    }
}
