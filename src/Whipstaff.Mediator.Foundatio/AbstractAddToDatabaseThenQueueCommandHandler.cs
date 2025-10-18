// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Foundatio.Queues;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Whipstaff.Mediator.Foundatio
{
    /// <summary>
    /// Abstract logic for a mediator request to insert a record to a database then send a message on a queue.
    /// </summary>
    /// <typeparam name="TCommand">The type for the mediator command.</typeparam>
    /// <typeparam name="TDbContext">The type for the entity framework database context.</typeparam>
    /// <typeparam name="TEntityFrameworkEntity">The type for the entity being inserted into the database.</typeparam>
    /// <typeparam name="TQueueMessage">The type for the message being added to the queue.</typeparam>
    public abstract class AbstractAddToDatabaseThenQueueCommandHandler<TCommand, TDbContext, TEntityFrameworkEntity, TQueueMessage> : IRequestHandler<TCommand, string>
        where TDbContext : DbContext
        where TCommand : IRequest<string>
        where TEntityFrameworkEntity : class
        where TQueueMessage : class
    {
        private readonly Func<Task<TDbContext>> _dbContextFactory;
        private readonly IQueue<TQueueMessage> _queue;
        private readonly ILogger<AbstractAddToDatabaseThenQueueCommandHandler<TCommand, TDbContext, TEntityFrameworkEntity, TQueueMessage>> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractAddToDatabaseThenQueueCommandHandler{TCommand,TDbContext,TEntityFrameworkEntity,TQueueMessage}"/> class.
        /// </summary>
        /// <param name="dbContextFactory">Factory for creating a database context instance.</param>
        /// <param name="queue">The queue to add the requests to.</param>
        /// <param name="logger">Logging framework instance.</param>
        protected AbstractAddToDatabaseThenQueueCommandHandler(
            Func<Task<TDbContext>> dbContextFactory,
            IQueue<TQueueMessage> queue,
            ILogger<AbstractAddToDatabaseThenQueueCommandHandler<TCommand, TDbContext, TEntityFrameworkEntity, TQueueMessage>> logger)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
            _queue = queue ?? throw new ArgumentNullException(nameof(queue));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public async ValueTask<string> Handle(
            TCommand request,
            CancellationToken cancellationToken)
        {
            using (var dbContext = await _dbContextFactory().ConfigureAwait(false))
            {
                _logger.LogDebug("Getting DbSet");
                var dbSet = GetDbSet(dbContext);

                _logger.LogDebug("Getting entity to add to database");
                var entityToAdd = GetEntityToAddToDatabase(request);

                _logger.LogDebug("Adding entity to database change tracking graph");
                _ = await dbSet.AddAsync(
                    entityToAdd,
                    CancellationToken.None)
                    .ConfigureAwait(false);

                _logger.LogDebug("Saving entity to database");
                var saveChangesResult = dbContext.SaveChangesAsync(CancellationToken.None)
                    .ConfigureAwait(false);

                _logger.LogDebug("DbContext Save Changes Result: {SaveChangesResult}", saveChangesResult);

                _logger.LogDebug("Getting queue message");
                var queueMessage = GetQueueMessage(
                    request,
                    entityToAdd);

                _logger.LogDebug("Getting queue entry options");
                var queueEntryOptions = GetQueueEntryOptions(
                    request,
                    entityToAdd);

                _logger.LogDebug("Queueing message");
                var queueResult = await _queue.EnqueueAsync(
                        queueMessage,
                        queueEntryOptions)
                    .ConfigureAwait(false);

                _logger.LogDebug("Queue Enqueue Result: {QueueResult}", queueResult);

                return queueResult;
            }
        }

        /// <summary>
        /// Gets the DbSet the entity is to be inserted into.
        /// </summary>
        /// <param name="dbContext">database context instance.</param>
        /// <returns>The DbSet entity.</returns>
        protected abstract DbSet<TEntityFrameworkEntity> GetDbSet(TDbContext dbContext);

        /// <summary>
        /// Gets the entity to add to the database based upon the request.
        /// </summary>
        /// <param name="request">The incoming mediator request.</param>
        /// <returns>The entity to insert.</returns>
        protected abstract TEntityFrameworkEntity GetEntityToAddToDatabase(TCommand request);

        /// <summary>
        /// Gets the queue entry options, if required.
        /// </summary>
        /// <param name="request">The original mediator request.</param>
        /// <param name="entityInserted">The entity inserted into the database.</param>
        /// <returns>Queue entry options, or null if not required.</returns>
        protected abstract QueueEntryOptions? GetQueueEntryOptions(
            TCommand request,
            TEntityFrameworkEntity entityInserted);

        /// <summary>
        /// Gets the message to send to the queue.
        /// </summary>
        /// <param name="request">The original mediator request.</param>
        /// <param name="entityInserted">The entity inserted into the database.</param>
        /// <returns>Queue Message.</returns>
        protected abstract TQueueMessage GetQueueMessage(
            TCommand request,
            TEntityFrameworkEntity entityInserted);
    }
}
