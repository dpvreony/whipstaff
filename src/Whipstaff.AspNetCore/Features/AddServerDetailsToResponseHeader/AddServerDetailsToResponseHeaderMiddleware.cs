// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Whipstaff.AspNetCore.Features.AddServerDetailsToResponseHeader
{
    /// <summary>
    /// Adds server details to the response headers of a request. Can be used
    /// to aid supporting diagnostics, support and testing on a client.
    ///
    /// There are 2 constructors, 1 uses the machine name, the other allows
    /// you to replace the hostname returned so it can be a token of some
    /// form if you don't want to expose the actual machine name.
    /// </summary>
    public sealed class AddServerDetailsToResponseHeaderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _hostName;
        private readonly string _appVersion;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddServerDetailsToResponseHeaderMiddleware"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor sets the request header to use <see cref="System.Environment.MachineName"/>.
        /// </remarks>
        /// <param name="next">The next middleware registration in the pipeline.</param>
        /// <param name="appVersion">The application version to place in the response header.</param>
#pragma warning disable GR0027 // Constructor should have a logging framework instance as the final parameter.
        public AddServerDetailsToResponseHeaderMiddleware(
            RequestDelegate next,
            string appVersion)
            : this(
                next,
                Environment.MachineName,
                appVersion)
        {
        }
#pragma warning restore GR0027 // Constructor should have a logging framework instance as the final parameter.

        /// <summary>
        /// Initializes a new instance of the <see cref="AddServerDetailsToResponseHeaderMiddleware"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor allows you to set a hostname or equivalent token such as a guid. This allows
        /// you to use an arbitary value and\or hide the hostname from the client.
        /// </remarks>
        /// <param name="next">The next middleware registration in the pipeline.</param>
        /// <param name="hostname">The hostname or respective token to place in the response header.</param>
        /// <param name="appVersion">The application version to place in the response header.</param>
#pragma warning disable GR0027 // Constructor should have a logging framework instance as the final parameter.
        public AddServerDetailsToResponseHeaderMiddleware(
            RequestDelegate next,
            string hostname,
            string appVersion)
        {
            ArgumentNullException.ThrowIfNull(next);
            _next = next;
            _hostName = hostname;
            _appVersion = appVersion;
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

            var response = context.Response;

            var headers = response.Headers;
            headers.Append("X-HOSTNAME", _hostName);
            headers.Append("X-APPVERSION", _appVersion);

            await _next.Invoke(context);
        }
    }
}
