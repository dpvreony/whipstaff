// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Whipstaff.Core.Entities;
using Whipstaff.Runtime.Exceptions;

namespace Whipstaff.EntityFramework.Extensions
{
    /// <summary>
    /// Extensions for Entity Framework DBContext.
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// Carries out an action on the database context and saves changes.
        /// </summary>
        /// <typeparam name="TDbContext">The type for the Database Context.</typeparam>
        /// <param name="dbContext">Database Context instance to act upon.</param>
        /// <param name="func">Action to carry out prior to calling save.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task ActOnDbContextAndSaveChangesAsync<TDbContext>(
            this TDbContext dbContext,
            Func<TDbContext, Task> func)
            where TDbContext : DbContext
        {
            ArgumentNullException.ThrowIfNull(dbContext);
            ArgumentNullException.ThrowIfNull(func);

            await func(dbContext).ConfigureAwait(false);
            _ = await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Carries out an action on the database context, saves changes and then returns a result defined by the action.
        /// </summary>
        /// <typeparam name="TDbContext">The type for the Database Context.</typeparam>
        /// <typeparam name="TResult">The type for the function result.</typeparam>
        /// <param name="dbContext">Database Context instance to act upon.</param>
        /// <param name="func">Action to carry out prior to calling save.</param>
        /// <returns>Result defined by the function.</returns>
        public static async Task<TResult> ActOnDbContextAndSaveChangesAsync<TDbContext, TResult>(
            this TDbContext dbContext,
            Func<TDbContext, Task<TResult>> func)
            where TDbContext : DbContext
        {
            ArgumentNullException.ThrowIfNull(dbContext);
            ArgumentNullException.ThrowIfNull(func);

            var result = await func(dbContext).ConfigureAwait(false);
            _ = await dbContext.SaveChangesAsync().ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Get or adds an entity to a DBSet.
        /// </summary>
        /// <typeparam name="TDbContext">The type for the Database Context.</typeparam>
        /// <typeparam name="TEntity">The type for the DBSet Entity.</typeparam>
        /// <param name="instance">Instance of the Database Context.</param>
        /// <param name="dbSetSelectorFunc">Selector for the database context.</param>
        /// <param name="predicate">Predicate for checking if the entity exists.</param>
        /// <param name="addEntityFactoryFunc">Function for creating the item if it's not found.</param>
        /// <returns>The matched or newly created entity.</returns>
        public static async Task<TEntity> GetOrAddAsync<TDbContext, TEntity>(
            this TDbContext instance,
            Func<TDbContext, DbSet<TEntity>> dbSetSelectorFunc,
            Expression<Func<TEntity, bool>> predicate,
            Func<TEntity> addEntityFactoryFunc)
            where TDbContext : DbContext
            where TEntity : class
        {
            ArgumentNullException.ThrowIfNull(instance);
            ArgumentNullException.ThrowIfNull(dbSetSelectorFunc);
            ArgumentNullException.ThrowIfNull(predicate);
            ArgumentNullException.ThrowIfNull(addEntityFactoryFunc);

            return await InternalGetOrAddAsync(
                instance,
                dbSetSelectorFunc,
                predicate,
                addEntityFactoryFunc)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Get or adds an entity to a DBSet when searching by id. Useful when you have keyed reference data to ensure exists.
        /// </summary>
        /// <typeparam name="TDbContext">The type for the Database Context.</typeparam>
        /// <typeparam name="TEntity">The type for the DBSet Entity.</typeparam>
        /// <param name="instance">Instance of the Database Context.</param>
        /// <param name="dbSetSelectorFunc">Selector for the database context.</param>
        /// <param name="id">The id to search for.</param>
        /// <param name="addEntityFactoryFunc">Function for creating the item if it's not found.</param>
        /// <returns>The matched or newly created entity.</returns>
        public static async Task<TEntity> GetOrAddByIdAsync<TDbContext, TEntity>(
            this TDbContext instance,
            Func<TDbContext, DbSet<TEntity>> dbSetSelectorFunc,
            int id,
            Func<TEntity> addEntityFactoryFunc)
            where TDbContext : DbContext
            where TEntity : class, IIntId
        {
            ArgumentNullException.ThrowIfNull(instance);
            ArgumentNullException.ThrowIfNull(dbSetSelectorFunc);
            ArgumentNullException.ThrowIfNull(addEntityFactoryFunc);

            if (id < 1)
            {
                throw new NumberTooLowInteger32Exception(nameof(id), 1, id);
            }

            return await InternalGetOrAddAsync(
                instance,
                dbSetSelectorFunc,
                x => x.Id == id,
                addEntityFactoryFunc)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Get or adds an entity to a DBSet when searching by id. Useful when you have keyed reference data to ensure exists.
        /// </summary>
        /// <typeparam name="TDbContext">The type for the Database Context.</typeparam>
        /// <typeparam name="TEntity">The type for the DBSet Entity.</typeparam>
        /// <param name="instance">Instance of the Database Context.</param>
        /// <param name="dbSetSelectorFunc">Selector for the database context.</param>
        /// <param name="id">The id to search for.</param>
        /// <param name="addEntityFactoryFunc">Function for creating the item if it's not found.</param>
        /// <returns>The matched or newly created entity.</returns>
        public static async Task<TEntity> GetOrAddByIdAsync<TDbContext, TEntity>(
            this TDbContext instance,
            Func<TDbContext, DbSet<TEntity>> dbSetSelectorFunc,
            long id,
            Func<TEntity> addEntityFactoryFunc)
            where TDbContext : DbContext
            where TEntity : class, ILongId
        {
            ArgumentNullException.ThrowIfNull(instance);
            ArgumentNullException.ThrowIfNull(dbSetSelectorFunc);
            ArgumentNullException.ThrowIfNull(addEntityFactoryFunc);

            if (id < 1)
            {
                throw new NumberTooLowInteger64Exception(nameof(id), 1, id);
            }

            return await InternalGetOrAddAsync(
                instance,
                dbSetSelectorFunc,
                x => x.Id == id,
                addEntityFactoryFunc)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Get or adds an entity to a DBSet when searching by id. Useful when you have keyed reference data to ensure exists.
        /// </summary>
        /// <typeparam name="TDbContext">The type for the Database Context.</typeparam>
        /// <typeparam name="TEntity">The type for the DBSet Entity.</typeparam>
        /// <param name="instance">Instance of the Database Context.</param>
        /// <param name="dbSetSelectorFunc">Selector for the database context.</param>
        /// <param name="id">The id to search for.</param>
        /// <param name="addEntityFactoryFunc">Function for creating the item if it's not found.</param>
        /// <returns>The matched or newly created entity.</returns>
        public static async Task<TEntity> GetOrAddByIdAsync<TDbContext, TEntity>(
            this TDbContext instance,
            Func<TDbContext, DbSet<TEntity>> dbSetSelectorFunc,
            Guid id,
            Func<TEntity> addEntityFactoryFunc)
            where TDbContext : DbContext
            where TEntity : class, IGuidId
        {
            ArgumentNullException.ThrowIfNull(instance);
            ArgumentNullException.ThrowIfNull(dbSetSelectorFunc);
            ArgumentNullException.ThrowIfNull(addEntityFactoryFunc);

            if (id == Guid.Empty)
            {
                throw new ArgumentException("Guid must not be empty");
            }

            return await InternalGetOrAddAsync(
                instance,
                dbSetSelectorFunc,
                x => x.Id == id,
                addEntityFactoryFunc)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Get or adds an entity to a DBSet when searching by name.
        /// </summary>
        /// <typeparam name="TDbContext">The type for the Database Context.</typeparam>
        /// <typeparam name="TEntity">The type for the DBSet Entity.</typeparam>
        /// <param name="instance">Instance of the Database Context.</param>
        /// <param name="dbSetSelectorFunc">Selector for the database context.</param>
        /// <param name="name">The name to search for.</param>
        /// <param name="addEntityFactoryFunc">Function for creating the item if it's not found.</param>
        /// <returns>The matched or newly created entity.</returns>
        public static async Task<TEntity> GetOrAddByNameAsync<TDbContext, TEntity>(
            this TDbContext instance,
            Func<TDbContext, DbSet<TEntity>> dbSetSelectorFunc,
            string name,
            Func<TEntity> addEntityFactoryFunc)
            where TDbContext : DbContext
            where TEntity : class, INameable
        {
            ArgumentNullException.ThrowIfNull(instance);
            ArgumentNullException.ThrowIfNull(dbSetSelectorFunc);
            ArgumentNullException.ThrowIfNull(addEntityFactoryFunc);

            return await InternalGetOrAddAsync(
                instance,
                dbSetSelectorFunc,
                x => x.Name == name,
                addEntityFactoryFunc)
                .ConfigureAwait(false);
        }

        internal static async Task<TEntity> InternalGetOrAddAsync<TDbContext, TEntity>(
            this TDbContext instance,
            Func<TDbContext, DbSet<TEntity>> dbSetSelectorFunc,
            Expression<Func<TEntity, bool>> predicate,
            Func<TEntity> addEntityFactoryFunc)
            where TDbContext : DbContext
            where TEntity : class
        {
            var d = dbSetSelectorFunc(instance);
            var item = await d.TagWithCallerMemberAndCallSite().FirstOrDefaultAsync(predicate)
                .ConfigureAwait(false);

            if (item != null)
            {
                return item;
            }

            item = addEntityFactoryFunc();

            _ = await d.AddAsync(item)
                .ConfigureAwait(false);

            _ = await instance
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return item;
        }
    }
}
