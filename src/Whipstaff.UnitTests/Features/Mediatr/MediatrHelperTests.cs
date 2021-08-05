﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Whipstaff.Core.Mediatr;
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
        public sealed class RegisterMediatrWithExplicitTypesMethod : Foundatio.Logging.Xunit.TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="RegisterMediatrWithExplicitTypesMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit Test Output helper.</param>
            public RegisterMediatrWithExplicitTypesMethod(ITestOutputHelper output) : base(output)
            {
            }

            /// <summary>
            /// Tests to ensure registration successfully takes place.
            /// </summary>
            [Fact]
            public async Task ShouldSucceed()
            {
                var services = new ServiceCollection();

                var databaseName = Guid.NewGuid().ToString();
                services.AddTransient(_ => new DbContextOptionsBuilder<FakeDbContext>()
                    .UseInMemoryDatabase(databaseName: databaseName)
                    .Options);

                // services.AddEntityFrameworkInMemoryDatabase();

                MediatrHelpers.RegisterMediatrWithExplicitTypes(
                    services,
                    null,
                    new MediatRServiceConfiguration(),
                    new FakeMediatrRegistration());

                var serviceProvider = services.BuildServiceProvider();

                using (var dbContext = new FakeDbContext(serviceProvider.GetService<DbContextOptions<FakeDbContext>>()))
                {
                    var entityCount = dbContext.FakeAddAudit.Count();
                    Assert.Equal(0, entityCount);
                }

                var mediator = serviceProvider.GetService<IMediator>();
                const int expected = 987654321;
                var request = new FakeCrudAddCommand(expected, ClaimsPrincipal.Current);
                var sendResult = await mediator.Send(request).ConfigureAwait(false);
                Assert.Equal(expected, sendResult);

                using (var dbContext = new FakeDbContext(serviceProvider.GetService<DbContextOptions<FakeDbContext>>()))
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
                    .ConfigureAwait(false);
            }
        }
    }
}
