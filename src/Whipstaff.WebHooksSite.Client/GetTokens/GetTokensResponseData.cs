// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Text.Json.Serialization;

#if TBC
namespace Dhgms.WebHooksSite.Client.GetTokens
{
    /// <summary>
    /// Nested data element for the create token response.
    /// </summary>
    public class GetTokensResponseData
    {
        [JsonPropertyName("uuid")]
        public string Uuid { get; set; }

        [JsonPropertyName("redirect")]
        public bool Redirect { get; set; }

        [JsonPropertyName("alias")]
        public string Alias { get; set; }

        [JsonPropertyName("actions")]
        public bool Actions { get; set; }

        [JsonPropertyName("cors")]
        public bool Cors { get; set; }

        [JsonPropertyName("expiry")]
        public bool Expiry { get; set; }

        [JsonPropertyName("timeout")]
        public int Timeout { get; set; }

        [JsonPropertyName("premium")]
        public bool Premium { get; set; }

        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("password")]
        public bool Password { get; set; }

        [JsonPropertyName("ip")]
        public string Ip { get; set; }

        [JsonPropertyName("user_agent")]
        public string UserAgent { get; set; }

        [JsonPropertyName("default_content")]
        public string DefaultContent { get; set; }

        [JsonPropertyName("default_status")]
        public int DefaultStatus { get; set; }

        [JsonPropertyName("default_content_type")]
        public string DefaultContentType { get; set; }

        [JsonPropertyName("premium_expires_at")]
        public DateTimeOffset PremiumExpiresAt { get; set; }

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonPropertyName("require_auth")]
        public bool RequireAuth { get; set; }

        [JsonPropertyName("latest_request_id")]
        public string LatestRequestId { get; set; }

        [JsonPropertyName("latest_request_at")]
        public string LatestRequestAt { get; set; }

        [JsonPropertyName("category_id")]
        public string CategoryId { get; set; }

        [JsonPropertyName("requests")]
        public int Requests { get; set; }
    }
}
#endif
