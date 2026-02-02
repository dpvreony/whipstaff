// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Whipstaff.EntityFramework.RowVersionSaving;

namespace Whipstaff.Testing.EntityFramework
{
    /// <summary>
    /// Design Time DB Context Factory for the Fake DB Context.
    /// </summary>
    public sealed class FakeDesignTimeDbContextFactory : IDesignTimeDbContextFactory<FakeDbContext>
    {
        private readonly DbConnection _dbConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeDesignTimeDbContextFactory"/> class.
        /// </summary>
        public FakeDesignTimeDbContextFactory()
        {
            _dbConnection = CreateInMemoryDatabase();
        }

        /// <inheritdoc/>
        public FakeDbContext CreateDbContext(string[] args)
        {
            var dbContextOptions = new DbContextOptionsBuilder<FakeDbContext>()
                .UseSqlite(_dbConnection)
                .AddInterceptors(new RowVersionSaveChangesInterceptor())
                .Options;

            var dbContext = new FakeDbContext(
                dbContextOptions,
                () => new SqliteFakeDbContextModelCreator<FakeDbContext>());

#pragma warning disable GR0019 // Do not use Entity Framework Database EnsureCreated.
            _ = dbContext.Database.EnsureCreated();
#pragma warning restore GR0019 // Do not use Entity Framework Database EnsureCreated.

            return dbContext;
        }

        private static SqliteConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");

            connection.Open();

            return connection;
        }
    }
}
