// Copyright (c) 2020 DHGMS Solutions and Contributors. All rights reserved.
// DHGMS Solutions and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Whipstaff.Core.Entities;

namespace Whipstaff.MediatR.EntityFrameworkCore
{
    /// <summary>
    /// MediatR request handler for acting on an entity framework dbset.
    /// </summary>
    /// <typeparam name="TRequest">The type for the MediatR Request.</typeparam>
    /// <typeparam name="TDbContext">The type for the Entity Framework DB Context.</typeparam>
    /// <typeparam name="TEntity">The type for the POCO object.</typeparam>
    /// <typeparam name="TResult">The type for the Result.</typeparam>
    public abstract class FetchFromEntityFrameworkByInt32IdRequestHandler<TRequest, TDbContext, TEntity, TResult>
        : FetchFromEntityFrameworkRequestHandler<TRequest, TDbContext, TEntity, TResult>
        where TDbContext : DbContext
        where TRequest : IRequest<TResult>, IIntId
        where TEntity : class, IIntId
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FetchFromEntityFrameworkByInt32IdRequestHandler{TRequest, TDbContext, TEntity,TResult}"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The factory for the database context.</param>
        protected FetchFromEntityFrameworkByInt32IdRequestHandler(Func<Task<TDbContext>> dbContextFactory)
            : base(dbContextFactory)
        {
        }

        /// <inheritdoc/>
        protected sealed override Expression<Func<TEntity, bool>> GetWherePredicate(TRequest request)
        {
            return entity => entity.Id == request.Id;
        }

        /// <inheritdoc/>
        protected override Task<TResult?> GetResultAsync(IQueryable<TResult> queryable, CancellationToken cancellationToken)
        {
            return queryable.FirstOrDefaultAsync(cancellationToken);
        }
    }
}
