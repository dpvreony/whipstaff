// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Whipstaff.EntityFramework.Extensions;
using Whipstaff.Testing.EntityFramework;
using Whipstaff.Testing.EntityFramework.DbSets;
using Xunit;
using Xunit.Abstractions;

namespace Whipstaff.UnitTests.EntityFramework.Extensions
{
    /// <summary>
    /// Unit Tests for <see cref="DbSetExtensions"/>.
    /// </summary>
    public static class DbSetExtensionsTests
    {
        /// <summary>
        /// Unit Tests for <see cref="DbSetExtensions.GetMaxIntIdOrDefault{TEntity}"/>.
        /// </summary>
        public sealed class GetMaxIntIdOrDefaultMethod : Foundatio.Xunit.TestWithLoggingBase
        {
            private readonly IDbContextFactory<FakeDbContext> _dbContextFactory;

            /// <summary>
            /// Initializes a new instance of the <see cref="GetMaxIntIdOrDefaultMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit logging output helper.</param>
            public GetMaxIntIdOrDefaultMethod(ITestOutputHelper output)
                : base(output)
            {
                _dbContextFactory = new FakeDbContextFactory(Log);
            }

            /// <summary>
            /// Test to ensure that the method returns the expected value.
            /// </summary>
            [Fact]
            public void ReturnsValue()
            {
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 1 });
                    _ = dbContext.SaveChanges();

                    var result = dbContext.FakeAddAudit.GetMaxIntIdOrDefault();
                    var validatedResult = Assert.NotNull(result);
                    Assert.True(validatedResult > 0);
                }
            }

            /// <summary>
            /// Test to ensure that the method returns null.
            /// </summary>
            [Fact]
            public void ReturnsNull()
            {
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    var result = dbContext.FakeAddAudit.GetMaxIntIdOrDefault();
                    Assert.Null(result);
                }
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="DbSetExtensions.GetMaxIntIdOrDefaultAsync{TEntity}"/>.
        /// </summary>
        public sealed class GetMaxIntIdOrDefaultAsyncMethod : Foundatio.Xunit.TestWithLoggingBase
        {
            private readonly IDbContextFactory<FakeDbContext> _dbContextFactory;

            /// <summary>
            /// Initializes a new instance of the <see cref="GetMaxIntIdOrDefaultAsyncMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit logging output helper.</param>
            public GetMaxIntIdOrDefaultAsyncMethod(ITestOutputHelper output)
                : base(output)
            {
                _dbContextFactory = new FakeDbContextFactory(Log);
            }

            /// <summary>
            /// Test to ensure that the method returns the expected value.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsValueAsync()
            {
                using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
                {
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 1 });
                    _ = await dbContext.SaveChangesAsync()
                        .ConfigureAwait(false);

                    var result = await dbContext.FakeAddAudit.GetMaxIntIdOrDefaultAsync()
                        .ConfigureAwait(false);

                    var validatedResult = Assert.NotNull(result);
                    Assert.True(validatedResult > 0);
                }
            }

            /// <summary>
            /// Test to ensure that the method returns null.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsNullAsync()
            {
                using (var dbContext = await _dbContextFactory.CreateDbContextAsync()
                           .ConfigureAwait(false))
                {
                    var result = await dbContext.FakeAddAudit.GetMaxIntIdOrDefaultAsync()
                        .ConfigureAwait(false);

                    Assert.Null(result);
                }
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="DbSetExtensions.GetMaxLongIdOrDefault{TEntity}"/>.
        /// </summary>
        public sealed class GetMaxLongIdOrDefaultMethod : Foundatio.Xunit.TestWithLoggingBase
        {
            private readonly IDbContextFactory<FakeDbContext> _dbContextFactory;

            /// <summary>
            /// Initializes a new instance of the <see cref="GetMaxLongIdOrDefaultMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit logging output helper.</param>
            public GetMaxLongIdOrDefaultMethod(ITestOutputHelper output)
                : base(output)
            {
                _dbContextFactory = new FakeDbContextFactory(Log);
            }

            /// <summary>
            /// Test to ensure that the method returns the expected value.
            /// </summary>
            [Fact]
            public void ReturnsValue()
            {
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    _ = dbContext.FakeLongIdTable.Add(new FakeLongIdTableDbSet());
                    _ = dbContext.SaveChanges();

                    var result = dbContext.FakeLongIdTable.GetMaxLongIdOrDefault();
                    var validatedResult = Assert.NotNull(result);
                    Assert.True(validatedResult > 0);
                }
            }

            /// <summary>
            /// Test to ensure that the method returns null.
            /// </summary>
            [Fact]
            public void ReturnsNull()
            {
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    var result = dbContext.FakeLongIdTable.GetMaxLongIdOrDefault();

                    Assert.Null(result);
                }
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="DbSetExtensions.GetMaxLongIdOrDefaultAsync{TEntity}"/>.
        /// </summary>
        public sealed class GetMaxLongIdOrDefaultAsyncMethod : Foundatio.Xunit.TestWithLoggingBase
        {
            private readonly IDbContextFactory<FakeDbContext> _dbContextFactory;

            /// <summary>
            /// Initializes a new instance of the <see cref="GetMaxLongIdOrDefaultAsyncMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit logging output helper.</param>
            public GetMaxLongIdOrDefaultAsyncMethod(ITestOutputHelper output)
                : base(output)
            {
                _dbContextFactory = new FakeDbContextFactory(Log);
            }

            /// <summary>
            /// Test to ensure that the method returns the expected value.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsValueAsync()
            {
                using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
                {
                    _ = dbContext.FakeLongIdTable.Add(new FakeLongIdTableDbSet());
                    _ = await dbContext.SaveChangesAsync()
                        .ConfigureAwait(false);

                    var result = await dbContext.FakeLongIdTable.GetMaxLongIdOrDefaultAsync()
                        .ConfigureAwait(false);

                    var validatedResult = Assert.NotNull(result);
                    Assert.True(validatedResult > 0);
                }
            }

            /// <summary>
            /// Test to ensure that the method returns null.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsNullAsync()
            {
                using (var dbContext = await _dbContextFactory.CreateDbContextAsync()
                           .ConfigureAwait(false))
                {
                    var result = await dbContext.FakeLongIdTable.GetMaxLongIdOrDefaultAsync()
                        .ConfigureAwait(false);

                    Assert.Null(result);
                }
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="DbSetExtensions.GetMaxRowVersionOrDefault{TEntity}"/>.
        /// </summary>
        public sealed class GetMaxRowVersionOrDefaultMethod : Foundatio.Xunit.TestWithLoggingBase
        {
            private readonly IDbContextFactory<FakeDbContext> _dbContextFactory;

            /// <summary>
            /// Initializes a new instance of the <see cref="GetMaxRowVersionOrDefaultMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit logging output helper.</param>
            public GetMaxRowVersionOrDefaultMethod(ITestOutputHelper output)
                : base(output)
            {
                _dbContextFactory = new FakeDbContextFactory(Log);
            }

            /// <summary>
            /// Test to ensure that the method returns the expected value.
            /// </summary>
            [Fact]
            public void ReturnsValue()
            {
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 1, RowVersion = 1 });
                    _ = dbContext.SaveChanges();

                    var result = dbContext.FakeAddAudit.GetMaxRowVersionOrDefault();

                    var validatedResult = Assert.NotNull(result);
                    Assert.True(validatedResult > 0);
                }
            }

            /// <summary>
            /// Test to ensure that the method returns null.
            /// </summary>
            [Fact]
            public void ReturnsNull()
            {
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    var result = dbContext.FakeAddAudit.GetMaxRowVersionOrDefault();

                    Assert.Null(result);
                }
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="DbSetExtensions.GetMaxRowVersionOrDefaultAsync{TEntity}"/>.
        /// </summary>
        public sealed class GetMaxRowVersionOrDefaultAsyncMethod : Foundatio.Xunit.TestWithLoggingBase
        {
            private readonly IDbContextFactory<FakeDbContext> _dbContextFactory;

            /// <summary>
            /// Initializes a new instance of the <see cref="GetMaxRowVersionOrDefaultAsyncMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit logging output helper.</param>
            public GetMaxRowVersionOrDefaultAsyncMethod(ITestOutputHelper output)
                : base(output)
            {
                _dbContextFactory = new FakeDbContextFactory(Log);
            }

            /// <summary>
            /// Test to ensure that the method returns the expected value.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsValueAsync()
            {
                using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
                {
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 1, RowVersion = 1 });
                    _ = await dbContext.SaveChangesAsync()
                        .ConfigureAwait(false);

                    var result = await dbContext.FakeAddAudit.GetMaxRowVersionOrDefaultAsync()
                        .ConfigureAwait(false);

                    var validatedResult = Assert.NotNull(result);
                    Assert.True(validatedResult > 0);
                }
            }

            /// <summary>
            /// Test to ensure that the method returns null.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsNullAsync()
            {
                using (var dbContext = await _dbContextFactory.CreateDbContextAsync()
                           .ConfigureAwait(false))
                {
                    var result = await dbContext.FakeAddAudit.GetMaxRowVersionOrDefaultAsync()
                        .ConfigureAwait(false);

                    Assert.Null(result);
                }
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="DbSetExtensions.GetRowsGreaterThanIntId{TEntity}(DbSet{TEntity}, int)"/>.
        /// </summary>
        public sealed class GetRowsGreaterThanAndLessThanOrEqualToRowVersionsMethod : Foundatio.Xunit.TestWithLoggingBase
        {
            private readonly IDbContextFactory<FakeDbContext> _dbContextFactory;

            /// <summary>
            /// Initializes a new instance of the <see cref="GetRowsGreaterThanAndLessThanOrEqualToRowVersionsMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit logging output helper.</param>
            public GetRowsGreaterThanAndLessThanOrEqualToRowVersionsMethod(ITestOutputHelper output)
                : base(output)
            {
                _dbContextFactory = new FakeDbContextFactory(Log);
            }

            /// <summary>
            /// Test to ensure that the method returns empty when there an no new records.
            /// </summary>
            [Fact]
            public void ReturnsRows()
            {
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 1 });
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 2 });
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 3 });
                    _ = dbContext.SaveChanges();

                    var maxRowVersion = dbContext.FakeAddAudit.GetMaxRowVersionOrDefault();

                    var result = dbContext.FakeAddAudit.GetRowsGreaterThanAndLessThanOrEqualToRowVersions(1, maxRowVersion!.Value, 2).ToArray();

                    Assert.NotNull(result);
                    Assert.Equal(2, result.Length);
                }
            }

            /// <summary>
            /// Test to ensure that the method returns empty when there an no new records.
            /// </summary>
            [Fact]
            public void ReturnsEmpty()
            {
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 1 });
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 2 });
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 3 });
                    _ = dbContext.SaveChanges();

                    var maxRowVersion = dbContext.FakeAddAudit.GetMaxRowVersionOrDefault();
                    var result = dbContext.FakeAddAudit.GetRowsGreaterThanAndLessThanOrEqualToRowVersions(maxRowVersion!.Value, maxRowVersion!.Value, 2).ToArray();

                    Assert.NotNull(result);
                    Assert.Empty(result);
                }
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="DbSetExtensions.GetRowsGreaterThanLongId{TEntity}(DbSet{TEntity}, long)"/>.
        /// </summary>
        public sealed class GetRowsGreaterThanLongIdMethod : Foundatio.Xunit.TestWithLoggingBase
        {
            private readonly IDbContextFactory<FakeDbContext> _dbContextFactory;

            /// <summary>
            /// Initializes a new instance of the <see cref="GetRowsGreaterThanLongIdMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit logging output helper.</param>
            public GetRowsGreaterThanLongIdMethod(ITestOutputHelper output)
                : base(output)
            {
                _dbContextFactory = new FakeDbContextFactory(Log);
            }

            /// <summary>
            /// Test to ensure that the method returns empty when there an no new records.
            /// </summary>
            [Fact]
            public void ReturnsRows()
            {
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    _ = dbContext.FakeLongIdTable.Add(new FakeLongIdTableDbSet());
                    _ = dbContext.FakeLongIdTable.Add(new FakeLongIdTableDbSet());
                    _ = dbContext.FakeLongIdTable.Add(new FakeLongIdTableDbSet());
                    _ = dbContext.SaveChanges();

                    var result = dbContext.FakeLongIdTable.GetRowsGreaterThanLongId(1).ToArray();

                    Assert.NotNull(result);
                    Assert.Equal(2, result.Length);
                }
            }

            /// <summary>
            /// Test to ensure that the method returns empty when there an no new records.
            /// </summary>
            [Fact]
            public void ReturnsEmpty()
            {
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    _ = dbContext.FakeLongIdTable.Add(new FakeLongIdTableDbSet());
                    _ = dbContext.FakeLongIdTable.Add(new FakeLongIdTableDbSet());
                    _ = dbContext.FakeLongIdTable.Add(new FakeLongIdTableDbSet());
                    _ = dbContext.SaveChanges();

                    var result = dbContext.FakeLongIdTable.GetRowsGreaterThanLongId(3).ToArray();

                    Assert.NotNull(result);
                    Assert.Empty(result);
                }
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="DbSetExtensions.GetRowsGreaterThanLongId{TEntity}(DbSet{TEntity}, long)"/>.
        /// </summary>
        public sealed class GetRowsGreaterThanLongIdMethodWithTakeRecords : Foundatio.Xunit.TestWithLoggingBase
        {
            private readonly IDbContextFactory<FakeDbContext> _dbContextFactory;

            /// <summary>
            /// Initializes a new instance of the <see cref="GetRowsGreaterThanLongIdMethodWithTakeRecords"/> class.
            /// </summary>
            /// <param name="output">XUnit logging output helper.</param>
            public GetRowsGreaterThanLongIdMethodWithTakeRecords(ITestOutputHelper output)
                : base(output)
            {
                _dbContextFactory = new FakeDbContextFactory(Log);
            }

            /// <summary>
            /// Test to ensure that the method returns empty when there an no new records.
            /// </summary>
            [Fact]
            public void ReturnsRows()
            {
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    _ = dbContext.FakeLongIdTable.Add(new FakeLongIdTableDbSet());
                    _ = dbContext.FakeLongIdTable.Add(new FakeLongIdTableDbSet());
                    _ = dbContext.FakeLongIdTable.Add(new FakeLongIdTableDbSet());
                    _ = dbContext.SaveChanges();

                    var result = dbContext.FakeLongIdTable.GetRowsGreaterThanLongId(1, 1).ToArray();

                    Assert.NotNull(result);
                    _ = Assert.Single(result);
                }
            }

            /// <summary>
            /// Test to ensure that the method returns empty when there an no new records.
            /// </summary>
            [Fact]
            public void ReturnsEmpty()
            {
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    _ = dbContext.FakeLongIdTable.Add(new FakeLongIdTableDbSet());
                    _ = dbContext.FakeLongIdTable.Add(new FakeLongIdTableDbSet());
                    _ = dbContext.FakeLongIdTable.Add(new FakeLongIdTableDbSet());
                    _ = dbContext.SaveChanges();

                    var result = dbContext.FakeLongIdTable.GetRowsGreaterThanLongId(3, 1).ToArray();

                    Assert.NotNull(result);
                    Assert.Empty(result);
                }
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="DbSetExtensions.GetRowsGreaterThanLongId{TEntity}(DbSet{TEntity}, long)"/>.
        /// </summary>
        public sealed class GetRowsGreaterThanLongIdMethodWithSelector : Foundatio.Xunit.TestWithLoggingBase
        {
            private readonly IDbContextFactory<FakeDbContext> _dbContextFactory;

            /// <summary>
            /// Initializes a new instance of the <see cref="GetRowsGreaterThanLongIdMethodWithSelector"/> class.
            /// </summary>
            /// <param name="output">XUnit logging output helper.</param>
            public GetRowsGreaterThanLongIdMethodWithSelector(ITestOutputHelper output)
                : base(output)
            {
                _dbContextFactory = new FakeDbContextFactory(Log);
            }

            /// <summary>
            /// Test to ensure that the method returns empty when there an no new records.
            /// </summary>
            [Fact]
            public void ReturnsRows()
            {
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    _ = dbContext.FakeLongIdTable.Add(new FakeLongIdTableDbSet());
                    _ = dbContext.FakeLongIdTable.Add(new FakeLongIdTableDbSet());
                    _ = dbContext.FakeLongIdTable.Add(new FakeLongIdTableDbSet());
                    _ = dbContext.SaveChanges();

                    var result = dbContext.FakeLongIdTable.GetRowsGreaterThanLongId(
                        2,
                        set => set).ToArray();

                    Assert.NotNull(result);
                    _ = Assert.Single(result);
                }
            }

            /// <summary>
            /// Test to ensure that the method returns empty when there an no new records.
            /// </summary>
            [Fact]
            public void ReturnsEmpty()
            {
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    _ = dbContext.FakeLongIdTable.Add(new FakeLongIdTableDbSet());
                    _ = dbContext.FakeLongIdTable.Add(new FakeLongIdTableDbSet());
                    _ = dbContext.FakeLongIdTable.Add(new FakeLongIdTableDbSet());
                    _ = dbContext.SaveChanges();

                    var result = dbContext.FakeLongIdTable.GetRowsGreaterThanLongId(
                        3,
                        set => set).ToArray();

                    Assert.NotNull(result);
                    Assert.Empty(result);
                }
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="DbSetExtensions.GetRowsGreaterThanIntId{TEntity}(DbSet{TEntity}, int)"/>.
        /// </summary>
        public sealed class GetRowsGreaterThanLongIdMethodWithTakeRecordsAndSelector : Foundatio.Xunit.TestWithLoggingBase
        {
            private readonly IDbContextFactory<FakeDbContext> _dbContextFactory;

            /// <summary>
            /// Initializes a new instance of the <see cref="GetRowsGreaterThanLongIdMethodWithTakeRecordsAndSelector"/> class.
            /// </summary>
            /// <param name="output">XUnit logging output helper.</param>
            public GetRowsGreaterThanLongIdMethodWithTakeRecordsAndSelector(ITestOutputHelper output)
                : base(output)
            {
                _dbContextFactory = new FakeDbContextFactory(Log);
            }

            /// <summary>
            /// Test to ensure that the method returns empty when there an no new records.
            /// </summary>
            [Fact]
            public void ReturnsRows()
            {
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    _ = dbContext.FakeLongIdTable.Add(new FakeLongIdTableDbSet());
                    _ = dbContext.FakeLongIdTable.Add(new FakeLongIdTableDbSet());
                    _ = dbContext.FakeLongIdTable.Add(new FakeLongIdTableDbSet());
                    _ = dbContext.SaveChanges();

                    var result = dbContext.FakeLongIdTable.GetRowsGreaterThanLongId(
                        1,
                        1,
                        set => set).ToArray();

                    Assert.NotNull(result);
                    _ = Assert.Single(result);
                }
            }

            /// <summary>
            /// Test to ensure that the method returns empty when there an no new records.
            /// </summary>
            [Fact]
            public void ReturnsEmpty()
            {
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    _ = dbContext.FakeLongIdTable.Add(new FakeLongIdTableDbSet());
                    _ = dbContext.FakeLongIdTable.Add(new FakeLongIdTableDbSet());
                    _ = dbContext.FakeLongIdTable.Add(new FakeLongIdTableDbSet());
                    _ = dbContext.SaveChanges();

                    var result = dbContext.FakeLongIdTable.GetRowsGreaterThanLongId(
                        3,
                        1,
                        set => set).ToArray();

                    Assert.NotNull(result);
                    Assert.Empty(result);
                }
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="DbSetExtensions.GetRowsGreaterThanIntId{TEntity}(DbSet{TEntity}, int)"/>.
        /// </summary>
        public sealed class GetRowsGreaterThanIntIdMethod : Foundatio.Xunit.TestWithLoggingBase
        {
            private readonly IDbContextFactory<FakeDbContext> _dbContextFactory;

            /// <summary>
            /// Initializes a new instance of the <see cref="GetRowsGreaterThanIntIdMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit logging output helper.</param>
            public GetRowsGreaterThanIntIdMethod(ITestOutputHelper output)
                : base(output)
            {
                _dbContextFactory = new FakeDbContextFactory(Log);
            }

            /// <summary>
            /// Test to ensure that the method returns empty when there an no new records.
            /// </summary>
            [Fact]
            public void ReturnsRows()
            {
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 1 });
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 2 });
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 3 });
                    _ = dbContext.SaveChanges();

                    var result = dbContext.FakeAddAudit.GetRowsGreaterThanIntId(1).ToArray();

                    Assert.NotNull(result);
                    Assert.Equal(2, result.Length);
                }
            }

            /// <summary>
            /// Test to ensure that the method returns empty when there an no new records.
            /// </summary>
            [Fact]
            public void ReturnsEmpty()
            {
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 1 });
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 2 });
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 3 });
                    _ = dbContext.SaveChanges();

                    var result = dbContext.FakeAddAudit.GetRowsGreaterThanIntId(3).ToArray();

                    Assert.NotNull(result);
                    Assert.Empty(result);
                }
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="DbSetExtensions.GetRowsGreaterThanIntId{TEntity}(DbSet{TEntity}, int)"/>.
        /// </summary>
        public sealed class GetRowsGreaterThanIntIdMethodWithTakeRecords : Foundatio.Xunit.TestWithLoggingBase
        {
            private readonly IDbContextFactory<FakeDbContext> _dbContextFactory;

            /// <summary>
            /// Initializes a new instance of the <see cref="GetRowsGreaterThanIntIdMethodWithTakeRecords"/> class.
            /// </summary>
            /// <param name="output">XUnit logging output helper.</param>
            public GetRowsGreaterThanIntIdMethodWithTakeRecords(ITestOutputHelper output)
                : base(output)
            {
                _dbContextFactory = new FakeDbContextFactory(Log);
            }

            /// <summary>
            /// Test to ensure that the method returns empty when there an no new records.
            /// </summary>
            [Fact]
            public void ReturnsRows()
            {
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 1 });
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 2 });
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 3 });
                    _ = dbContext.SaveChanges();

                    var result = dbContext.FakeAddAudit.GetRowsGreaterThanIntId(1, 1).ToArray();

                    Assert.NotNull(result);
                    _ = Assert.Single(result);
                }
            }

            /// <summary>
            /// Test to ensure that the method returns empty when there an no new records.
            /// </summary>
            [Fact]
            public void ReturnsEmpty()
            {
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 1 });
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 2 });
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 3 });
                    _ = dbContext.SaveChanges();

                    var result = dbContext.FakeAddAudit.GetRowsGreaterThanIntId(3, 1).ToArray();

                    Assert.NotNull(result);
                    Assert.Empty(result);
                }
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="DbSetExtensions.GetRowsGreaterThanIntId{TEntity}(DbSet{TEntity}, int)"/>.
        /// </summary>
        public sealed class GetRowsGreaterThanIntIdMethodWithSelector : Foundatio.Xunit.TestWithLoggingBase
        {
            private readonly IDbContextFactory<FakeDbContext> _dbContextFactory;

            /// <summary>
            /// Initializes a new instance of the <see cref="GetRowsGreaterThanIntIdMethodWithSelector"/> class.
            /// </summary>
            /// <param name="output">XUnit logging output helper.</param>
            public GetRowsGreaterThanIntIdMethodWithSelector(ITestOutputHelper output)
                : base(output)
            {
                _dbContextFactory = new FakeDbContextFactory(Log);
            }

            /// <summary>
            /// Test to ensure that the method returns empty when there an no new records.
            /// </summary>
            [Fact]
            public void ReturnsRows()
            {
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 1 });
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 2 });
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 3 });
                    _ = dbContext.SaveChanges();

                    var result = dbContext.FakeAddAudit.GetRowsGreaterThanIntId(
                        2,
                        set => set).ToArray();

                    Assert.NotNull(result);
                    _ = Assert.Single(result);
                }
            }

            /// <summary>
            /// Test to ensure that the method returns empty when there an no new records.
            /// </summary>
            [Fact]
            public void ReturnsEmpty()
            {
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 1 });
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 2 });
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 3 });
                    _ = dbContext.SaveChanges();

                    var result = dbContext.FakeAddAudit.GetRowsGreaterThanIntId(
                        3,
                        set => set).ToArray();

                    Assert.NotNull(result);
                    Assert.Empty(result);
                }
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="DbSetExtensions.GetRowsGreaterThanIntId{TEntity}(DbSet{TEntity}, int)"/>.
        /// </summary>
        public sealed class GetRowsGreaterThanIntIdMethodWithTakeRecordsAndSelector : Foundatio.Xunit.TestWithLoggingBase
        {
            private readonly IDbContextFactory<FakeDbContext> _dbContextFactory;

            /// <summary>
            /// Initializes a new instance of the <see cref="GetRowsGreaterThanIntIdMethodWithTakeRecordsAndSelector"/> class.
            /// </summary>
            /// <param name="output">XUnit logging output helper.</param>
            public GetRowsGreaterThanIntIdMethodWithTakeRecordsAndSelector(ITestOutputHelper output)
                : base(output)
            {
                _dbContextFactory = new FakeDbContextFactory(Log);
            }

            /// <summary>
            /// Test to ensure that the method returns empty when there an no new records.
            /// </summary>
            [Fact]
            public void ReturnsRows()
            {
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 1 });
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 2 });
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 3 });
                    _ = dbContext.SaveChanges();

                    var result = dbContext.FakeAddAudit.GetRowsGreaterThanIntId(
                        1,
                        1,
                        set => set).ToArray();

                    Assert.NotNull(result);
                    _ = Assert.Single(result);
                }
            }

            /// <summary>
            /// Test to ensure that the method returns empty when there an no new records.
            /// </summary>
            [Fact]
            public void ReturnsEmpty()
            {
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 1 });
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 2 });
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 3 });
                    _ = dbContext.SaveChanges();

                    var result = dbContext.FakeAddAudit.GetRowsGreaterThanIntId(
                        3,
                        1,
                        set => set).ToArray();

                    Assert.NotNull(result);
                    Assert.Empty(result);
                }
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="DbSetExtensions.GetRowsGreaterThanIntId{TEntity}(DbSet{TEntity}, int)"/>.
        /// </summary>
        public sealed class GetRowsGreaterThanRowVersionMethodWithId : Foundatio.Xunit.TestWithLoggingBase
        {
            private readonly IDbContextFactory<FakeDbContext> _dbContextFactory;

            /// <summary>
            /// Initializes a new instance of the <see cref="GetRowsGreaterThanRowVersionMethodWithId"/> class.
            /// </summary>
            /// <param name="output">XUnit logging output helper.</param>
            public GetRowsGreaterThanRowVersionMethodWithId(ITestOutputHelper output)
                : base(output)
            {
                _dbContextFactory = new FakeDbContextFactory(Log);
            }

            /// <summary>
            /// Test to ensure that the method returns empty when there an no new records.
            /// </summary>
            [Fact]
            public void ReturnsRows()
            {
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 1 });
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 2 });
                    _ = dbContext.SaveChanges();

                    var maxRowVersion = dbContext.FakeAddAudit.GetMaxRowVersionOrDefault();

                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 3 });
                    _ = dbContext.SaveChanges();

                    var result = dbContext.FakeAddAudit.GetRowsGreaterThanRowVersion(maxRowVersion!.Value)
                        .ToArray();

                    Assert.NotNull(result);
                    _ = Assert.Single(result);
                }
            }

            /// <summary>
            /// Test to ensure that the method returns empty when there an no new records.
            /// </summary>
            [Fact]
            public void ReturnsEmpty()
            {
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 1 });
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 2 });
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 3 });
                    _ = dbContext.SaveChanges();

                    var maxRowVersion = dbContext.FakeAddAudit.GetMaxRowVersionOrDefault();

                    var result = dbContext.FakeAddAudit.GetRowsGreaterThanRowVersion(maxRowVersion!.Value)
                        .ToArray();

                    Assert.NotNull(result);
                    Assert.Empty(result);
                }
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="DbSetExtensions.GetRowsGreaterThanIntId{TEntity}(DbSet{TEntity}, int)"/>.
        /// </summary>
        public sealed class GetRowsGreaterThanRowVersionMethodWithIdAndTakeRecords : Foundatio.Xunit.TestWithLoggingBase
        {
            private readonly IDbContextFactory<FakeDbContext> _dbContextFactory;

            /// <summary>
            /// Initializes a new instance of the <see cref="GetRowsGreaterThanRowVersionMethodWithIdAndTakeRecords"/> class.
            /// </summary>
            /// <param name="output">XUnit logging output helper.</param>
            public GetRowsGreaterThanRowVersionMethodWithIdAndTakeRecords(ITestOutputHelper output)
                : base(output)
            {
                _dbContextFactory = new FakeDbContextFactory(Log);
            }

            /// <summary>
            /// Test to ensure that the method returns empty when there an no new records.
            /// </summary>
            [Fact]
            public void ReturnsRows()
            {
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 1, RowVersion = 1 });
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 2, RowVersion = 2 });
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 3, RowVersion = 3 });
                    _ = dbContext.SaveChanges();

                    var result = dbContext.FakeAddAudit.GetRowsGreaterThanRowVersion(1, 2)
                        .ToArray();

                    Assert.NotNull(result);
                    Assert.Equal(2, result.Length);
                }
            }

            /// <summary>
            /// Test to ensure that the method returns empty when there an no new records.
            /// </summary>
            [Fact]
            public void ReturnsEmpty()
            {
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 1 });
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 2 });
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 3 });
                    _ = dbContext.SaveChanges();

                    var maxVersion = dbContext.FakeAddAudit.GetMaxRowVersionOrDefault();

                    var result = dbContext.FakeAddAudit.GetRowsGreaterThanRowVersion(maxVersion!.Value, 2)
                        .ToArray();

                    Assert.NotNull(result);
                    Assert.Empty(result);
                }
            }
        }
    }
}
