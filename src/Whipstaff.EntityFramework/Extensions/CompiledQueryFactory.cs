using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Whipstaff.Core.Entities;

namespace Whipstaff.EntityFramework.Extensions
{
    /// <summary>
    /// Entity Framework Compiled Query Factories. Produces common queries based on common interfaces for DBSet.
    /// </summary>
    public static class CompiledQueryFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <typeparam name="TDbSet"></typeparam>
        /// <param name="dbSetSelector"></param>
        /// <returns></returns>
        public static Func<TDbContext, int, IAsyncEnumerable<TDbSet>> GetWhereUniqueIntIdEqualsCompiledAsyncQuery<TDbContext, TDbSet>(System.Linq.Expressions.Expression<Func<TDbContext, DbSet<TDbSet>>> dbSetSelector)
            where TDbContext : DbContext
            where TDbSet : class, IIntId
        {
            return EF.CompileAsyncQuery((TDbContext context, int id) => dbSetSelector.Compile()(context).Where(entity => entity.Id == id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <typeparam name="TDbSet"></typeparam>
        /// <param name="dbSetSelector"></param>
        /// <returns></returns>
        public static Func<TDbContext, int, IEnumerable<TDbSet>> GetWhereUniqueIntIdEqualsCompiledQuery<TDbContext, TDbSet>(System.Linq.Expressions.Expression<Func<TDbContext, DbSet<TDbSet>>> dbSetSelector)
            where TDbContext : DbContext
            where TDbSet : class, IIntId
        {
            return EF.CompileQuery((TDbContext context, int id) => dbSetSelector.Compile()(context).Where(entity => entity.Id == id));
        }

        /// <summary>
        /// Gets a compiled query for retrieving a record with a long primary key.
        /// </summary>
        /// <typeparam name="TDbContext">The type for the database context.</typeparam>
        /// <typeparam name="TDbSet">The type for the Database set.</typeparam>
        /// <param name="dbSetSelector">Function to select the DBSet used for the compiled query</param>
        /// <returns></returns>
        public static Func<TDbContext, long, IAsyncEnumerable<TDbSet>> GetWhereUniqueLongIdEqualsCompiledAsyncQuery<TDbContext, TDbSet>(System.Linq.Expressions.Expression<Func<TDbContext, DbSet<TDbSet>>> dbSetSelector)
            where TDbContext : DbContext
            where TDbSet : class, ILongId
        {
            return EF.CompileAsyncQuery((TDbContext context, long id) => dbSetSelector.Compile()(context).Where(entity => entity.Id == id));
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
            return EF.CompileQuery((TDbContext context, long id) => dbSetSelector.Compile()(context).Where(entity => entity.Id == id));
        }    }
}
