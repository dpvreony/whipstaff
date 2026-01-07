// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Mediator;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetTestRegimentation.XUnit.Logging;
using Whipstaff.EntityFramework.ModelCreation;
using Whipstaff.EntityFramework.RowVersionSaving;
using Whipstaff.Mediator;
using Whipstaff.Testing.Cqrs;
using Whipstaff.Testing.EntityFramework;
using Whipstaff.Testing.Logging;
using Whipstaff.Testing.Mediator;
using Xunit;

namespace Whipstaff.UnitTests.Features.Mediatr
{
    /// <summary>
    /// Unit tests for the MediatrHelpers class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class MediatrHelperTests
    {
        /// <summary>
        /// Unit tests for the RegisterMediatrWithExplicitTypes method.
        /// </summary>
        public sealed class RegisterMediatrWithExplicitTypesMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="RegisterMediatrWithExplicitTypesMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit Test Output helper.</param>
            public RegisterMediatrWithExplicitTypesMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Tests to ensure registration successfully takes place.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ShouldSucceedAsync()
            {
                var services = new ServiceCollection();

                _ = services.AddScoped<ILoggerFactory>(_ => LoggerFactory);
                _ = services.AddLogging();

#pragma warning disable CA2000
                var dbConnection = CreateInMemoryDatabase();
#pragma warning restore CA2000

                _ = services.AddTransient(_ => new DbContextOptionsBuilder<FakeDbContext>()
                    .UseSqlite(dbConnection)
                    .AddInterceptors(new RowVersionSaveChangesInterceptor())
                    .Options);
                _ = services.AddSingleton<Func<IModelCreator<FakeDbContext>>>(x =>
                    () => new SqliteFakeDbContextModelCreator());

                MediatorHelpers.RegisterMediatorWithExplicitTypes(
                    services,
                    null,
                    new FakeMediatorRegistration());

                _ = services.AddSingleton<IMediator, FakeMediator>();

                var serviceProvider = services.BuildServiceProvider();

                var dbContextOptions = serviceProvider.GetService<DbContextOptions<FakeDbContext>>();
                using (var dbContext = new FakeDbContext(dbContextOptions!, () => new SqliteFakeDbContextModelCreator()))
                {
#pragma warning disable GR0020 // Do not use Entity Framework Database EnsureCreatedAsync.
                    _ = await dbContext.Database.EnsureCreatedAsync(TestContext.Current.CancellationToken);
#pragma warning restore GR0020 // Do not use Entity Framework Database EnsureCreatedAsync.

                    var entityCount = await dbContext.FakeAddAudit.CountAsync(cancellationToken: TestContext.Current.CancellationToken);
                    Assert.Equal(0, entityCount);

                    entityCount = await dbContext.FakeAddPreProcessAudit.CountAsync(cancellationToken: TestContext.Current.CancellationToken);
                    Assert.Equal(0, entityCount);

                    entityCount = await dbContext.FakeAddPostProcessAudit.CountAsync(cancellationToken: TestContext.Current.CancellationToken);
                    Assert.Equal(0, entityCount);
                }

                var mediator = serviceProvider.GetRequiredService<IMediator>();
                const int expected = 987654321;
                var request = new FakeCrudAddCommand(expected, ClaimsPrincipal.Current!);
                var sendResult = await mediator.Send(request, TestContext.Current.CancellationToken);
                Assert.Equal(expected, sendResult);

                using (var dbContext = new FakeDbContext(dbContextOptions!, () => new SqliteFakeDbContextModelCreator()))
                {
                    var entityCount = await dbContext.FakeAddAudit.CountAsync(cancellationToken: TestContext.Current.CancellationToken);
                    Assert.Equal(1, entityCount);
                }

                var notification = new FakeNotification();
                await mediator.Publish(notification, TestContext.Current.CancellationToken);
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
