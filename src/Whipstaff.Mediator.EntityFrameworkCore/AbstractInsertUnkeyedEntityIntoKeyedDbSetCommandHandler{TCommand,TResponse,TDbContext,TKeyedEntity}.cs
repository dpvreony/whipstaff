// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Whipstaff.Mediator.EntityFrameworkCore
{
    /// <summary>
    /// Abstract class for inserting an unkeyed entity into a <see cref="DbSet{TEntity}"/>.
    /// </summary>
    /// <typeparam name="TCommand">The type for the CQRS command.</typeparam>
    /// <typeparam name="TResponse">The type for the CQRS response.</typeparam>
    /// <typeparam name="TDbContext">The type for the <see cref="DbContext"/>.</typeparam>
    /// <typeparam name="TKeyedEntity">The type for the entity in the DbSet we will save to.</typeparam>
    public abstract class AbstractInsertUnkeyedEntityIntoKeyedDbSetCommandHandler<TCommand, TResponse, TDbContext, TKeyedEntity> : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
        where TDbContext : DbContext
        where TKeyedEntity : class
    {
        /// <inheritdoc/>
        public async ValueTask<TResponse> Handle(TCommand command, CancellationToken cancellationToken)
        {
            var entityToAdd = GetDbSetEntityFromUnkeyedEntity(command);

            using (var dbContext = GetDbContext())
            {
                var dbSet = GetDbSet(dbContext);

                _ = await dbSet.AddAsync(
                        entityToAdd,
                        CancellationToken.None)
                    .ConfigureAwait(false);

                var saveChangesResult = await dbContext.SaveChangesAsync(CancellationToken.None)
                    .ConfigureAwait(false);

                return GetResponse(entityToAdd, saveChangesResult);
            }
        }

        /// <summary>
        /// Gets the <see cref="DbContext"/> for use in inserting a record.
        /// </summary>
        /// <returns>EF Core database context.</returns>
        protected abstract TDbContext GetDbContext();

        /// <summary>
        /// Gets the target DbSet for inserting data.
        /// </summary>
        /// <param name="dbContext">EfCore database context to use.</param>
        /// <returns>DbSet to insert data to.</returns>
        protected abstract DbSet<TKeyedEntity> GetDbSet(TDbContext dbContext);

        /// <summary>
        /// Gets a Keyed entity for inserting into the DbSet.
        /// </summary>
        /// <param name="command">Command to convert.</param>
        /// <returns>Keyed entity to insert.</returns>
        protected abstract TKeyedEntity GetDbSetEntityFromUnkeyedEntity(TCommand command);

        /// <summary>
        /// Gets the response to return from the command.
        /// </summary>
        /// <param name="entityAdded">The entity that was inserted into the DbSet.</param>
        /// <param name="saveChangesResult">The count of records saved, should be 1 in most cases, unless you have relational keys being affected.</param>
        /// <returns>Response to return out of the command.</returns>
        protected abstract TResponse GetResponse(TKeyedEntity entityAdded, int saveChangesResult);
    }
}
