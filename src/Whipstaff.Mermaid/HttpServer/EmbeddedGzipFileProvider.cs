// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if TBC
using System.Reflection;
using Microsoft.Extensions.FileProviders;

namespace Whipstaff.Mermaid.HttpServer
{
    /// <summary>
    /// Wrapper for the <see cref="EmbeddedFileProvider"/> that provides Gzip decompression for embedded resources.
    /// </summary>
    public class EmbeddedGzipFileProvider : EmbeddedFileProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedGzipFileProvider" /> class using the specified
        /// assembly with the base namespace defaulting to the assembly name.
        /// </summary>
        /// <param name="assembly">The assembly that contains the embedded resources.</param>
        public EmbeddedGzipFileProvider(Assembly assembly)
            : base(assembly)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedGzipFileProvider" /> class using the specified
        /// assembly and base namespace.
        /// </summary>
        /// <param name="assembly">The assembly that contains the embedded resources.</param>
        /// <param name="baseNamespace">The base namespace that contains the embedded resources.</param>
        public EmbeddedGzipFileProvider(Assembly assembly, string? baseNamespace)
            : base(assembly, baseNamespace)
        {
        }

        /// <summary>
        /// Locates a file at the given path.
        /// </summary>
        /// <param name="subpath">The path that identifies the file. </param>
        /// <returns>
        /// The file information. Caller must check Exists property. A <see cref="NotFoundFileInfo" /> if the file could
        /// not be found.
        /// </returns>
        // ReSharper disable once IdentifierTypo
        public new IFileInfo GetFileInfo(string subpath)
        {
            var baseResponse = base.GetFileInfo(subpath);

            if (!baseResponse.Exists)
            {
                return baseResponse;
            }

            return new EmbeddedResourceGzipFileInfo(baseResponse);
        }
    }
}
#endif
