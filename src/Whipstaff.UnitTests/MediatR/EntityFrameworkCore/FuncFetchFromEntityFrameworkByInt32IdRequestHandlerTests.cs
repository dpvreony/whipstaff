// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualBasic.Logging;
using Whipstaff.MediatR.EntityFrameworkCore;
using Whipstaff.Testing.Cqrs;
using Whipstaff.Testing.EntityFramework;
using Whipstaff.Testing.EntityFramework.DbSets;
using Whipstaff.Testing.Logging;
using Xunit;

namespace Whipstaff.UnitTests.MediatR.EntityFrameworkCore
{
    /// <summary>
    /// Unit Tests for the Functional class to fetch an entity for a MediatR request based on an id that is an int32.
    /// </summary>
    public static class FuncFetchFromEntityFrameworkByInt32IdRequestHandlerTests
    {
        /// <inheritdoc />
        public sealed class ConstructorMethod : TestWithLoggingBase, NetTestRegimentation.ITestConstructorMethod
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ConstructorMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public ConstructorMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc />
            [Fact]
            public void ReturnsInstance()
            {
                var dbContextFactory = new FakeDbContextFactory(Log);

                var instance =
                    new FuncFetchFromEntityFrameworkByInt32IdQueryHandler<RequestById, FakeDbContext, FakeAddAuditDbSet, int?>(
                        dbContextFactory,
                        context => context.FakeAddAudit,
                        set => set.Id);

                Assert.NotNull(instance);
            }
        }

        /// <summary>
        /// Unit Tests for the Handle Method.
        /// </summary>
        public sealed class HandleMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="HandleMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public HandleMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Unit Tests for ensuring a result is returned.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsResultAsync()
            {
                var dbContextFactory = new FakeDbContextFactory(Log);

                using (var dbContext = dbContextFactory.CreateDbContext())
                {
                    _ = await dbContext.FakeAddAudit.AddAsync(new FakeAddAuditDbSet { Value = 1 });
                    _ = await dbContext.SaveChangesAsync();
                }

                var instance =
                    new FuncFetchFromEntityFrameworkByInt32IdQueryHandler<RequestById, FakeDbContext, FakeAddAuditDbSet, int?>(
                        dbContextFactory,
                        context => context.FakeAddAudit,
                        set => set.Id);

                var request = new RequestById
                {
                    Id = 1
                };

                var response = await instance.Handle(
                    request,
                    CancellationToken.None);

                Assert.Equal(1, response);
            }

            /// <summary>
            /// Unit Tests for ensuring a result is returned.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsNullAsync()
            {
                var dbContextFactory = new FakeDbContextFactory(Log);

                var instance =
                    new FuncFetchFromEntityFrameworkByInt32IdQueryHandler<RequestById, FakeDbContext, FakeAddAuditDbSet, int?>(
                        dbContextFactory,
                        context => context.FakeAddAudit,
                        set => set.Id);

                var request = new RequestById
                {
                    Id = 1
                };

                var response = await instance.Handle(
                    request,
                    CancellationToken.None);

                Assert.Null(response);
            }
        }
    }
}
