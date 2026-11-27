// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Net.Http;
using Refit;

namespace Whipstaff.WebHooksSite.Client
{
    /// <summary>
    /// Factory for Refit clients.
    /// </summary>
    public static class ClientFactory
    {
#pragma warning disable CA1054
        /// <summary>
        /// Generate a Refit implementation of the specified interface.
        /// </summary>
        /// <param name="client">The <see cref="HttpClient"/> the implementation will use to send requests.</param>
        /// <param name="settings"><see cref="RefitSettings"/> to use to configure the HttpClient.</param>
        /// <returns>An instance of token client.</returns>
        public static ITokenClient CreateTokenClient(HttpClient client, RefitSettings settings)
        {
            return Refit.RestService.ForGenerated<ITokenClient>(client, settings);
        }

        /// <summary>
        /// Generate a Refit implementation of the specified interface.
        /// </summary>
        /// <param name="client">The <see cref="HttpClient"/> the implementation will use to send requests.</param>
        /// <returns>An instance of token client.</returns>
        public static ITokenClient CreateTokenClient(HttpClient client)
        {
            return Refit.RestService.ForGenerated<ITokenClient>(client);
        }

        /// <summary>
        /// Generate a Refit implementation of the specified interface.
        /// </summary>
        /// <param name="hostUrl">Base address the implementation will use.</param>
        /// <param name="settings"><see cref="RefitSettings"/> to use to configure the HttpClient.</param>
        /// <returns>An instance of token client.</returns>
        public static ITokenClient CreateTokenClient(string hostUrl, RefitSettings settings)
        {
            return Refit.RestService.ForGenerated<ITokenClient>(hostUrl, settings);
        }

        /// <summary>
        /// Generate a Refit implementation of the specified interface.
        /// </summary>
        /// <param name="hostUrl">Base address the implementation will use.</param>
        /// <returns>An instance of token client.</returns>
        public static ITokenClient CreateTokenClient(string hostUrl)
        {
            return Refit.RestService.ForGenerated<ITokenClient>(hostUrl);
        }

        /// <summary>
        /// Generate a Refit implementation of the specified interface.
        /// </summary>
        /// <param name="client">The <see cref="HttpClient"/> the implementation will use to send requests.</param>
        /// <param name="settings"><see cref="RefitSettings"/> to use to configure the HttpClient.</param>
        /// <returns>An instance of token client.</returns>
        public static IRequestClient CreateRequestClient(HttpClient client, RefitSettings settings)
        {
            return Refit.RestService.ForGenerated<IRequestClient>(client, settings);
        }

        /// <summary>
        /// Generate a Refit implementation of the specified interface.
        /// </summary>
        /// <param name="client">The <see cref="HttpClient"/> the implementation will use to send requests.</param>
        /// <returns>An instance of token client.</returns>
        public static IRequestClient CreateRequestClient(HttpClient client)
        {
            return Refit.RestService.ForGenerated<IRequestClient>(client);
        }

        /// <summary>
        /// Generate a Refit implementation of the specified interface.
        /// </summary>
        /// <param name="hostUrl">Base address the implementation will use.</param>
        /// <param name="settings"><see cref="RefitSettings"/> to use to configure the HttpClient.</param>
        /// <returns>An instance of token client.</returns>
        public static IRequestClient CreateRequestClient(string hostUrl, RefitSettings settings)
        {
            return Refit.RestService.ForGenerated<IRequestClient>(hostUrl, settings);
        }

        /// <summary>
        /// Generate a Refit implementation of the specified interface.
        /// </summary>
        /// <param name="hostUrl">Base address the implementation will use.</param>
        /// <returns>An instance of token client.</returns>
        public static IRequestClient CreateRequestClient(string hostUrl)
        {
            return Refit.RestService.ForGenerated<IRequestClient>(hostUrl);
        }
    }
#pragma warning restore CA1054
}
