// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Whipstaff.EntityFramework.SmokeTest
{
    /// <summary>
    /// Abstract class for checking DBSets.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the DbContext being tested.</typeparam>
    public abstract class AbstractDbSetChecker<TDbContext>
        where TDbContext : DbContext
    {
        private readonly DbSetCheckerLogMessageActionsWrapper<TDbContext> _logMessageActionsWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractDbSetChecker{TDbContext}"/> class.
        /// </summary>
        /// <param name="logMessageActionsWrapper">Log Message Wrapper instance.</param>
        protected AbstractDbSetChecker(DbSetCheckerLogMessageActionsWrapper<TDbContext> logMessageActionsWrapper)
        {
            ArgumentNullException.ThrowIfNull(logMessageActionsWrapper);
            _logMessageActionsWrapper = logMessageActionsWrapper;
        }

        /// <summary>
        /// Carries out the testing of DB Sets.
        /// </summary>
        /// <param name="dbContext">Instance of the DbContext to test.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public abstract Task CheckDbSetsAsync(TDbContext dbContext);

        /// <summary>
        /// Carries out the test of an individual Db Set.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity being tested.</typeparam>
        /// <param name="dbSet">The Db Set to test.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected async Task CheckDbSetAsync<TEntity>(DbSet<TEntity> dbSet)
            where TEntity : class
        {
            try
            {
                _logMessageActionsWrapper.StartingTestOfDbSet(typeof(TEntity));
                _ = await dbSet.FirstOrDefaultAsync().ConfigureAwait(false);
            }
#pragma warning disable CA1031
            catch (Exception ex)
#pragma warning restore CA1031
            {
                _logMessageActionsWrapper.FailedToTestDbSet(ex, typeof(TEntity));
            }
        }
    }
}
