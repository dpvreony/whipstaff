// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Whipstaff.Core.Entities;

namespace Whipstaff.EntityFramework
{
    /// <summary>
    /// Entity Framework Compiled Query Factories. Produces common queries based on common interfaces for DBSet.
    /// </summary>
    public static class CompiledQueryFactory
    {
        /// <summary>
        /// Gets a compiled async query for retrieving the maximum row version.
        /// </summary>
        /// <typeparam name="TDbContext">The type for the database context.</typeparam>
        /// <typeparam name="TDbSet">The type for the Database set.</typeparam>
        /// <param name="dbSetSelector">Function to select the DBSet used for the compiled query</param>
        /// <returns>Compiled EF Query.</returns>
        public static Func<TDbContext, CancellationToken, Task<Task<long>>> GetMaxRowVersionCompiledAsyncQuery<TDbContext, TDbSet>(System.Linq.Expressions.Expression<Func<TDbContext, DbSet<TDbSet>>> dbSetSelector)
            where TDbContext : DbContext
            where TDbSet : class, ILongRowVersion
        {
            return EF.CompileAsyncQuery((TDbContext context, CancellationToken cancellationToken) => dbSetSelector.Compile()(context)
                .MaxAsync(
                    entity => entity.RowVersion,
                    cancellationToken));
        }

        /// <summary>
        /// Gets a compiled query for retrieving the maximum row version.
        /// </summary>
        /// <typeparam name="TDbContext">The type for the database context.</typeparam>
        /// <typeparam name="TDbSet">The type for the Database set.</typeparam>
        /// <param name="dbSetSelector">Function to select the DBSet used for the compiled query</param>
        /// <returns>Compiled EF Query.</returns>
        public static Func<TDbContext, long> GetMaxRowVersionCompiledQuery<TDbContext, TDbSet>(System.Linq.Expressions.Expression<Func<TDbContext, DbSet<TDbSet>>> dbSetSelector)
            where TDbContext : DbContext
            where TDbSet : class, ILongRowVersion
        {
            return EF.CompileQuery((TDbContext context) => dbSetSelector.Compile()(context)
                .Max(entity => entity.RowVersion));
        }

        /// <summary>
        /// Gets a compiled async query for retrieving records where the row version is greater than a specified number.
        /// </summary>
        /// <typeparam name="TDbContext">The type for the database context.</typeparam>
        /// <typeparam name="TDbSet">The type for the Database set.</typeparam>
        /// <param name="dbSetSelector">Function to select the DBSet used for the compiled query</param>
        /// <returns>Compiled EF Query.</returns>
        public static Func<TDbContext, long, long, IAsyncEnumerable<TDbSet>> GetWhereRowVersionBetweenCompiledAsyncQuery<TDbContext, TDbSet>(System.Linq.Expressions.Expression<Func<TDbContext, DbSet<TDbSet>>> dbSetSelector)
            where TDbContext : DbContext
            where TDbSet : class, ILongRowVersion
        {
            return EF.CompileAsyncQuery((TDbContext context, long min, long max) => dbSetSelector.Compile()(context)
                .Where(entity => entity.RowVersion >= min && entity.RowVersion <= max));
        }

        /// <summary>
        /// Gets a compiled async query for retrieving records where the row version is greater than a specified number.
        /// </summary>
        /// <typeparam name="TDbContext">The type for the database context.</typeparam>
        /// <typeparam name="TDbSet">The type for the Database set.</typeparam>
        /// <param name="dbSetSelector">Function to select the DBSet used for the compiled query</param>
        /// <returns>Compiled EF Query.</returns>
        public static Func<TDbContext, long, IAsyncEnumerable<TDbSet>> GetWhereRowVersionGreaterThanCompiledAsyncQuery<TDbContext, TDbSet>(System.Linq.Expressions.Expression<Func<TDbContext, DbSet<TDbSet>>> dbSetSelector)
            where TDbContext : DbContext
            where TDbSet : class, ILongRowVersion
        {
            return EF.CompileAsyncQuery((TDbContext context, long rowVersion) => dbSetSelector.Compile()(context).Where(entity => entity.RowVersion > rowVersion));
        }

        /// <summary>
        /// Gets a compiled async query for retrieving a record with a int primary key.
        /// </summary>
        /// <typeparam name="TDbContext">The type for the database context.</typeparam>
        /// <typeparam name="TDbSet">The type for the Database set.</typeparam>
        /// <param name="dbSetSelector">Function to select the DBSet used for the compiled query</param>
        /// <returns>Compiled EF Query.</returns>
        public static Func<TDbContext, int, IAsyncEnumerable<TDbSet>> GetWhereUniqueIntIdEqualsCompiledAsyncQuery<TDbContext, TDbSet>(System.Linq.Expressions.Expression<Func<TDbContext, DbSet<TDbSet>>> dbSetSelector)
            where TDbContext : DbContext
            where TDbSet : class, IIntId
        {
            return EF.CompileAsyncQuery((TDbContext context, int id) => dbSetSelector.Compile()(context)
                .Where(entity => entity.Id == id));
        }

        /// <summary>
        /// Gets a compiled query for retrieving a record with a long primary key.
        /// </summary>
        /// <typeparam name="TDbContext">The type for the database context.</typeparam>
        /// <typeparam name="TDbSet">The type for the Database set.</typeparam>
        /// <param name="dbSetSelector">Function to select the DBSet used for the compiled query</param>
        /// <returns></returns>
        public static Func<TDbContext, int, IEnumerable<TDbSet>> GetWhereUniqueIntIdEqualsCompiledQuery<TDbContext, TDbSet>(System.Linq.Expressions.Expression<Func<TDbContext, DbSet<TDbSet>>> dbSetSelector)
            where TDbContext : DbContext
            where TDbSet : class, IIntId
        {
            return EF.CompileQuery((TDbContext context, int id) => dbSetSelector.Compile()(context)
                .Where(entity => entity.Id == id));
        }

        /// <summary>
        /// Gets a compiled async query for retrieving a record with a long primary key.
        /// </summary>
        /// <typeparam name="TDbContext">The type for the database context.</typeparam>
        /// <typeparam name="TDbSet">The type for the Database set.</typeparam>
        /// <param name="dbSetSelector">Function to select the DBSet used for the compiled query</param>
        /// <returns>Compiled EF Query.</returns>
        public static Func<TDbContext, long, IAsyncEnumerable<TDbSet>> GetWhereUniqueLongIdEqualsCompiledAsyncQuery<TDbContext, TDbSet>(System.Linq.Expressions.Expression<Func<TDbContext, DbSet<TDbSet>>> dbSetSelector)
            where TDbContext : DbContext
            where TDbSet : class, ILongId
        {
            return EF.CompileAsyncQuery((TDbContext context, long id) => dbSetSelector.Compile()(context)
                .Where(entity => entity.Id == id));
        }

        /// <summary>
        /// Gets a compiled query for retrieving a record with a long primary key.
        /// </summary>
        /// <typeparam name="TDbContext">The type for the database context.</typeparam>
        /// <typeparam name="TDbSet">The type for the Database set.</typeparam>
        /// <param name="dbSetSelector">Function to select the DBSet used for the compiled query</param>
        /// <returns></returns>
        public static Func<TDbContext, long, IEnumerable<TDbSet>> GetWhereUniqueLongIdEqualsCompiledQuery<TDbContext, TDbSet>(System.Linq.Expressions.Expression<Func<TDbContext, DbSet<TDbSet>>> dbSetSelector)
            where TDbContext : DbContext
            where TDbSet : class, ILongId
        {
            return EF.CompileQuery((TDbContext context, long id) => dbSetSelector.Compile()(context)
                .Where(entity => entity.Id == id));
        }
    }
}
