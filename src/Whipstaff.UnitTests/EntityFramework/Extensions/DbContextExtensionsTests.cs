// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetTestRegimentation.XUnit.Logging;
using Whipstaff.EntityFramework.Extensions;
using Whipstaff.Testing.EntityFramework;
using Whipstaff.Testing.EntityFramework.DbSets;
using Whipstaff.Testing.Logging;
using Xunit;

namespace Whipstaff.UnitTests.EntityFramework.Extensions
{
    /// <summary>
    /// Unit tests for <see cref="DbContextExtensions"/>.
    /// </summary>
    public static class DbContextExtensionsTests
    {
        /// <summary>
        /// Unit tests for <see cref="DbContextExtensions.GetOrAddAsync{TDbContext,TEntity}(TDbContext,Func{TDbContext,DbSet{TEntity}},System.Linq.Expressions.Expression{Func{TEntity,bool}},Func{TEntity})"/>.
        /// </summary>
        public sealed class GetOrAddAsyncMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetOrAddAsyncMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit logging output helper.</param>
            public GetOrAddAsyncMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Tests that a row is added when the predicate is not matched.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task AddsRecordAsync()
            {
                var dbContextFactory = new FakeDbContextFactory(LoggerFactory);
                var dbContext = dbContextFactory.CreateDbContext();
                var result = await dbContext.GetOrAddAsync(
                    x => x.FakeAddAudit,
                    x => x.Value == 2,
                    () => new FakeAddAuditDbSet { Value = 2 });

                Assert.NotNull(result);
                Assert.True(result.Id > 0);
                Assert.Equal(2, result.Value);
                Assert.True(result.RowVersion > 0);
            }
        }
    }
}
