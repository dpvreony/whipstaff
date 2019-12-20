using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Fakes;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Dhgms.AspNetCoreContrib.UnitTests.Features.Mediatr
{
    /// <summary>
    /// Unit tests for the MediatrHelpers class.
    /// </summary>
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
                Dhgms.AspNetCoreContrib.App.Features.Mediatr.MediatrHelpers.RegisterMediatrWithExplicitTypes(
                    services,
                    null,
                    new MediatRServiceConfiguration(),
                    new FakeMediatrRegistration());

                var serviceProvider = services.BuildServiceProvider();
                var mediator = serviceProvider.GetService<IMediator>();
                const int expected = 987654321;
                var request = new FakeCrudAddCommand(expected, ClaimsPrincipal.Current);
                var result = await mediator.Send(request).ConfigureAwait(false);
                Assert.Equal(expected,result);
            }

        }
    }
}
