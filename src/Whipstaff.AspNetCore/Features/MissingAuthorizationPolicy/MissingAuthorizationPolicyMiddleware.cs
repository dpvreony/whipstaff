// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Whipstaff.AspNetCore.Features.MissingAuthorizationPolicy
{
    /// <summary>
    /// Middleware that logs and returns a 404 when an endpoint references an authorization policy that has not been configured.
    /// </summary>
    public sealed class MissingAuthorizationPolicyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MissingAuthorizationPolicyMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingAuthorizationPolicyMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next Middleware in the pipeline.</param>
        /// <param name="logger">Logging framework instance.</param>
        public MissingAuthorizationPolicyMiddleware(
            RequestDelegate next,
            ILogger<MissingAuthorizationPolicyMiddleware> logger)
        {
            ArgumentNullException.ThrowIfNull(next);
            ArgumentNullException.ThrowIfNull(logger);
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Handles a Middleware request.
        /// </summary>
        /// <param name="context">Http Context for the current request.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            try
            {
                await _next(context).ConfigureAwait(false);
            }
            catch (InvalidOperationException ex) when (!context.Response.HasStarted && ex.Message.Contains("AuthorizationPolicy named", StringComparison.Ordinal))
            {
#pragma warning disable CA1848 // Use the LoggerMessage delegates
                _logger.LogError(ex, "Authorization policy is not configured for the requested endpoint, returning 404");
#pragma warning restore CA1848 // Use the LoggerMessage delegates
                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status404NotFound;
            }
        }
    }
}
