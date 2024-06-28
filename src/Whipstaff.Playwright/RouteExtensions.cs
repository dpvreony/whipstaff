// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Net.Http;
using Microsoft.Playwright;

namespace Whipstaff.Playwright
{
    /// <summary>
    /// Extension methods for <see cref="IRoute"/>.
    /// </summary>
    public static class RouteExtensions
    {
        /// <summary>
        /// Converts a <see cref="IRoute"/> to a <see cref="HttpRequestMessage"/>.
        /// </summary>
        /// <param name="route">Route to convert.</param>
        /// <returns>A <see cref="HttpRequestMessage"/> representing the route.</returns>
        public static HttpRequestMessage ToHttpRequestMessage(this IRoute route)
        {
            ArgumentNullException.ThrowIfNull(route);

            var request = route.Request;

            return request.ToHttpRequestMessage();
        }
    }
}
