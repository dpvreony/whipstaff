// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Net.Http;
using Microsoft.Playwright;
using Whipstaff.Runtime.Http;

namespace Whipstaff.Playwright
{
    /// <summary>
    /// Extension methods for <see cref="IRequest"/>.
    /// </summary>
    public static class RequestExtensions
    {
        /// <summary>
        /// Converts a <see cref="IRequest"/> to a <see cref="HttpRequestMessage"/>.
        /// </summary>
        /// <param name="request">Request to convert.</param>
        /// <returns>A <see cref="HttpRequestMessage"/> representing the request.</returns>
        public static HttpRequestMessage ToHttpRequestMessage(this IRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var httpRequestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(request.Url)
            };
            httpRequestMessage.AddHeaders(request.Headers);

            switch (request.Method)
            {
                case "CONNECT":
                    httpRequestMessage.Method = HttpMethod.Connect;
                    break;
                case "DELETE":
                    httpRequestMessage.Method = HttpMethod.Delete;
                    break;
                case "GET":
                    httpRequestMessage.Method = HttpMethod.Get;
                    break;
                case "HEAD":
                    httpRequestMessage.Method = HttpMethod.Head;
                    break;
                case "OPTIONS":
                    httpRequestMessage.Method = HttpMethod.Options;
                    break;
                case "PATCH":
                    httpRequestMessage.Method = HttpMethod.Patch;
                    break;
                case "POST":
                    httpRequestMessage.Method = HttpMethod.Post;

                    if (request.PostDataBuffer != null)
                    {
                        httpRequestMessage.Content = new StreamContent(new MemoryStream(request.PostDataBuffer));
                    }

                    break;
                case "PUT":
                    httpRequestMessage.Method = HttpMethod.Put;
                    break;
                case "TRACE":
                    httpRequestMessage.Method = HttpMethod.Trace;
                    break;
                default:
                    throw new ArgumentException($"Failed to map request HTTP method: {request.Method}", nameof(request));
            }

            return httpRequestMessage;
        }
    }
}
