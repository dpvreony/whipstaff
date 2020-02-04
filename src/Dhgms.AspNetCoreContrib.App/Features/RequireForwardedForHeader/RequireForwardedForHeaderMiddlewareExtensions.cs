// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;

namespace Dhgms.AspNetCoreContrib.App.Features.RequireForwardedForHeader
{
    /// <summary>
    /// Extension methods for the Require Forwarded Header Middleware.
    /// </summary>
    public static class RequireForwardedForHeaderMiddlewareExtensions
    {
        /// <summary>
        /// Registered the Require Forwarded Header Middleware onto the application builder.
        /// </summary>
        /// <param name="builder">The instance of the application builder to register the middleware on.</param>
        /// <returns>The application builder, to allow fluent registrations.</returns>
        public static IApplicationBuilder UseRequireForwardedForHeader(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequireForwardedForHeaderMiddleware>();
        }
    }
}
