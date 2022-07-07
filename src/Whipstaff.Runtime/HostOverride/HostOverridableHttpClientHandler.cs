// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Whipstaff.Runtime.HostOverride
{
    /// <summary>
    /// Http Client Handler that allows host overriding.
    /// </summary>
    /// <remarks>
    /// Based upon: https://stackoverflow.com/questions/58547451/is-it-possible-to-set-custom-dns-resolver-in-cs-httpclient
    /// Split out logic to make it re-usable in different scenario's without having to re-implement this class.
    /// </remarks>
    public class HostOverridableHttpClientHandler : HttpClientHandler
    {
        private readonly IHostOverride _hostOverride;

        /// <summary>
        /// Initializes a new instance of the <see cref="HostOverridableHttpClientHandler"/> class.
        /// </summary>
        /// <param name="hostOverride">Implementation of a host override helper.</param>
        public HostOverridableHttpClientHandler(IHostOverride hostOverride)
        {
            _hostOverride = hostOverride ?? throw new ArgumentNullException(nameof(hostOverride));
        }

        /// <inheritdoc/>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.CheckForHostnameOverride(_hostOverride);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
