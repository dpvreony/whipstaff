using System;
using System.Collections.Generic;
using System.Text;
using Dhgms.AspNetCoreContrib.App.Features.RequireForwardedForHeader;
using Microsoft.AspNetCore.Builder;

namespace Dhgms.AspNetCoreContrib.App.Features.AddServerDetailsToResponseHeader
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
