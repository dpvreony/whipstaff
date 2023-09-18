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
    /// DB Context Factory for the Fake DB Context.
    /// </summary>
    public sealed class FakeDbContextFactory : IDbContextFactory<FakeDbContext>
    {
        private readonly DbConnection _dbConnection;
        private readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeDbContextFactory"/> class.
        /// </summary>
        /// <param name="loggerFactory">Instance of the logger factory.</param>
        public FakeDbContextFactory(ILoggerFactory loggerFactory)
            : this(CreateInMemoryDatabase(), loggerFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeDbContextFactory"/> class.
        /// </summary>
        /// <param name="dbConnection">Database connection instance.</param>
        /// <param name="loggerFactory">Instance of the logger factory.</param>
        public FakeDbContextFactory(DbConnection dbConnection, ILoggerFactory loggerFactory)
        {
            ArgumentNullException.ThrowIfNull(dbConnection);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _dbConnection = dbConnection;
            _loggerFactory = loggerFactory;
        }

        /// <inheritdoc/>
        public FakeDbContext CreateDbContext()
        {
            var dbContextOptions = new DbContextOptionsBuilder<FakeDbContext>()
                .UseSqlite(_dbConnection)
                .AddInterceptors(new RowVersionSaveChangesInterceptor())
                .UseLoggerFactory(_loggerFactory)
                .Options;

            var dbContext = new FakeDbContext(
                dbContextOptions,
                () => new SqliteFakeDbContextModelCreator());

            _ = dbContext.Database.EnsureCreated();

            return dbContext;
        }

        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");

            connection.Open();

            return connection;
        }
    }
}
