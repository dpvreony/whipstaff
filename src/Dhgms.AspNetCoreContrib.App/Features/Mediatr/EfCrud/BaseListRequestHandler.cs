// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dhgms.AspNetCoreContrib.App.Features.Mediatr.EfCrud
{
    public abstract class BaseListRequestHandler<TRequest, TResponse, TDbContext, TEntity, TKey> : IRequestHandler<TRequest, TResponse[]>
        where TDbContext : DbContext
        where TEntity : class
        where TRequest : IRequest<TResponse>, IRequest<TResponse[]>
    {
        /// <inheritdoc />
        public async Task<TResponse[]> Handle(TRequest request, CancellationToken cancellationToken)
        {
            using (var dbContext = GetDbContext())
            {
                var dbSet = GetDbSet(dbContext);
                var query = dbSet.Where(GetWherePredicateExpression());

                var orderingExpression = GetOrderingExpression();
                query = GetWhetherOrderingIsByDescending()
                    ? query.OrderByDescending(orderingExpression)
                    : query.OrderBy(orderingExpression);

                var skipCount = 0;
                query = query.Skip(skipCount);

                var takeCount = 1;
                query = query.Take(takeCount);

                var result = await query.Select(GetSelectorExpression())
                    .ToArrayAsync(cancellationToken)
                    .ConfigureAwait(false);

                return result;
            }
        }

        protected abstract bool GetWhetherOrderingIsByDescending();

        protected abstract Expression<Func<TEntity, TKey>> GetOrderingExpression();

        protected abstract Expression<Func<TEntity, int, TResponse>> GetSelectorExpression();

        protected abstract Expression<Func<TEntity, int, bool>> GetWherePredicateExpression();

        protected abstract TDbContext GetDbContext();

        protected abstract DbSet<TEntity> GetDbSet(TDbContext dbContext);
    }
}
