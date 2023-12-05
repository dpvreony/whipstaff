// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Whipstaff.Playwright
{
    /// <summary>
    /// Factory methods for <see cref="RouteFulfillOptions"/>.
    /// </summary>
    public static class RouteFulfillOptionsFactory
    {
        /// <summary>
        /// Creates a <see cref="RouteFulfillOptions"/> from a <see cref="HttpResponseMessage"/>. Can be used when carrying out manual HTTP client work inside Playwright.
        /// </summary>
        /// <param name="httpResponseMessage">HTTP response message to process.</param>
        /// <returns>Route Fulfill Options to return to Playwright.</returns>
        public static async Task<RouteFulfillOptions> FromHttpResponseMessageAsync(HttpResponseMessage httpResponseMessage)
        {
            ArgumentNullException.ThrowIfNull(httpResponseMessage);

            var routeFulfillOptions = new RouteFulfillOptions
            {
                Status = (int)httpResponseMessage.StatusCode,
                BodyBytes = await httpResponseMessage.Content.ReadAsByteArrayAsync().ConfigureAwait(false)
            };

            if (httpResponseMessage.Content.Headers.ContentType != null)
            {
                routeFulfillOptions.ContentType = httpResponseMessage.Content.Headers.ContentType.ToString();
            }

            return routeFulfillOptions;
        }
    }
}
