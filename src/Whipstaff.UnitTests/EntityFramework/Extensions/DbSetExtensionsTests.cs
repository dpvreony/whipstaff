// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        /// Unit Tests for <see cref="DbSetExtensions.GetMaxIntIdOrDefault"/>.
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
                _dbContextFactory = new FakeDbContextFactory();
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
                    Assert.NotNull(result);
                    Assert.True(result.Value > 0);
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
        /// Unit Tests for <see cref="DbSetExtensions.GetMaxIntIdOrDefaultAsync"/>.
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
                _dbContextFactory = new FakeDbContextFactory();
            }

            /// <summary>
            /// Test to ensure that the method returns the expected value.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsValue()
            {
                using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
                {
                    _ = dbContext.FakeAddAudit.Add(new FakeAddAuditDbSet { Value = 1 });
                    _ = dbContext.SaveChangesAsync()
                        .ConfigureAwait(false);

                    var result = await dbContext.FakeAddAudit.GetMaxIntIdOrDefaultAsync()
                        .ConfigureAwait(false);

                    Assert.NotNull(result);
                    Assert.True(result.Value > 0);
                }
            }

            /// <summary>
            /// Test to ensure that the method returns null.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsNull()
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
        /// Unit Tests for <see cref="DbSetExtensions.GetMaxLongIdOrDefault"/>.
        /// </summary>
        public sealed class GetMaxLongIdOrDefaultMethod : Foundatio.Xunit.TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetMaxLongIdOrDefaultMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit logging output helper.</param>
            public GetMaxLongIdOrDefaultMethod(ITestOutputHelper output)
                : base(output)
            {
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="DbSetExtensions.GetMaxRowVersionOrDefault"/>.
        /// </summary>
        public sealed class GetMaxRowVersionOrDefaultMethod : Foundatio.Xunit.TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetMaxRowVersionOrDefaultMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit logging output helper.</param>
            public GetMaxRowVersionOrDefaultMethod(ITestOutputHelper output)
                : base(output)
            {
            }
        }
    }
}
