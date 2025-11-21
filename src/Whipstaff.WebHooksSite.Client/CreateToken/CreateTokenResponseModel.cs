// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Text.Json.Serialization;

namespace Whipstaff.WebHooksSite.Client.CreateToken
{
    /// <summary>
    /// Response to a create token request.
    /// </summary>
    public sealed class CreateTokenResponseModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether to redirect webhook requests. See https://docs.webhook.site/api/tokens.html for more information.
        /// </summary>
        [JsonPropertyName("redirect")]
        public bool Redirect { get; set; }

        /// <summary>
        /// Gets or sets the alias for the session.
        /// </summary>
        [JsonPropertyName("alias")]
        public required string Alias { get; set; }

        /// <summary>
        /// Gets or sets the timeout applied to requests in the session.
        /// </summary>
        [JsonPropertyName("timeout")]
        public int Timeout { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the session is on a premium account.
        /// </summary>
        [JsonPropertyName("premium")]
        public bool Premium { get; set; }

        /// <summary>
        /// Gets or sets the Unique Identifier for the session.
        /// </summary>
        [JsonPropertyName("uuid")]
        public Guid Uuid { get; set; }

        /// <summary>
        /// Gets or sets the IP address that created the session.
        /// </summary>
        [JsonPropertyName("ip")]
        public required string Ip { get; set; }

        /// <summary>
        /// Gets or sets the User Agent that created the session.
        /// </summary>
        [JsonPropertyName("user_agent")]
        public required string UserAgent { get; set; }

        /// <summary>
        /// Gets or sets the default content for webhook responses.
        /// </summary>
        [JsonPropertyName("default_content")]
        public required string DefaultContent { get; set; }

        /// <summary>
        /// Gets or sets the default HTTP status code for webhook responses.
        /// </summary>
        [JsonPropertyName("default_status")]
        public int DefaultStatus { get; set; }

        /// <summary>
        /// Gets or sets the default content type for webhook responses.
        /// </summary>
        [JsonPropertyName("default_content_type")]
        public required string DefaultContentType { get; set; }

        /// <summary>
        /// Gets or sets the expiry timestamp if the session is on a premium account.
        /// </summary>
        [JsonPropertyName("premium_expires_at")]
        public DateTimeOffset? PremiumExpiresAt { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of when the session was created.
        /// </summary>
        [JsonPropertyName("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of when the session was last updated.
        /// </summary>
        [JsonPropertyName("updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
