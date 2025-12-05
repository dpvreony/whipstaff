// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Net.Http;
using Microsoft.Playwright;
using Rocks;
using Whipstaff.Playwright;
using Xunit;

[assembly: Rock(typeof(IRequest), BuildType.Create)]

namespace Whipstaff.UnitTests.Playwright
{
    /// <summary>
    /// Unit Tests for <see cref="Whipstaff.Playwright.RequestExtensions"/>.
    /// </summary>
    public static class RequestExtensionTests
    {
        /// <summary>
        /// Unit tests for <see cref="Whipstaff.Playwright.RequestExtensions.ToHttpRequestMessage(IRequest)"/>.
        /// </summary>
        public sealed class ToHttpRequestMessageMethod
        {
            /// <summary>
            /// Tests to ensure a request is mapped as expected.
            /// </summary>
            /// <param name="request">The request to convert.</param>
            /// <param name="expectedMethod">The expected http method.</param>
            [Theory]
            [ClassData(typeof(ReturnsResultTestSource))]
            public void ReturnsResult(IRequest request, HttpMethod expectedMethod)
            {
                var httpRequestMessage = request.ToHttpRequestMessage();
                Assert.NotNull(httpRequestMessage);
                Assert.Equal(httpRequestMessage.Method, expectedMethod);
            }
        }

        /// <summary>
        /// Test source for <see cref="ToHttpRequestMessageMethod"/>.
        /// </summary>
        public sealed class ReturnsResultTestSource : TheoryData<IRequest, HttpMethod>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ReturnsResultTestSource"/> class.
            /// </summary>
            public ReturnsResultTestSource()
            {
                Add(GetConnectRequest(), HttpMethod.Connect);
                Add(GetDeleteRequest(), HttpMethod.Delete);
                Add(GetGetRequest(), HttpMethod.Get);
                Add(GetHeadRequest(), HttpMethod.Head);
                Add(GetOptionsRequest(), HttpMethod.Options);
                Add(GetPatchRequest(), HttpMethod.Patch);
                Add(GetPostRequest(), HttpMethod.Post);
                Add(GetPutRequest(), HttpMethod.Put);
                Add(GetTraceRequest(), HttpMethod.Trace);
            }

            private static IRequest GetRequest(string method)
            {
                var expectation = new IRequestCreateExpectations();

                var setups = expectation.Setups;
                _ = setups.Method.Gets().ReturnValue(method);
                _ = setups.Url.Gets().ReturnValue("https://localhost/");
                _ = setups.Headers.Gets().ReturnValue([]);

                if (method.Equals("POST", StringComparison.Ordinal))
                {
                    _ = setups.PostDataBuffer.Gets().ReturnValue([1, 2, 3, 4, 5, 6, 7, 8, 9]);
                }

                return expectation.Instance();
            }

            private static IRequest GetConnectRequest() => GetRequest("CONNECT");

            private static IRequest GetDeleteRequest() => GetRequest("DELETE");

            private static IRequest GetGetRequest() => GetRequest("GET");

            private static IRequest GetHeadRequest() => GetRequest("HEAD");

            private static IRequest GetOptionsRequest() => GetRequest("OPTIONS");

            private static IRequest GetPatchRequest() => GetRequest("PATCH");

            private static IRequest GetPostRequest() => GetRequest("POST");

            private static IRequest GetPutRequest() => GetRequest("PUT");

            private static IRequest GetTraceRequest() => GetRequest("TRACE");
        }
    }
}
