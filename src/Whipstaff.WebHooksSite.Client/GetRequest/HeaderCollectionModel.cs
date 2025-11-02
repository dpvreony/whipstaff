// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Whipstaff.WebHooksSite.Client.GetRequest
{
    /// <summary>
    /// Model for the header collection.
    /// </summary>
    /// <param name="ContentLength">The content length of the request.</param>
    /// <param name="UserAgent">The user agent of the request.</param>
    public sealed record HeaderCollectionModel(
        [property: JsonPropertyName("content-length")] IReadOnlyList<string> ContentLength,
        [property: JsonPropertyName("user-agent")] IReadOnlyList<string> UserAgent);
}
