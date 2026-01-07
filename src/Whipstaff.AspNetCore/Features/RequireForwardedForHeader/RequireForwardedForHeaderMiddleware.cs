// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Whipstaff.AspNetCore.Features.RequireForwardedForHeader
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
#pragma warning disable GR0027 // Constructor should have a logging framework instance as the final parameter.
        public RequireForwardedForHeaderMiddleware(RequestDelegate next)
        {
            ArgumentNullException.ThrowIfNull(next);
            _next = next;
        }
#pragma warning restore GR0027 // Constructor should have a logging framework instance as the final parameter.

        /// <summary>
        /// Handles a Middleware request.
        /// </summary>
        /// <param name="context">Http Context for the current request.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

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

            switch (xForwardedProto.Count)
            {
                case 1:
                    var value = xForwardedProto[0];
                    if (string.IsNullOrWhiteSpace(value) || !value.Equals("https", StringComparison.OrdinalIgnoreCase))
                    {
                        await WriteResponseAsync(context.Response, WhipcordHttpStatusCode.ExpectedXForwardedProto)
                            .ConfigureAwait(false);
                    }

                    break;
                default:
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
            int statusCode)
        {
            response.StatusCode = statusCode;
            await response.WriteAsync("Load Balancer Configuration Issue.")
                .ConfigureAwait(false);
        }
    }
}
