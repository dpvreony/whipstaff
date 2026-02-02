// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Whipstaff.EntityFramework.RowVersionSaving;

namespace Whipstaff.Testing.EntityFramework
{
    /// <summary>
    /// Abstraction for a DB Context Factory for testing purposes.
    /// </summary>
    /// <typeparam name="TDbContext">The <see cref="Microsoft.EntityFrameworkCore.DbContext" /> type to create.</typeparam>
    public abstract class AbstractDbContextFactory<TDbContext> : IDbContextFactory<TDbContext>
        where TDbContext : DbContext
    {
        private readonly DbConnection _dbConnection;
        private readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractDbContextFactory{TDbContext}"/> class.
        /// </summary>
        /// <param name="loggerFactory">Instance of the logger factory.</param>
#pragma warning disable GR0027 // Constructor should have a logging framework instance as the final parameter.
        protected AbstractDbContextFactory(ILoggerFactory loggerFactory)
            : this(CreateInMemoryDatabase(), loggerFactory)
        {
        }
#pragma warning restore GR0027 // Constructor should have a logging framework instance as the final parameter.

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractDbContextFactory{TDbContext}"/> class.
        /// </summary>
        /// <param name="dbConnection">Database connection instance.</param>
        /// <param name="loggerFactory">Instance of the logger factory.</param>
#pragma warning disable GR0027 // Constructor should have a logging framework instance as the final parameter.
        protected AbstractDbContextFactory(DbConnection dbConnection, ILoggerFactory loggerFactory)
        {
            ArgumentNullException.ThrowIfNull(dbConnection);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _dbConnection = dbConnection;
            _loggerFactory = loggerFactory;
        }
#pragma warning restore GR0027 // Constructor should have a logging framework instance as the final parameter.

        /// <inheritdoc/>
        public TDbContext CreateDbContext()
        {
            var dbContextOptions = new DbContextOptionsBuilder<TDbContext>()
                .UseSqlite(_dbConnection)
                .AddInterceptors(new RowVersionSaveChangesInterceptor())
                .UseLoggerFactory(_loggerFactory)
                .Options;

            var dbContext = GetDbContext(dbContextOptions);

#pragma warning disable GR0019 // Do not use Entity Framework Database EnsureCreated.
            _ = dbContext.Database.EnsureCreated();
#pragma warning restore GR0019 // Do not use Entity Framework Database EnsureCreated.

            return dbContext;
        }

        /// <summary>
        /// Gets the database context instance.
        /// </summary>
        /// <param name="dbContextOptions">DB Context options to use.</param>
        /// <returns>DB Context instance.</returns>
        protected abstract TDbContext GetDbContext(DbContextOptions<TDbContext> dbContextOptions);

        private static SqliteConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");

            connection.Open();

            return connection;
        }
    }
}
