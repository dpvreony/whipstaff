// Copyright (c) 2020 DHGMS Solutions and Contributors. All rights reserved.
// DHGMS Solutions and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Whipstaff.MediatR.EntityFrameworkCore
{
    /// <summary>
    /// MediatR request handler for acting on an entity framework dbset.
    /// </summary>
    /// <typeparam name="TRequest">The type for the MediatR Request.</typeparam>
    /// <typeparam name="TDbContext">The type for the Entity Framework DB Context.</typeparam>
    /// <typeparam name="TEntity">The type for the POCO object.</typeparam>
    public abstract class ActOnDbSetRequestHandler<TRequest, TDbContext, TEntity> : IRequestHandler<TRequest>
        where TDbContext : DbContext
        where TRequest : IRequest<Unit>
    {
        private readonly Func<Task<TDbContext>> _dbContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActOnDbSetRequestHandler{TRequest, TDbContext, TEntity}"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The factory for the database context.</param>
        protected ActOnDbSetRequestHandler(Func<Task<TDbContext>> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        /// <inheritdoc/>
        public async Task<Unit> Handle(TRequest request, CancellationToken cancellationToken)
        {
            using (var dbContext = await _dbContextFactory().ConfigureAwait(false))
            {
                var dbQuery = await GetQueryAsync(dbContext).ConfigureAwait(false);
                foreach (var requestEfModel in dbQuery)
                {
                    await ActOnItemAsync(requestEfModel).ConfigureAwait(false);
                }

                if (dbContext.ChangeTracker.HasChanges())
                {
                    await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                }
            }

            return Unit.Value;
        }

        /// <summary>
        /// Gets the query to be executed.
        /// </summary>
        /// <param name="dbContext">The Database Context.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        protected abstract Task<IQueryable<TEntity>> GetQueryAsync(TDbContext dbContext);

        /// <summary>
        /// Allows acting on a strongly typed entity.
        /// </summary>
        /// <param name="item">The item to act on.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        protected abstract Task ActOnItemAsync(TEntity item);
    }
}