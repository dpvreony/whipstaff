// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Whipstaff.Core.Entities;

namespace Whipstaff.MediatR.EntityFrameworkCore
{
    /// <summary>
    /// MediatR request handler for acting on an entity framework dbset.
    /// </summary>
    /// <typeparam name="TQuery">The type for the MediatR Request.</typeparam>
    /// <typeparam name="TDbContext">The type for the Entity Framework DB Context.</typeparam>
    /// <typeparam name="TEntity">The type for the POCO object.</typeparam>
    /// <typeparam name="TResult">The type for the Result.</typeparam>
    public sealed class FuncFetchFromEntityFrameworkByInt32IdQueryHandler<TQuery, TDbContext, TEntity, TResult> : FetchFromEntityFrameworkByInt32IdQueryHandler<TQuery, TDbContext, TEntity, TResult>
        where TDbContext : DbContext
        where TQuery : IQuery<TResult?>, IIntId
        where TEntity : class, IIntId
    {
        private readonly Expression<Func<TEntity, TResult?>> _selector;
        private readonly Func<TDbContext, DbSet<TEntity>> _dbSetFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="FuncFetchFromEntityFrameworkByInt32IdQueryHandler{TRequest, TDbContext, TEntity,TResult}"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The factory for the database context.</param>
        /// <param name="dbSetFunc">Function for selecting the DBSet from the Entity Framework Context.</param>
        /// <param name="selector">Selector for the result output.</param>
        public FuncFetchFromEntityFrameworkByInt32IdQueryHandler(
            IDbContextFactory<TDbContext> dbContextFactory,
            Func<TDbContext, DbSet<TEntity>> dbSetFunc,
            Expression<Func<TEntity, TResult?>> selector)
            : base(dbContextFactory)
        {
            _dbSetFunc = dbSetFunc;
            _selector = selector;
        }

        /// <inheritdoc />
        protected override IQueryable<TEntity> ExtendQueryable(IQueryable<TEntity> queryable)
        {
            return queryable;
        }

        /// <inheritdoc />
        protected override DbSet<TEntity> GetDbSet(TDbContext dbContext) => _dbSetFunc(dbContext);

        /// <inheritdoc />
        protected override Expression<Func<TEntity, TResult?>> GetSelector() => _selector;
    }
}
