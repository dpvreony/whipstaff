﻿// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Builder;

namespace Whipstaff.AspNetCore.Features.AddServerDetailsToResponseHeader
{
    /// <summary>
    /// Extension methods for the Add Server Details To Response Header Middleware.
    /// </summary>
    public static class AddServerDetailsToResponseHeaderMiddlewareExtensions
    {
        /// <summary>
        /// Registered the Add Server Details To Response Header Middleware onto the application builder.
        /// </summary>
        /// <param name="builder">The instance of the application builder to register the middleware on.</param>
        /// <returns>The application builder, to allow fluent registrations.</returns>
        public static IApplicationBuilder UseAddServerDetailsToResponseHeaderMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AddServerDetailsToResponseHeaderMiddleware>();
        }
    }
}
