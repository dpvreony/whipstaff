// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Text.Json.Serialization;

namespace Whipstaff.WebHooksSite.Client.GetRequest
{
    /// <summary>
    /// Model for the file collection.
    /// </summary>
    /// <param name="File">Individual File Model.</param>
    public sealed record FileCollectionModel(
        [property: JsonPropertyName("file")] FileModel File);
}
