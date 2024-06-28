// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if TBC
using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using Microsoft.Extensions.FileProviders;

namespace Whipstaff.Mermaid.HttpServer
{
    /// <summary>
    /// Represents a file in the embedded resources that is compressed with Gzip.
    /// </summary>
    public sealed class EmbeddedResourceGzipFileInfo : IFileInfo
    {
        private readonly IFileInfo _baseResponse;
        private long? _length;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedResourceGzipFileInfo"/> class.
        /// </summary>
        /// <param name="baseResponse">The compressed response to handle.</param>
        public EmbeddedResourceGzipFileInfo(IFileInfo baseResponse)
        {
            _baseResponse = baseResponse;
        }

        /// <inheritdoc />
        public bool Exists => _baseResponse.Exists;

        /// <inheritdoc />
        public bool IsDirectory => _baseResponse.IsDirectory;

        /// <inheritdoc />
        public DateTimeOffset LastModified => _baseResponse.LastModified;

        /// <inheritdoc />
        public long Length
        {
            get
            {
                if (!_length.HasValue)
                {
                    using var stream = CreateReadStream();
                    _length = stream.Length;
                }

                return _length.Value;
            }
        }

        /// <inheritdoc />
        public string Name => _baseResponse.Name;

        /// <inheritdoc />
        public string? PhysicalPath => _baseResponse.PhysicalPath;

        /// <inheritdoc />
        public Stream CreateReadStream()
        {
            var stream = _baseResponse.CreateReadStream();
            var gZipStream = new GZipStream(stream, CompressionMode.Decompress);
            return gZipStream;
        }
    }
}
#endif
