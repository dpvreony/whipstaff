// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Whipstaff.EntityFramework.Extensions;
using Whipstaff.Testing.EntityFramework;
using Xunit;

namespace Whipstaff.UnitTests.EntityFramework.Extensions
{
    /// <summary>
    /// Unit tests for the <see cref="Whipstaff.EntityFramework.Extensions.ServiceCollectionExtensions"/> class.
    /// </summary>
    public static class ServiceCollectionExtensionsTests
    {
        /// <summary>
        /// Unit tests for the <see cref="Whipstaff.EntityFramework.Extensions.ServiceCollectionExtensions.AddDbContextWithModelCreator{TDbContext, TModelCreator}(IServiceCollection, Action{IServiceProvider, DbContextOptionsBuilder}?, ServiceLifetime, ServiceLifetime)"/> method.
        /// </summary>
        public sealed class AddDbContextWithModelCreatorMethod
        {
            /// <summary>
            /// Tests that the method returns a modified service collection.
            /// </summary>
            [Fact]
            public void ReturnsModifiedServiceCollection()
            {
                // Arrange
                IServiceCollection serviceCollection = new ServiceCollection();
                var optionsAction = OptionsAction;

                serviceCollection = serviceCollection.AddSingleton<SqliteConnection>(_ => CreateInMemoryDatabase());

                // Act
                var result = serviceCollection.AddDbContextWithModelCreator<FakeDbContext, SqliteFakeDbContextModelCreator<FakeDbContext>>(optionsAction);

                // Assert
                Assert.NotNull(result);

                var dbContext = serviceCollection.BuildServiceProvider().GetRequiredService<FakeDbContext>();
                Assert.NotNull(dbContext);
            }

            private static void OptionsAction(IServiceProvider serviceProvider, DbContextOptionsBuilder options)
            {
                var dbConnection = serviceProvider.GetRequiredService<SqliteConnection>();
                _ = options.UseSqlite(dbConnection);
            }

            private static SqliteConnection CreateInMemoryDatabase()
            {
                var connection = new SqliteConnection("Filename=:memory:");

                connection.Open();

                return connection;
            }
        }
    }
}
