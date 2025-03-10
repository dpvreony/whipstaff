// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Whipstaff.AspNetCore.Features.RequireForwardedForHeader;
using Whipstaff.Testing.Logging;
using Xunit;

namespace Whipstaff.UnitTests.Features.RequireForwardedForHeader
{
    /// <summary>
    /// Unit Tests for the RequireForwardedForHeaderMiddleware.
    /// </summary>
    public static class RequireForwardedForHeaderMiddlewareTests
    {
        private static Task NextAsync(HttpContext httpContext)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Unit tests for the constructor method.
        /// </summary>
        public sealed class ConstructorMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ConstructorMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit Test Output helper.</param>
            public ConstructorMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure an Argument Null Exception is thrown.
            /// </summary>
            [Fact]
            public void ThrowsArgumentNullException()
            {
                var exception = Assert.Throws<ArgumentNullException>(() => new RequireForwardedForHeaderMiddleware(null!));

                Assert.Equal("next", exception.ParamName);
            }

            /// <summary>
            /// Tests to ensure an instance is returned.
            /// </summary>
            [Fact]
            public void ReturnsInstance()
            {
                var instance = new RequireForwardedForHeaderMiddleware(NextAsync);
                Assert.NotNull(instance);
            }
        }

        /// <summary>
        /// Unit Tests for the InvokeAsync method.
        /// </summary>
        public sealed class InvokeAsyncMethod
        {
            /// <summary>
            /// Test Data for the Rejection of Http Requests.
            /// </summary>
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1401 // Fields should be private
            public static IEnumerable<object[]> RejectsRequestTestData = GetRejectsRequestTestData();
#pragma warning restore SA1401 // Fields should be private
#pragma warning restore CA2211 // Non-constant fields should not be visible

            /// <summary>
            /// Tests to ensure the middleware rejects requests without
            /// X-Forwarded-... http headers.
            /// </summary>
            /// <param name="httpContext">The request HTTP context.</param>
            /// <param name="expectedHttpStatusCode">The expected HTTP Status code that will be returned.</param>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Theory]
            [MemberData(nameof(RejectsRequestTestData))]
            public async Task RejectsRequestAsync(
                HttpContext httpContext,
                int expectedHttpStatusCode)
            {
                var instance = new RequireForwardedForHeaderMiddleware(NextAsync);
                await instance.InvokeAsync(httpContext)
;

                Assert.Equal(
                    expectedHttpStatusCode,
#pragma warning disable CA1062 // Validate arguments of public methods
                    httpContext.Response.StatusCode);
#pragma warning restore CA1062 // Validate arguments of public methods
            }

            private static object[][] GetRejectsRequestTestData()
            {
                return new[]
                {
                    GetXForwardedForRequestTestData(),
                    GetXForwardedProtoHttpMissingRequestTestData(),
                    GetXForwardedProtoHttpInsecureRequestTestData(),
                    GetXForwardedHostRequestTestData(),
                };
            }

            private static object[] GetXForwardedForRequestTestData()
            {
                return new object[]
                {
                    GetXForwardForHttpContext(),
                    WhipcordHttpStatusCode.ExpectedXForwardedFor,
                };
            }

            private static object[] GetXForwardedProtoHttpMissingRequestTestData()
            {
                return new object[]
                {
                    GetXForwardProtoHttpMissingProto(),
                    WhipcordHttpStatusCode.ExpectedXForwardedProto,
                };
            }

            private static object[] GetXForwardedProtoHttpInsecureRequestTestData()
            {
                return new object[]
                {
                    GetXForwardProtoHttpInsecureProto(),
                    WhipcordHttpStatusCode.ExpectedXForwardedProto,
                };
            }

            private static object[] GetXForwardedHostRequestTestData()
            {
                return new object[]
                {
                    GetXForwardHostHttpHost(),
                    WhipcordHttpStatusCode.ExpectedXForwardedHost,
                };
            }

            private static DefaultHttpContext GetXForwardForHttpContext()
            {
                var httpContext = new DefaultHttpContext();

                var headers = httpContext.Request.Headers;
                headers.Append("X-Forwarded-Proto", "https");
                headers.Append("X-Forwarded-Host", "localhost");

                return httpContext;
            }

            private static DefaultHttpContext GetXForwardProtoHttpMissingProto()
            {
                var httpContext = new DefaultHttpContext();

                var headers = httpContext.Request.Headers;
                headers.Append("X-Forwarded-For", "192.168.0.1");
                headers.Append("X-Forwarded-Host", "localhost");

                return httpContext;
            }

            private static DefaultHttpContext GetXForwardProtoHttpInsecureProto()
            {
                var httpContext = new DefaultHttpContext();

                var headers = httpContext.Request.Headers;
                headers.Append("X-Forwarded-For", "192.168.0.1");
                headers.Append("X-Forwarded-Proto", "http");
                headers.Append("X-Forwarded-Host", "localhost");

                return httpContext;
            }

            private static DefaultHttpContext GetXForwardHostHttpHost()
            {
                var httpContext = new DefaultHttpContext();

                var headers = httpContext.Request.Headers;
                headers.Append("X-Forwarded-For", "192.168.0.1");
                headers.Append("X-Forwarded-Proto", "https");

                return httpContext;
            }
        }
    }
}
