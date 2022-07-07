// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Whipstaff.MediatR.EntityFrameworkCore
{
    /// <summary>
    /// MediatR request handler for acting on an entity framework dbset.
    /// </summary>
    /// <typeparam name="TRequest">The type for the MediatR Request.</typeparam>
    /// <typeparam name="TDbContext">The type for the Entity Framework DB Context.</typeparam>
    /// <typeparam name="TEntity">The type for the POCO object.</typeparam>
    public abstract class ActOnDbSetRequestHandler<TRequest, TDbContext, TEntity> : IRequestHandler<TRequest, int>
        where TDbContext : DbContext
        where TRequest : IRequest<int>
    {
        private readonly Func<Task<TDbContext>> _dbContextFactory;
        private readonly ILogger<ActOnDbSetRequestHandler<TRequest, TDbContext, TEntity>> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActOnDbSetRequestHandler{TRequest, TDbContext, TEntity}"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The factory for the database context.</param>
        /// <param name="logger">Logging framework instance.</param>
        protected ActOnDbSetRequestHandler(
            Func<Task<TDbContext>> dbContextFactory,
            ILogger<ActOnDbSetRequestHandler<TRequest, TDbContext, TEntity>> logger)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task<int> Handle(TRequest request, CancellationToken cancellationToken)
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
                    var saveResult = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
#pragma warning disable CA1848 // Use the LoggerMessage delegates
                    _logger.LogDebug("Save Result: {SaveResult}", saveResult);
#pragma warning restore CA1848 // Use the LoggerMessage delegates
                    return saveResult;
                }
            }

            return 0;
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
