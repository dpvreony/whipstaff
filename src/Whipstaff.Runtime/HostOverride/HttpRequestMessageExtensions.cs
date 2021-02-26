// Copyright (c) 2020 DHGMS Solutions and Contributors. All rights reserved.
// DHGMS Solutions and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Net.Http;

namespace Whipstaff.Runtime.HostOverride
{
    /// <summary>
    /// Extensions for HttpRequestMessage and host name overrides.
    /// </summary>
    public static class HttpRequestMessageExtensions
    {
        /// <summary>
        /// Checks to see if the host on a http request message should be overriden.
        /// </summary>
        /// <param name="instance">The Http Request Message.</param>
        /// <param name="hostOverride">The Host Override implementation to check.</param>
        public static void CheckForHostnameOverride(
            this HttpRequestMessage instance,
            IHostOverride hostOverride)
        {
            var requestUri = instance.RequestUri;
            var host = requestUri.Host;
            var target = hostOverride.Resolve(host) ?? host;

            var builder = new UriBuilder(requestUri)
            {
                Host = target,
            };

            instance.RequestUri = builder.Uri;
        }
    }
}
