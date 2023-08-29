// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO;

namespace Whipstaff.AspNetCore.FileTransfer
{
    /// <summary>
    /// POCO object representing a file name and the respective stream.
    /// While FileStream has both properties, this is aimed at In Memory
    /// File Manipulation.
    /// </summary>
    public sealed class FileNameAndStreamModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileNameAndStreamModel"/> class.
        /// </summary>
        /// <param name="fileName">The filename to associate with the stream.</param>
        /// <param name="fileStream">The stream of file content.</param>
        public FileNameAndStreamModel(
            string fileName,
            Stream fileStream)
        {
            ArgumentNullException.ThrowIfNull(fileName);
            ArgumentNullException.ThrowIfNull(fileStream);

            FileName = fileName;
            FileStream = fileStream;
        }

        /// <summary>
        /// Gets the file name.
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Gets the file stream.
        /// </summary>
        public Stream FileStream { get; }
    }
}
