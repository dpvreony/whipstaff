// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Whipstaff.Mediator.EntityFrameworkCore
{
    /// <summary>
    /// Base class for fetching a single record from mediator.
    /// </summary>
    /// <typeparam name="TQuery">The request type for the Mediator query.</typeparam>
    /// <typeparam name="TResponse">The response type coming out of Mediator.</typeparam>
    /// <typeparam name="TDbContext">The type for the Entity Framework DB Context.</typeparam>
    /// <typeparam name="TEntity">The type for the Entity Framework DB Set.</typeparam>
    public abstract class AbstractViewQueryHandler<TQuery, TResponse, TDbContext, TEntity> : IQueryHandler<TQuery, TResponse?>
        where TDbContext : DbContext
        where TEntity : class
        where TQuery : IQuery<TResponse?>
    {
        /// <inheritdoc />
        public async ValueTask<TResponse?> Handle(TQuery query, CancellationToken cancellationToken)
        {
            using (var dbContext = GetDbContext())
            {
                var dbSet = GetDbSet(dbContext);
                var result = await dbSet.TagWith(nameof(AbstractViewQueryHandler<TQuery, TResponse, TDbContext, TEntity>)).Where(GetWherePredicateExpression())
                    .Select(GetSelectorExpression())
                    .FirstOrDefaultAsync(cancellationToken)
                    .ConfigureAwait(false);

                return result;
            }
        }

        /// <summary>
        /// Gets the selector expression for use in the EF query. Used for mapping EF Core entities to an output POCO object.
        /// </summary>
        /// <returns>Expression representing the output selection.</returns>
        protected abstract Expression<Func<TEntity, int, TResponse>> GetSelectorExpression();

        /// <summary>
        /// Gets the expression representing the where filter predicate.
        /// </summary>
        /// <returns>Expression representing the where clause.</returns>
        protected abstract Expression<Func<TEntity, int, bool>> GetWherePredicateExpression();

        /// <summary>
        /// Gets the DBContext instance for use in the query.
        /// </summary>
        /// <returns>DBContext instance.</returns>
        protected abstract TDbContext GetDbContext();

        /// <summary>
        /// Gets the DBSet for use in the query.
        /// </summary>
        /// <param name="dbContext">The DB Context instance to use.</param>
        /// <returns>The DBSet.</returns>
        protected abstract DbSet<TEntity> GetDbSet(TDbContext dbContext);
    }
}
