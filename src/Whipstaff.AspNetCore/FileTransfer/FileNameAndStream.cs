// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO;

namespace Whipstaff.AspNetCore.FileTransfer
{
    /// <summary>
    /// POCO object representing a file name and the respective stream.
    /// </summary>
    public sealed class FileNameAndStream
    {
        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the file stream.
        /// </summary>
        public Stream FileStream { get; set; }
    }
}
