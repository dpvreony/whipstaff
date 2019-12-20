using System;
using System.Collections.Generic;
using System.Text;
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
            public void ShouldSucceed()
            {
                var services = new ServiceCollection();
                Dhgms.AspNetCoreContrib.App.Features.Mediatr.MediatrHelpers.RegisterMediatrWithExplicitTypes(
                    services,
                    null,
                    null,
                    new FakeMediatrRegistration());
            }

        }
    }
}
