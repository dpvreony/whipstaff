// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Whipstaff.Mediator.EntityFrameworkCore
{
    /// <summary>
    /// Mediator request handler for acting on an entity framework dbset.
    /// </summary>
    /// <typeparam name="TQuery">The type for the Mediator Query.</typeparam>
    /// <typeparam name="TDbContext">The type for the Entity Framework DB Context.</typeparam>
    /// <typeparam name="TEntity">The type for the POCO object.</typeparam>
    /// <typeparam name="TResult">The type for the Result.</typeparam>
    public abstract class FetchFromEntityFrameworkQueryHandler<TQuery, TDbContext, TEntity, TResult> :
        FetchFromEntityFrameworkQueryHandler<TQuery, TDbContext, TEntity, TResult, TResult>
        where TDbContext : DbContext
        where TQuery : IQuery<TResult?>
        where TEntity : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FetchFromEntityFrameworkQueryHandler{TRequest, TDbContext, TEntity,TResult}"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The factory for the database context.</param>
        protected FetchFromEntityFrameworkQueryHandler(IDbContextFactory<TDbContext> dbContextFactory)
            : base(dbContextFactory)
        {
        }
    }
}
