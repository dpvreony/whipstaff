// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Whipstaff.MediatR.EntityFrameworkCore
{
    /// <summary>
    /// MediatR request handler for acting on an entity framework dbset.
    /// </summary>
    /// <typeparam name="TQuery">The type for the MediatR Query.</typeparam>
    /// <typeparam name="TDbContext">The type for the Entity Framework DB Context.</typeparam>
    /// <typeparam name="TEntity">The type for the POCO object.</typeparam>
    /// <typeparam name="TResult">The type for the Result.</typeparam>
    public abstract class FetchFromEntityFrameworkQueryHandler<TQuery, TDbContext, TEntity, TResult> : IQueryHandler<TQuery, TResult?>
        where TDbContext : DbContext
        where TQuery : IQuery<TResult?>
        where TEntity : class
    {
        private readonly IDbContextFactory<TDbContext> _dbContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="FetchFromEntityFrameworkQueryHandler{TRequest, TDbContext, TEntity,TResult}"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The factory for the database context.</param>
        protected FetchFromEntityFrameworkQueryHandler(IDbContextFactory<TDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        /// <inheritdoc/>
        public async Task<TResult?> Handle(TQuery request, CancellationToken cancellationToken)
        {
            using (var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false))
            {
                dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var query = GetDbSet(dbContext)
                    .TagWith(nameof(FetchFromEntityFrameworkQueryHandler<TQuery, TDbContext, TEntity, TResult>))
                    .Where(GetWherePredicate(request))
                    .Select(GetSelector());

                return await GetResultAsync(query, cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Gets the Where predicate for the query.
        /// </summary>
        /// <param name="request">MediatR request.</param>
        /// <returns>Where predicate for the query.</returns>
        protected abstract Expression<Func<TEntity, bool>> GetWherePredicate(TQuery request);

        /// <summary>
        /// Gets the Selector for the result output from the query.
        /// </summary>
        /// <returns>Selector for the result output from the query.</returns>
        protected abstract Expression<Func<TEntity, TResult?>> GetSelector();

        /// <summary>
        /// Gets the DBSet from the DBContext that contains the relevant entity.
        /// </summary>
        /// <param name="dbContext">Database Context.</param>
        /// <returns>DBSet from the DBContext that contains the relevant entity.</returns>
        protected abstract DbSet<TEntity> GetDbSet(TDbContext dbContext);

        /// <summary>
        /// Gets the result from the query.
        /// </summary>
        /// <param name="queryable">Query to process.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Result from query.</returns>
        protected abstract Task<TResult?> GetResultAsync(IQueryable<TResult?> queryable, CancellationToken cancellationToken);
    }
}
