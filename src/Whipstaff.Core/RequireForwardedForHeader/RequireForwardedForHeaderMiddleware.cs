// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Whipstaff.Core.RequireForwardedForHeader
{
    /// <summary>
    /// Middleware for checking the X-FORWARDED-FOR header exists on requests.
    /// </summary>
    public sealed class RequireForwardedForHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequireForwardedForHeaderMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next Middleware in the pipeline.</param>
        public RequireForwardedForHeaderMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        /// <summary>
        /// Handles a Middleware request.
        /// </summary>
        /// <param name="context">Http Context for the current request.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var headers = context.Request.Headers;

            if (!headers.ContainsKey("X-Forwarded-For"))
            {
                await WriteResponseAsync(context.Response, WhipcordHttpStatusCode.ExpectedXForwardedFor)
                    .ConfigureAwait(false);
                return;
            }

            if (!headers.TryGetValue("X-Forwarded-Proto", out var xForwardedProto))
            {
                await WriteResponseAsync(context.Response, WhipcordHttpStatusCode.ExpectedXForwardedProto)
                    .ConfigureAwait(false);
                return;
            }

            if (xForwardedProto.Count != 1
                || !xForwardedProto.First().Equals("https", StringComparison.OrdinalIgnoreCase))
            {
                await WriteResponseAsync(context.Response, WhipcordHttpStatusCode.ExpectedXForwardedProto)
                    .ConfigureAwait(false);
                return;
            }

            if (!headers.ContainsKey("X-Forwarded-Host"))
            {
                await WriteResponseAsync(context.Response, WhipcordHttpStatusCode.ExpectedXForwardedHost)
                    .ConfigureAwait(false);
                return;
            }

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }

        private static async Task WriteResponseAsync(
            HttpResponse response,
            WhipcordHttpStatusCode statusCode)
        {
            response.StatusCode = (int)statusCode;
            await response.WriteAsync("Load Balancer Configuration Issue.")
                .ConfigureAwait(false);
        }
    }
}
