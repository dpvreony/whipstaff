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
            /// Tests to ensure the middleware rejects requests without
            /// X-Forwarded-... http headers.
            /// </summary>
            /// <param name="httpContext">The request HTTP context.</param>
            /// <param name="expectedHttpStatusCode">The expected HTTP Status code that will be returned.</param>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Theory]
            [ClassData(typeof(RejectsRequestTestSource))]
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

            /// <summary>
            /// Test source for the RejectsRequestAsync method.
            /// </summary>
            public sealed class RejectsRequestTestSource : TheoryData<HttpContext, int>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="RejectsRequestTestSource"/> class.
                /// </summary>
                public RejectsRequestTestSource()
                {
                    Add(GetXForwardedForRequestTestData());
                    Add(GetXForwardedProtoHttpMissingRequestTestData());
                    Add(GetXForwardedProtoHttpInsecureRequestTestData());
                    Add(GetXForwardedHostRequestTestData());
                }

                private static TheoryDataRow<HttpContext, int> GetXForwardedForRequestTestData()
                {
                    return new TheoryDataRow<HttpContext, int>(
                        GetXForwardForHttpContext(),
                        WhipcordHttpStatusCode.ExpectedXForwardedFor);
                }

                private static TheoryDataRow<HttpContext, int> GetXForwardedProtoHttpMissingRequestTestData()
                {
                    return new TheoryDataRow<HttpContext, int>(
                        GetXForwardProtoHttpMissingProto(),
                        WhipcordHttpStatusCode.ExpectedXForwardedProto);
                }

                private static TheoryDataRow<HttpContext, int> GetXForwardedProtoHttpInsecureRequestTestData()
                {
                    return new TheoryDataRow<HttpContext, int>(
                        GetXForwardProtoHttpInsecureProto(),
                        WhipcordHttpStatusCode.ExpectedXForwardedProto);
                }

                private static TheoryDataRow<HttpContext, int> GetXForwardedHostRequestTestData()
                {
                    return new TheoryDataRow<HttpContext, int>(
                        GetXForwardHostHttpHost(),
                        WhipcordHttpStatusCode.ExpectedXForwardedHost);
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
}
