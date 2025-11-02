// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Text.Json.Serialization;

namespace Whipstaff.WebHooksSite.Client.GetRequest
{
    /// <summary>
    /// Model for the file.
    /// </summary>
    /// <param name="Id">Unique id of the file.</param>
    /// <param name="Filename">Name of the file.</param>
    /// <param name="Size">Size of the file.</param>
    /// <param name="ContentType">Mime content type of the file.</param>
    public sealed record FileModel(
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("filename")] string Filename,
        [property: JsonPropertyName("size")] int Size,
        [property: JsonPropertyName("content_type")] string ContentType);
}
