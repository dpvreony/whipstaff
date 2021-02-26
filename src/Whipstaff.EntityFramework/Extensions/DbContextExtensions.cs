// Copyright (c) 2020 DHGMS Solutions and Contributors. All rights reserved.
// DHGMS Solutions and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Whipstaff.Core.Entities;

namespace Whipstaff.EntityFramework.Extensions
{
    /// <summary>
    /// Extensions for Entity Framework DBContext.
    /// </summary>
    public static class DbContextExtensions
    {
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
            var d = dbSetSelectorFunc(instance);
            var item = d.FirstOrDefault(predicate);

            if (item != null)
            {
                return item;
            }

            item = addEntityFactoryFunc();
            await instance.SaveChangesAsync()
                .ConfigureAwait(false);

            return item;
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
            return await GetOrAddAsync(
                instance,
                dbSetSelectorFunc,
                x => x.Id == id,
                addEntityFactoryFunc).ConfigureAwait(false);
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
            return await GetOrAddAsync(
                instance,
                dbSetSelectorFunc,
                x => x.Id == id,
                addEntityFactoryFunc).ConfigureAwait(false);
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
            return await GetOrAddAsync(
                instance,
                dbSetSelectorFunc,
                x => x.Id == id,
                addEntityFactoryFunc).ConfigureAwait(false);
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
        public static async Task<TEntity> GetOrAddByName<TDbContext, TEntity>(
            this TDbContext instance,
            Func<TDbContext, DbSet<TEntity>> dbSetSelectorFunc,
            string name,
            Func<TEntity> addEntityFactoryFunc)
            where TDbContext : DbContext
            where TEntity : class, INameable
        {
            return await GetOrAddAsync(
                instance,
                dbSetSelectorFunc,
                x => x.Name == name,
                addEntityFactoryFunc).ConfigureAwait(false);
        }
    }
}
