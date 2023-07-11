// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
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
    /// Extension methods for EF Core DBSets.
    /// </summary>
    public static class DbSetExtensions
    {
        /// <summary>
        /// Gets the maximum Id value from the DbSet or null if there are no rows.
        /// </summary>
        /// <typeparam name="TEntity">Type for the EF Core DBSet Entity.</typeparam>
        /// <param name="dbSet">EFCore DBSet Instance.</param>
        /// <returns>Maximum id or null if there are no rows.</returns>
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
        /// <returns>Maximum id or null if there are no rows.</returns>
        public static Task<int?> GetMaxIntIdOrDefaultAsync<TEntity>(this DbSet<TEntity> dbSet)
            where TEntity : class, IIntId
        {
            return dbSet.MaxAsync(x => (int?)x.Id);
        }

        /// <summary>
        /// Gets the maximum Id value from the DbSet or null if there are no rows.
        /// </summary>
        /// <typeparam name="TEntity">Type for the EF Core DBSet Entity.</typeparam>
        /// <param name="dbSet">EFCore DBSet Instance.</param>
        /// <returns>Maximum id or null if there are no rows.</returns>
        public static long? GetMaxLongIdOrDefault<TEntity>(this DbSet<TEntity> dbSet)
            where TEntity : class, ILongId
        {
            return dbSet.Max(x => (long?)x.Id);
        }

        /// <summary>
        /// Gets the maximum Id value from the DbSet or null if there are no rows.
        /// </summary>
        /// <typeparam name="TEntity">Type for the EF Core DBSet Entity.</typeparam>
        /// <param name="dbSet">EFCore DBSet Instance.</param>
        /// <returns>Maximum id or null if there are no rows.</returns>
        public static Task<long?> GetMaxLongIdOrDefaultAsync<TEntity>(this DbSet<TEntity> dbSet)
            where TEntity : class, ILongId
        {
            return dbSet.MaxAsync(x => (long?)x.Id);
        }

        /// <summary>
        /// Gets the maximum row version from the DbSet or null if there are no rows.
        /// </summary>
        /// <typeparam name="TEntity">Type for the EF Core DBSet Entity.</typeparam>
        /// <param name="dbSet">EFCore DBSet Instance.</param>
        /// <returns>Maximum row version or null.</returns>
        public static long? GetMaxRowVersionOrDefault<TEntity>(this DbSet<TEntity> dbSet)
            where TEntity : class, ILongRowVersion
        {
            return dbSet.Max(x => (long?)x.RowVersion);
        }

        /// <summary>
        /// Gets the maximum row version from the DbSet or null if there are no rows.
        /// </summary>
        /// <typeparam name="TEntity">Type for the EF Core DBSet Entity.</typeparam>
        /// <param name="dbSet">EFCore DBSet Instance.</param>
        /// <returns>Maximum row version or null.</returns>
        public static Task<long?> GetMaxRowVersionOrDefaultAsync<TEntity>(this DbSet<TEntity> dbSet)
            where TEntity : class, ILongRowVersion
        {
            return dbSet.MaxAsync(x => (long?)x.RowVersion);
        }

        /// <summary>
        /// Gets rows where the Id is greater than the previous max Id.
        /// </summary>
        /// <typeparam name="TEntity">Type for the EF Core DBSet Entity.</typeparam>
        /// <param name="dbSet">EFCore DBSet Instance.</param>
        /// <param name="id">The unique id records must be greater than.</param>
        /// <returns>Queryable representing the rows to return.</returns>
        public static IQueryable<TEntity> GetRowsGreaterThanIntId<TEntity>(
            this DbSet<TEntity> dbSet,
            int id)
            where TEntity : class, IIntId
        {
            return dbSet.Where(x => x.Id > id);
        }

        /// <summary>
        /// Gets rows where the Id is greater than the previous max Id.
        /// </summary>
        /// <typeparam name="TEntity">Type for the EF Core DBSet Entity.</typeparam>
        /// <param name="dbSet">EFCore DBSet Instance.</param>
        /// <param name="id">The unique id records must be greater than.</param>
        /// <param name="takeRecords">Maximum number of records to take.</param>
        /// <returns>Queryable representing the rows to return.</returns>
        public static IQueryable<TEntity> GetRowsGreaterThanIntId<TEntity>(
            this DbSet<TEntity> dbSet,
            int id,
            int takeRecords)
            where TEntity : class, IIntId
        {
            return dbSet.Where(x => x.Id > id)
                .Take(takeRecords);
        }

        /// <summary>
        /// Gets rows where the Id is greater than the previous max Id.
        /// </summary>
        /// <typeparam name="TEntity">Type for the EF Core DBSet Entity.</typeparam>
        /// <typeparam name="TResult">Type for the output result selector.</typeparam>
        /// <param name="dbSet">EFCore DBSet Instance.</param>
        /// <param name="id">The unique id records must be greater than.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <returns>Queryable representing the rows to return.</returns>
        public static IQueryable<TResult> GetRowsGreaterThanIntId<TEntity, TResult>(
            this DbSet<TEntity> dbSet,
            int id,
            Expression<Func<TEntity, TResult>> selector)
            where TEntity : class, IIntId
        {
            return dbSet.Where(x => x.Id > id)
                .Select(selector);
        }

        /// <summary>
        /// Gets rows where the Id is greater than the previous max Id.
        /// </summary>
        /// <typeparam name="TEntity">Type for the EF Core DBSet Entity.</typeparam>
        /// <typeparam name="TResult">Type for the output result selector.</typeparam>
        /// <param name="dbSet">EFCore DBSet Instance.</param>
        /// <param name="id">The unique id records must be greater than.</param>
        /// <param name="takeRecords">Maximum number of records to take.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <returns>Queryable representing the rows to return.</returns>
        public static IQueryable<TResult> GetRowsGreaterThanIntId<TEntity, TResult>(
            this DbSet<TEntity> dbSet,
            int id,
            int takeRecords,
            Expression<Func<TEntity, TResult>> selector)
            where TEntity : class, IIntId
        {
            return dbSet.Where(x => x.Id > id)
                .Take(takeRecords)
                .Select(selector);
        }

        /// <summary>
        /// Gets rows where the Id is greater than the previous max Id.
        /// </summary>
        /// <typeparam name="TEntity">Type for the EF Core DBSet Entity.</typeparam>
        /// <param name="dbSet">EFCore DBSet Instance.</param>
        /// <param name="id">The unique id records must be greater than.</param>
        /// <returns>Queryable representing the rows to return.</returns>
        public static IQueryable<TEntity> GetRowsGreaterThanLongId<TEntity>(
            this DbSet<TEntity> dbSet,
            int id)
            where TEntity : class, ILongId
        {
            return dbSet.Where(x => x.Id > id);
        }

        /// <summary>
        /// Gets rows where the Id is greater than the previous max Id.
        /// </summary>
        /// <typeparam name="TEntity">Type for the EF Core DBSet Entity.</typeparam>
        /// <typeparam name="TResult">Type for the output result selector.</typeparam>
        /// <param name="dbSet">EFCore DBSet Instance.</param>
        /// <param name="id">The unique id records must be greater than.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <returns>Queryable representing the rows to return.</returns>
        public static IQueryable<TResult> GetRowsGreaterThanLongId<TEntity, TResult>(
            this DbSet<TEntity> dbSet,
            int id,
            Expression<Func<TEntity, TResult>> selector)
            where TEntity : class, ILongId
        {
            return dbSet.Where(x => x.Id > id)
                .Select(selector);
        }

        /// <summary>
        /// Get rows where the Id is greater than the previous max Id.
        /// </summary>
        /// <typeparam name="TEntity">Type for the EF Core DBSet Entity.</typeparam>
        /// <param name="dbSet">EFCore DBSet Instance.</param>
        /// <param name="id">The unique id records must be greater than.</param>
        /// <param name="takeRecords">Maximum number of records to take.</param>
        /// <returns>Queryable representing the rows to return.</returns>
        public static IQueryable<TEntity> GetRowsGreaterThanLongId<TEntity>(
            this DbSet<TEntity> dbSet,
            int id,
            int takeRecords)
            where TEntity : class, ILongId
        {
            return dbSet.Where(x => x.Id > id)
                .Take(takeRecords);
        }

        /// <summary>
        /// Gets rows where the Id is greater than the previous max Id.
        /// </summary>
        /// <typeparam name="TEntity">Type for the EF Core DBSet Entity.</typeparam>
        /// <typeparam name="TResult">Type for the output result selector.</typeparam>
        /// <param name="dbSet">EFCore DBSet Instance.</param>
        /// <param name="id">The unique id records must be greater than.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <returns>Queryable representing the rows to return.</returns>
        public static IQueryable<TResult> GetRowsGreaterThanLongId<TEntity, TResult>(
            this DbSet<TEntity> dbSet,
            long id,
            Expression<Func<TEntity, TResult>> selector)
            where TEntity : class, ILongId
        {
            return dbSet.Where(x => x.Id > id)
                .Select(selector);
        }

        /// <summary>
        /// Gets rows where the Id is greater than the previous max Id.
        /// </summary>
        /// <typeparam name="TEntity">Type for the EF Core DBSet Entity.</typeparam>
        /// <typeparam name="TResult">Type for the output result selector.</typeparam>
        /// <param name="dbSet">EFCore DBSet Instance.</param>
        /// <param name="id">The unique id records must be greater than.</param>
        /// <param name="takeRecords">Maximum number of records to take.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <returns>Queryable representing the rows to return.</returns>
        public static IQueryable<TResult> GetRowsGreaterThanLongId<TEntity, TResult>(
            this DbSet<TEntity> dbSet,
            long id,
            int takeRecords,
            Expression<Func<TEntity, TResult>> selector)
            where TEntity : class, ILongId
        {
            return dbSet.Where(x => x.Id > id)
                .Take(takeRecords)
                .Select(selector);
        }

        /// <summary>
        /// Get rows where the row version is greater than the previous max row version.
        /// </summary>
        /// <typeparam name="TEntity">Type for the EF Core DBSet Entity.</typeparam>
        /// <param name="dbSet">EFCore DBSet Instance.</param>
        /// <param name="id">The unique id records must be greater than.</param>
        /// <returns>Queryable representing the rows to return.</returns>
        public static IQueryable<TEntity> GetRowsGreaterThanRowVersion<TEntity>(
            this DbSet<TEntity> dbSet,
            long id)
            where TEntity : class, ILongRowVersion
        {
            return dbSet.Where(x => x.RowVersion > id);
        }

        /// <summary>
        /// Get rows where the row version is greater than the previous max row version.
        /// </summary>
        /// <typeparam name="TEntity">Type for the EF Core DBSet Entity.</typeparam>
        /// <param name="dbSet">EFCore DBSet Instance.</param>
        /// <param name="rowVersion">The rowVersion records must be greater than.</param>
        /// <param name="takeRecords">Maximum number of records to take.</param>
        /// <returns>Queryable representing the rows to return.</returns>
        public static IQueryable<TEntity> GetRowsGreaterThanRowVersion<TEntity>(
            this DbSet<TEntity> dbSet,
            long rowVersion,
            int takeRecords)
            where TEntity : class, ILongRowVersion
        {
            return dbSet.Where(x => x.RowVersion > rowVersion)
                .Take(takeRecords);
        }

        /// <summary>
        /// Gets the rows where the row version is greater than the previous max row version and less than or equal to the current max row version.
        /// This is to facilitate getting rows that have been updated since the last sync of a process.
        /// </summary>
        /// <typeparam name="TEntity">Type for the EF Core DBSet Entity.</typeparam>
        /// <param name="dbSet">EFCore DBSet Instance.</param>
        /// <param name="greaterThanRowVersion">The rowVersion records must be greater than.</param>
        /// <param name="maxRowVersion">The row version that records can be up to.</param>
        /// <param name="takeRecords">Maximum number of records to take.</param>
        /// <returns>Queryable representing the rows to return.</returns>
        public static IQueryable<TEntity> GetRowsGreaterThanAndLessThanOrEqualToRowVersions<TEntity>(
            this DbSet<TEntity> dbSet,
            long greaterThanRowVersion,
            long maxRowVersion,
            int takeRecords)
            where TEntity : class, ILongRowVersion
        {
            return dbSet.Where(x => x.RowVersion > greaterThanRowVersion && x.RowVersion <= maxRowVersion)
                .Take(takeRecords);
        }
    }
}
