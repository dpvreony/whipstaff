// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dhgms.AspNetCoreContrib.App.Features.Mediatr.EfCrud
{
    /// <summary>
    /// Base class for fetching a single record from mediatr.
    /// </summary>
    /// <typeparam name="TRequest">The request type for the MediatR query.</typeparam>
    /// <typeparam name="TResponse">The response type coming out of MediatR.</typeparam>
    /// <typeparam name="TDbContext">The type for the Entity Framework DB Context.</typeparam>
    /// <typeparam name="TEntity">The type for the Entity Framework DB Set.</typeparam>
    public abstract class BaseViewRequestHandler<TRequest, TResponse, TDbContext, TEntity> : IRequestHandler<TRequest, TResponse>
        where TDbContext : DbContext
        where TEntity : class
        where TRequest : IRequest<TResponse>
    {
        /// <inheritdoc />
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            using (var dbContext = GetDbContext())
            {
                var dbSet = GetDbSet(dbContext);
                var result = await dbSet.Where(GetWherePredicateExpression())
                    .Select(GetSelectorExpression())
                    .FirstOrDefaultAsync(cancellationToken)
                    .ConfigureAwait(false);

                return result;
            }
        }

        /// <summary>
        /// Get the expression used for selecting records.
        /// </summary>
        /// <returns></returns>
        protected abstract Expression<Func<TEntity, int, TResponse>> GetSelectorExpression();

        protected abstract Expression<Func<TEntity, int, bool>> GetWherePredicateExpression();

        protected abstract TDbContext GetDbContext();

        protected abstract DbSet<TEntity> GetDbSet(TDbContext dbContext);
    }
}
