// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Linq;
using Microsoft.EntityFrameworkCore;
using Whipstaff.Core.Entities;

namespace Whipstaff.EntityFramework.Extensions
{
    /// <summary>
    /// Extension methods for EF Core DBSets.
    /// </summary>
    public static class DbSetExtensions
    {
        /// <summary>
        /// Gets the maximum Id value from the DbSet or null if there are no rows.
        /// </summary>
        /// <typeparam name="TEntity">Type for the EF Core DBSet Entity.</typeparam>
        /// <param name="dbSet">EFCore DBSet Instance.</param>
        /// <returns></returns>
        public static int? GetMaxIntIdOrDefault<TEntity>(this DbSet<TEntity> dbSet)
            where TEntity : class, IIntId
        {
            return dbSet.Max(x => (int?)x.Id);
        }

        /// <summary>
        /// Gets the maximum Id value from the DbSet or null if there are no rows.
        /// </summary>
        /// <typeparam name="TEntity">Type for the EF Core DBSet Entity.</typeparam>
        /// <param name="dbSet">EFCore DBSet Instance.</param>
        /// <returns></returns>
        public static long? GetMaxLongIdOrDefault<TEntity>(this DbSet<TEntity> dbSet)
            where TEntity : class, ILongId
        {
            return dbSet.Max(x => (long?)x.Id);
        }

        /// <summary>
        /// Gets the maximum row version from the DbSet or null if there are no rows.
        /// </summary>
        /// <typeparam name="TEntity">Type for the EF Core DBSet Entity.</typeparam>
        /// <param name="dbSet">EFCore DBSet Instance.</param>
        /// <returns></returns>
        public static long? GetMaxRowVersionOrDefault<TEntity>(this DbSet<TEntity> dbSet)
            where TEntity : class, ILongRowVersion
        {
            return dbSet.Max(x => (long?)x.RowVersion);
        }

        public static IQueryable<TEntity> GetRowsGreaterThanIntId<TEntity>(
            this DbSet<TEntity> dbSet,
            int previousMaxId)
            where TEntity : class, IIntId
        {
            return dbSet.Where(x => x.Id > previousMaxId);
        }

        public static IQueryable<TEntity> GetRowsGreaterThanIntId<TEntity>(
            this DbSet<TEntity> dbSet,
            int previousMaxId,
            int takeRecords)
            where TEntity : class, IIntId
        {
            return dbSet.Where(x => x.Id > previousMaxId)
                .Take(takeRecords);
        }

        public static IQueryable<TEntity> GetRowsGreaterThanLongId<TEntity>(
            this DbSet<TEntity> dbSet,
            int previousMaxId)
            where TEntity : class, ILongId
        {
            return dbSet.Where(x => x.Id > previousMaxId);
        }

        public static IQueryable<TEntity> GetRowsGreaterThanLongId<TEntity>(
            this DbSet<TEntity> dbSet,
            int previousMaxId,
            int takeRecords)
            where TEntity : class, ILongId
        {
            return dbSet.Where(x => x.Id > previousMaxId)
                .Take(takeRecords);
        }

        public static IQueryable<TEntity> GetRowsGreaterThanRowVersion<TEntity>(
            this DbSet<TEntity> dbSet,
            long previousMaxRowVersion)
            where TEntity : class, ILongRowVersion
        {
            return dbSet.Where(x => x.RowVersion > previousMaxRowVersion);
        }

        public static IQueryable<TEntity> GetRowsGreaterThanRowVersion<TEntity>(
            this DbSet<TEntity> dbSet,
            long previousMaxRowVersion,
            int takeRecords)
            where TEntity : class, ILongRowVersion
        {
            return dbSet.Where(x => x.RowVersion > previousMaxRowVersion)
                .Take(takeRecords);
        }
    }
}
