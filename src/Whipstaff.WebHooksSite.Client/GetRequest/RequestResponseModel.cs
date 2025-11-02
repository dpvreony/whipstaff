// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Whipstaff.WebHooksSite.Client.GetRequest
{
#pragma warning disable CA1054
    /// <summary>
    /// Model for the response.
    /// </summary>
    /// <param name="Uuid">Unique id of the response.</param>
    /// <param name="TokenId">Token id associated with the request.</param>
    /// <param name="Ip">Source IP for the request.</param>
    /// <param name="Hostname">Hostname the request was targeting.</param>
    /// <param name="Method">Http method used on the request.</param>
    /// <param name="UserAgent">User agent.</param>
    /// <param name="Content">Content of the request, if any.</param>
    /// <param name="Query">Querystring of the request.</param>
    /// <param name="Request">Request payload model.</param>
    /// <param name="Files">File collection model.</param>
    /// <param name="Headers">HTTP header payload.</param>
    /// <param name="Url">Url for the request.</param>
    /// <param name="CreatedAt">Timestamp for when the request was created.</param>
    /// <param name="UpdatedAt">Timestamp for when the request was updated.</param>
    /// <param name="CustomActionOutput">collection of custom action outputs.</param>
    public sealed record RequestResponseModel(
        [property: JsonPropertyName("uuid")] string Uuid,
        [property: JsonPropertyName("token_id")] string TokenId,
        [property: JsonPropertyName("ip")] string Ip,
        [property: JsonPropertyName("hostname")] string Hostname,
        [property: JsonPropertyName("method")] string Method,
        [property: JsonPropertyName("user_agent")] string UserAgent,
        [property: JsonPropertyName("content")] string Content,
        [property: JsonPropertyName("query")] QueryModel Query,
        [property: JsonPropertyName("request")] RequestPayloadModel Request,
        [property: JsonPropertyName("files")] FileCollectionModel Files,
        [property: JsonPropertyName("headers")] HeaderCollectionModel Headers,
        [property: JsonPropertyName("url")] string Url,
        [property: JsonPropertyName("created_at")] string CreatedAt,
        [property: JsonPropertyName("updated_at")] string UpdatedAt,
        [property: JsonPropertyName("custom_action_output")] IReadOnlyList<object> CustomActionOutput);
#pragma warning restore CA1054
}
