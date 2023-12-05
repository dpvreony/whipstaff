﻿// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Whipstaff.Core.Mediatr;
using Whipstaff.EntityFramework.ModelCreation;
using Whipstaff.EntityFramework.RowVersionSaving;
using Whipstaff.Testing.Cqrs;
using Whipstaff.Testing.EntityFramework;
using Whipstaff.Testing.MediatR;
using Xunit;
using Xunit.Abstractions;

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
        public sealed class RegisterMediatrWithExplicitTypesMethod : Foundatio.Xunit.TestWithLoggingBase
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

                _ = services.AddScoped<ILoggerFactory>(_ => Log);
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

                MediatrHelpers.RegisterMediatrWithExplicitTypes(
                    services,
                    null,
                    new MediatRServiceConfiguration(),
                    new FakeMediatrRegistration());

                var serviceProvider = services.BuildServiceProvider();

                var dbContextOptions = serviceProvider.GetService<DbContextOptions<FakeDbContext>>();
                using (var dbContext = new FakeDbContext(dbContextOptions!, () => new SqliteFakeDbContextModelCreator()))
                {
                    _ = await dbContext.Database.EnsureCreatedAsync();

                    var entityCount = dbContext.FakeAddAudit.Count();
                    Assert.Equal(0, entityCount);

                    entityCount = dbContext.FakeAddPreProcessAudit.Count();
                    Assert.Equal(0, entityCount);

                    entityCount = dbContext.FakeAddPostProcessAudit.Count();
                    Assert.Equal(0, entityCount);
                }

                var mediator = serviceProvider.GetService<IMediator>();
                const int expected = 987654321;
                var request = new FakeCrudAddCommand(expected, ClaimsPrincipal.Current!);
                var sendResult = await mediator!.Send(request);
                Assert.Equal(expected, sendResult);

                using (var dbContext = new FakeDbContext(dbContextOptions!, () => new SqliteFakeDbContextModelCreator()))
                {
                    var entityCount = dbContext.FakeAddAudit.Count();
                    Assert.Equal(1, entityCount);

                    entityCount = dbContext.FakeAddPreProcessAudit.Count();
                    Assert.Equal(1, entityCount);

                    entityCount = dbContext.FakeAddPostProcessAudit.Count();
                    Assert.Equal(1, entityCount);
                }

                var notification = new FakeNotification();
                await mediator.Publish(notification)
;
            }

            private static DbConnection CreateInMemoryDatabase()
            {
                var connection = new SqliteConnection("Filename=:memory:");

                connection.Open();

                return connection;
            }
        }
    }
}
