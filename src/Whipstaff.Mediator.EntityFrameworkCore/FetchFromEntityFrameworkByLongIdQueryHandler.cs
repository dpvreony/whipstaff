// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Whipstaff.Core.Entities;

namespace Whipstaff.Mediator.EntityFrameworkCore
{
    /// <summary>
    /// Mediator request handler for acting on an entity framework dbset.
    /// </summary>
    /// <typeparam name="TQuery">The type for the Mediator Query.</typeparam>
    /// <typeparam name="TDbContext">The type for the Entity Framework DB Context.</typeparam>
    /// <typeparam name="TEntity">The type for the POCO object.</typeparam>
    /// <typeparam name="TResult">The type for the Result.</typeparam>
    public abstract class FetchFromEntityFrameworkByLongIdQueryHandler<TQuery, TDbContext, TEntity, TResult>
        : FetchFromEntityFrameworkQueryHandler<TQuery, TDbContext, TEntity, TResult>
        where TDbContext : DbContext
        where TQuery : IQuery<TResult>, ILongId
        where TEntity : class, ILongId
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FetchFromEntityFrameworkByLongIdQueryHandler{TQuery, TDbContext, TEntity,TResult}"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The factory for the database context.</param>
        protected FetchFromEntityFrameworkByLongIdQueryHandler(IDbContextFactory<TDbContext> dbContextFactory)
            : base(dbContextFactory)
        {
        }

        /// <inheritdoc/>
        protected sealed override Expression<Func<TEntity, bool>> GetWherePredicate(TQuery request)
        {
            return entity => entity.Id == request.Id;
        }

        /// <inheritdoc/>
        protected override Task<TResult?> GetResultAsync(IQueryable<TResult?> queryable, CancellationToken cancellationToken)
        {
            return queryable.TagWith(nameof(FetchFromEntityFrameworkByLongIdQueryHandler<TQuery, TDbContext, TEntity, TResult>))
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
