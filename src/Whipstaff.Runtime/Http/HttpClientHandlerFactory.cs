// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Whipstaff.Runtime.Cryptography.X509;

#if ARGUMENT_NULL_EXCEPTION_SHIM
using ArgumentNullException = Whipstaff.Runtime.Exceptions.ArgumentNullException;
#endif

namespace Whipstaff.Runtime.Http
{
    /// <summary>
    /// Factory for creating <see cref="HttpClientHandler"/> instances.
    /// </summary>
    public static class HttpClientHandlerFactory
    {
        /// <summary>
        /// Creates a Http Client Handler with a client certificate attached.
        /// </summary>
        /// <param name="certificate">Certificate to bind as client certificate.</param>
        /// <returns>&lt;see cref="HttpClientHandler"/&gt; instance.</returns>
        public static HttpClientHandler GetHttpClientHandlerWithClientCertificate(X509Certificate2 certificate)
        {
            ArgumentNullException.ThrowIfNull(certificate);

            certificate.EnsurePrivateKey();

            var clientHandler = new HttpClientHandler();
            _ = clientHandler.ClientCertificates.Add(certificate);

            return clientHandler;
        }
    }
}
