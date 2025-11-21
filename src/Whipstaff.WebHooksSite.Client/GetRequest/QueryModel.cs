// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Text.Json.Serialization;

namespace Whipstaff.WebHooksSite.Client.GetRequest
{
    /// <summary>
    /// Model for the query.
    /// </summary>
    /// <param name="Action">action for the query.</param>
    public sealed record QueryModel(
        [property: JsonPropertyName("action")] string Action);
}
