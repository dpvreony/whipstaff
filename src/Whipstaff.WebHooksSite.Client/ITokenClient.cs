// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Refit;
using Whipstaff.WebHooksSite.Client.CreateToken;

namespace Whipstaff.WebHooksSite.Client
{
    /// <summary>
    /// Represents the webhooks.site Token calls.
    /// </summary>
    public interface ITokenClient
    {
        /// <summary>
        /// This is the API route path for the token calls.
        /// </summary>
        private const string TokenPath = "/token";

        /// <summary>
        /// Creates a token.
        /// </summary>
        /// <returns>Model representing the token and session details.</returns>
        [Post(TokenPath)]
        Task<CreateTokenResponseModel> CreateAsync();

        /// <summary>
        /// Deletes a token.
        /// </summary>
        /// <param name="uuid">Unique ID of the token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Delete(TokenPath)]
        Task DeleteAsync([Query(Format = "D")] Guid uuid);
    }
}
