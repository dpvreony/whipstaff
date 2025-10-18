// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DynamicData;
using Microsoft.EntityFrameworkCore;

namespace Whipstaff.Mediator.EntityFrameworkCore
{
    /// <summary>
    /// Base class for an EFCore List Operation.
    /// </summary>
    /// <typeparam name="TQuery">The type for the query.</typeparam>
    /// <typeparam name="TResponse">The type for the CQRS response.</typeparam>
    /// <typeparam name="TDbContext">The type for the Entity Framework DB Context.</typeparam>
    /// <typeparam name="TEntity">The type for the Entity Framework Entity being queried.</typeparam>
    /// <typeparam name="TKey">The type for the Entity Primary Key.</typeparam>
    public abstract class AbstractListQueryHandler<TQuery, TResponse, TDbContext, TEntity, TKey> : IQueryHandler<TQuery, TResponse[]>
        where TDbContext : DbContext
        where TEntity : class
        where TQuery : IQuery<TResponse[]>, IPageRequest
    {
        /// <inheritdoc />
        public async Task<TResponse[]?> Handle(TQuery request, CancellationToken cancellationToken)
        {
            using (var dbContext = GetDbContext())
            {
                var dbSet = GetDbSet(dbContext);
                var query = dbSet.TagWith(nameof(AbstractListQueryHandler<TQuery, TResponse, TDbContext, TEntity, TKey>)).Where(GetWherePredicateExpression());

                var orderingExpression = GetOrderingExpression();
                if (orderingExpression != null)
                {
                    query = GetWhetherOrderingIsByDescending()
                        ? query.OrderByDescending(orderingExpression)
                        : query.OrderBy(orderingExpression);
                }

                var pageNumber = request.Page;
                var pageSize = request.Size;

                if (pageNumber > 1 && pageSize > 1)
                {
                    var skipCount = pageSize * pageNumber;

                    query = query.Skip(skipCount);
                }

                query = query.Take(pageSize);

                var result = await query.Select(GetSelectorExpression())
                    .ToArrayAsync(cancellationToken)
                    .ConfigureAwait(false);

                return result;
            }
        }

        /// <summary>
        /// Gets a flag indicating whether the ordering is descending or not.
        /// </summary>
        /// <returns>True if descending, otherwise false.</returns>
        protected abstract bool GetWhetherOrderingIsByDescending();

        /// <summary>
        /// Gets the ordering expression for use in the EF query.
        /// </summary>
        /// <returns>Expression representing the query ordering.</returns>
        protected abstract Expression<Func<TEntity, TKey>>? GetOrderingExpression();

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
