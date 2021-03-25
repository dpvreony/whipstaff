// Copyright (c) 2020 DHGMS Solutions and Contributors. All rights reserved.
// DHGMS Solutions and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.StaticFiles;

namespace Whipstaff.AspNetCore.ContentTypes
{
    /// <summary>
    /// Extensions for Content Types.
    /// </summary>
    public static class ContentTypeExtensions
    {
        /// <summary>
        /// Gets the file extension for a content type.
        /// </summary>
        /// <param name="instance">Instance of the content type.</param>
        /// <returns>File extension.</returns>
        public static string GetFileExtension(this ContentType instance)
        {
            return GetFileExtension(
                instance,
                new FileExtensionContentTypeProvider());
        }

        /// <summary>
        /// Gets the file extension for a content type.
        /// </summary>
        /// <param name="instance">Instance of the content type.</param>
        /// <param name="fileExtensionContentTypeProvider">Instance of the File Extension Content Type Provider. This override is here to allow caching the provider to reduce re-init overhead.</param>
        /// <returns>File extension.</returns>
        public static string GetFileExtension(
            this ContentType instance,
            FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            var mediaType = instance.MediaType;

            var mapping = fileExtensionContentTypeProvider.Mappings
                .Where(x => x.Value.Equals(mediaType, StringComparison.Ordinal))
                .Select(s => s.Key)
                .FirstOrDefault();

            if (mapping != null)
            {
                return mapping;
            }

            throw new ArgumentException($"Unable to map file extension for Media Type: {instance.MediaType}");
        }
    }
}
