// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Whipstaff.Runtime.Extensions;

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

            var content = httpResponseMessage.Content;
            var contentHeaders = httpResponseMessage.Content.Headers;
            var contentEncoding = contentHeaders.ContentEncoding;
            var bodyBytes = await GetContent(contentEncoding, content)
                .ConfigureAwait(false);

            var routeFulfillOptions = new RouteFulfillOptions
            {
                Status = (int)httpResponseMessage.StatusCode,
                BodyBytes = bodyBytes
            };

            var contentType = contentHeaders.ContentType;
            if (contentType != null)
            {
                routeFulfillOptions.ContentType = contentType.ToString();
            }

            return routeFulfillOptions;
        }

        private static async Task<byte[]> GetContent(ICollection<string> contentEncoding, HttpContent content)
        {
            if (contentEncoding.Count == 1 && contentEncoding.First().Equals("gzip", StringComparison.OrdinalIgnoreCase))
            {
                var sourceStream = await content.ReadAsStreamAsync().ConfigureAwait(false);
                using (var gzipStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                {
                    return gzipStream.ToByteArray();
                }
            }

            return await content.ReadAsByteArrayAsync().ConfigureAwait(false);
        }
    }
}
