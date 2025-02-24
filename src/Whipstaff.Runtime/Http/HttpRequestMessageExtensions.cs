// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Net.Http;

#if ARGUMENT_NULL_EXCEPTION_SHIM
using ArgumentNullException = Whipstaff.Runtime.Exceptions.ArgumentNullException;
#endif

namespace Whipstaff.Runtime.Http
{
    /// <summary>
    /// Extension methods for <see cref="HttpRequestMessage"/>.
    /// </summary>
    public static class HttpRequestMessageExtensions
    {
        /// <summary>
        /// Adds a dictionary of single value headers.
        /// </summary>
        /// <param name="httpRequestMessage">Http Request Message to add headers to.</param>
        /// <param name="requestHeaders">Request headers to add.</param>
        public static void AddHeaders(
            this HttpRequestMessage httpRequestMessage,
            IDictionary<string, string> requestHeaders)
        {
            ArgumentNullException.ThrowIfNull(httpRequestMessage);
            ArgumentNullException.ThrowIfNull(requestHeaders);

            var targetHeaders = httpRequestMessage.Headers;

            foreach (var requestHeader in requestHeaders)
            {
                targetHeaders.Add(requestHeader.Key, requestHeader.Value);
            }
        }

        /// <summary>
        /// Adds a dictionary of headers that can contain one of more values..
        /// </summary>
        /// <param name="httpRequestMessage">Http Request Message to add headers to.</param>
        /// <param name="requestHeaders">Request headers to add.</param>
        public static void AddHeaders(
            this HttpRequestMessage httpRequestMessage,
            IDictionary<string, IEnumerable<string>> requestHeaders)
        {
            ArgumentNullException.ThrowIfNull(httpRequestMessage);
            ArgumentNullException.ThrowIfNull(requestHeaders);

            var targetHeaders = httpRequestMessage.Headers;

            foreach (var requestHeader in requestHeaders)
            {
                targetHeaders.Add(requestHeader.Key, requestHeader.Value);
            }
        }
    }
}
