// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Whipstaff.EntityFramework.Relational;
using Xunit;

namespace Whipstaff.UnitTests.EntityFramework.Relational
{
    /// <summary>
    /// Unit Tests for SQL Lite DateTimeOffset ordering.
    /// </summary>
    public static class SqlLiteDateTimeOffsetOrdering
    {
        /// <summary>
        /// Represents a test entity.
        /// </summary>
        public sealed class TestEntity
        {
            /// <summary>
            /// Gets or sets the Id.
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// Gets or sets the date time with offset.
            /// </summary>
            public DateTimeOffset DateTimeOffset { get; set; }
        }

        /// <summary>
        /// Represents the db context common functionality for testing.
        /// </summary>
        public class BaseTestDbContext : DbContext
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="BaseTestDbContext"/> class.
            /// </summary>
            /// <param name="options">Database context options.</param>
            public BaseTestDbContext(DbContextOptions options)
                : base(options)
            {
            }

            /// <summary>
            /// Gets the test entity db set.
            /// </summary>
            public DbSet<TestEntity> TestEntity => Set<TestEntity>();
        }

        /// <summary>
        /// Represents the identity based db context common functionality for testing.
        /// </summary>
        public class BaseTestIdentityDbContext : IdentityDbContext
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="BaseTestIdentityDbContext"/> class.
            /// </summary>
            /// <param name="options">Database context options.</param>
            public BaseTestIdentityDbContext(DbContextOptions options)
                : base(options)
            {
            }

            /// <summary>
            /// Gets the test entity db set.
            /// </summary>
            public DbSet<TestEntity> TestEntity => Set<TestEntity>();
        }

        /// <summary>
        /// Represents a test db context that has the database model specific options injected into the constructor.
        /// </summary>
        public sealed class TestWithContextOptionsDbContext : BaseTestDbContext
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TestWithContextOptionsDbContext"/> class.
            /// </summary>
            /// <param name="options">Database context options.</param>
            public TestWithContextOptionsDbContext(DbContextOptions options)
                : base(options)
            {
            }
        }

        /// <summary>
        /// Represents a test db context that sets the database model up via the OnModelCreating override method.
        /// </summary>
        public sealed class TestWithOnModelCreatingDbContext : BaseTestDbContext
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TestWithOnModelCreatingDbContext"/> class.
            /// </summary>
            /// <param name="options">Database context options.</param>
#pragma warning disable GR0027 // Constructor should have a logging framework instance as the final parameter.
            public TestWithOnModelCreatingDbContext(DbContextOptions options)
                : base(options)
            {
            }
#pragma warning restore GR0027 // Constructor should have a logging framework instance as the final parameter.

            /// <inheritdoc/>
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                ModelBuilderHelpers.ConvertAllDateTimeOffSetPropertiesOnModelBuilderToLong(modelBuilder);
            }
        }

        /// <summary>
        /// Represents a test db context that has the database model specific options injected into the constructor.
        /// </summary>
        public sealed class TestWithContextOptionsIdentityDbContext : BaseTestDbContext
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TestWithContextOptionsIdentityDbContext"/> class.
            /// </summary>
            /// <param name="options">Database context options.</param>
            public TestWithContextOptionsIdentityDbContext(DbContextOptions options)
                : base(options)
            {
            }
        }

        /// <summary>
        /// Represents a test db context that sets the database model up via the OnModelCreating override method.
        /// </summary>
        public sealed class TestWithOnModelCreatingIdentityDbContext : BaseTestDbContext
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TestWithOnModelCreatingIdentityDbContext"/> class.
            /// </summary>
            /// <param name="options">Database context options.</param>
#pragma warning disable GR0027 // Constructor should have a logging framework instance as the final parameter.
            public TestWithOnModelCreatingIdentityDbContext(DbContextOptions options)
                : base(options)
            {
            }
#pragma warning restore GR0027 // Constructor should have a logging framework instance as the final parameter.

            /// <inheritdoc/>
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                ModelBuilderHelpers.ConvertAllDateTimeOffSetPropertiesOnModelBuilderToLong(modelBuilder);
            }
        }

        /// <summary>
        /// Unit Tests for the OrderBy method.
        /// </summary>
        public sealed class OrderByMethod
        {
            /// <summary>
            /// Test to ensure data is returned.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task IdentityDbContextReturnsDataAsync()
            {
                var dbContextOptionsBuilder = new DbContextOptionsBuilder();
                using (var connection = CreateInMemoryDatabase())
                {
                    _ = dbContextOptionsBuilder.UseSqlite(connection);

                    using (var dbContext = new TestWithOnModelCreatingIdentityDbContext(dbContextOptionsBuilder.Options))
                    {
#pragma warning disable GR0020 // Do not use Entity Framework Database EnsureCreatedAsync.
                        _ = await dbContext.Database.EnsureCreatedAsync(TestContext.Current.CancellationToken);
#pragma warning restore GR0020 // Do not use Entity Framework Database EnsureCreatedAsync.

                        _ = await dbContext.TestEntity.AddAsync(new TestEntity { DateTimeOffset = DateTimeOffset.Now }, TestContext.Current.CancellationToken);

                        _ = await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

                        var result = await dbContext.TestEntity
                            .Where(GetSelector())
                            .ToArrayAsync(cancellationToken: TestContext.Current.CancellationToken);

                        Assert.NotNull(result);
                        Assert.NotEmpty(result);
                    }
                }
            }

            /// <summary>
            /// Test to ensure data is returned.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task IdentityReturnsData2Async()
            {
                var dbContextOptionsBuilder = new DbContextOptionsBuilder();

                using (var connection = CreateInMemoryDatabase())
                {
                    _ = dbContextOptionsBuilder.UseSqlite(connection);

                    var modelBuilder = SqliteConventionSetBuilder.CreateModelBuilder();
                    _ = modelBuilder.Entity<TestEntity>();

                    ModelBuilderHelpers.ConvertAllDateTimeOffSetPropertiesOnModelBuilderToLong(modelBuilder);

                    var model = modelBuilder.FinalizeModel();

                    dbContextOptionsBuilder = dbContextOptionsBuilder.UseModel(model);

                    using (var dbContext = new TestWithContextOptionsIdentityDbContext(dbContextOptionsBuilder.Options))
                    {
#pragma warning disable GR0020 // Do not use Entity Framework Database EnsureCreatedAsync.
                        _ = await dbContext.Database.EnsureCreatedAsync(TestContext.Current.CancellationToken);
#pragma warning restore GR0020 // Do not use Entity Framework Database EnsureCreatedAsync.

                        _ = await dbContext.TestEntity.AddAsync(new TestEntity { DateTimeOffset = DateTimeOffset.Now }, TestContext.Current.CancellationToken);

                        _ = await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

                        var result = await dbContext.TestEntity
                            .Where(GetSelector())
                            .ToArrayAsync(cancellationToken: TestContext.Current.CancellationToken);

                        Assert.NotNull(result);
                        Assert.NotEmpty(result);
                    }
                }
            }

            /// <summary>
            /// Test to ensure data is returned.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task NormalDbContextReturnsDataAsync()
            {
                var dbContextOptionsBuilder = new DbContextOptionsBuilder();
                using (var connection = CreateInMemoryDatabase())
                {
                    _ = dbContextOptionsBuilder.UseSqlite(connection);

                    using (var dbContext = new TestWithOnModelCreatingDbContext(dbContextOptionsBuilder.Options))
                    {
#pragma warning disable GR0020 // Do not use Entity Framework Database EnsureCreatedAsync.
                        _ = await dbContext.Database.EnsureCreatedAsync(TestContext.Current.CancellationToken);
#pragma warning restore GR0020 // Do not use Entity Framework Database EnsureCreatedAsync.

                        _ = await dbContext.TestEntity.AddAsync(new TestEntity { DateTimeOffset = DateTimeOffset.Now }, TestContext.Current.CancellationToken);

                        _ = await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

                        var result = await dbContext.TestEntity
                            .Where(GetSelector())
                            .ToArrayAsync(cancellationToken: TestContext.Current.CancellationToken);

                        Assert.NotNull(result);
                        Assert.NotEmpty(result);
                    }
                }
            }

            /// <summary>
            /// Test to ensure data is returned.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task NormalReturnsData2Async()
            {
                var dbContextOptionsBuilder = new DbContextOptionsBuilder();

                using (var connection = CreateInMemoryDatabase())
                {
                    _ = dbContextOptionsBuilder.UseSqlite(connection);

                    var modelBuilder = SqliteConventionSetBuilder.CreateModelBuilder();
                    _ = modelBuilder.Entity<TestEntity>();

                    ModelBuilderHelpers.ConvertAllDateTimeOffSetPropertiesOnModelBuilderToLong(modelBuilder);

                    var model = modelBuilder.FinalizeModel();

                    dbContextOptionsBuilder = dbContextOptionsBuilder.UseModel(model);

                    using (var dbContext = new TestWithContextOptionsDbContext(dbContextOptionsBuilder.Options))
                    {
#pragma warning disable GR0020 // Do not use Entity Framework Database EnsureCreatedAsync.
                        _ = await dbContext.Database.EnsureCreatedAsync(TestContext.Current.CancellationToken);
#pragma warning restore GR0020 // Do not use Entity Framework Database EnsureCreatedAsync.

                        _ = await dbContext.TestEntity.AddAsync(new TestEntity { DateTimeOffset = DateTimeOffset.Now }, TestContext.Current.CancellationToken);

                        _ = await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

                        var result = await dbContext.TestEntity
                            .Where(GetSelector())
                            .ToArrayAsync(cancellationToken: TestContext.Current.CancellationToken);

                        Assert.NotNull(result);
                        Assert.NotEmpty(result);
                    }
                }
            }

            private static SqliteConnection CreateInMemoryDatabase()
            {
                var connection = new SqliteConnection("Filename=:memory:");

                connection.Open();

                return connection;
            }

            private static Expression<Func<TestEntity, bool>> GetSelector()
            {
                return entity => entity.Id > 0;
            }
        }
    }
}
