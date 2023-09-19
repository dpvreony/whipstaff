// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Whipstaff.Core.Logging;

namespace Whipstaff.EntityFramework.SmokeTest
{
    public abstract class AbstractDbSetChecker<TDbContext>
        where TDbContext : DbContext
    {
        private AbstractLogMessageActionsWrapper<AbstractDbSetChecker<TDbContext>, DbSetCheckerLogMessageActions> _logMessageActionsWrapper;

        protected AbstractDbSetChecker(AbstractLogMessageActionsWrapper<AbstractDbSetChecker<TDbContext>, DbSetCheckerLogMessageActions> logMessageActionsWrapper)
        {
            ArgumentNullException.ThrowIfNull(logMessageActionsWrapper);
            _logMessageActionsWrapper = logMessageActionsWrapper;
        }

        public abstract Task CheckDbSets(TDbContext dbContext);

        private async Task CheckDbSet<TEntity>(DbSet<TEntity> dbSet)
        {
            try
            {
                _logMessageActionsWrapper.StartingTestOfDbSet(typeof(TEntity));
                var result = await dbSet.FirstOrDefaultAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logMessageActionsWrapper.FailedToTestDbSet(ex, typeof(TEntity));
            }
        }
    }
}
