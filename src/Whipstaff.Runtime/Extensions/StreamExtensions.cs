// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

#if ARGUMENT_NULL_EXCEPTION_SHIM
using ArgumentNullException = Whipstaff.Runtime.Exceptions.ArgumentNullException;
#endif

namespace Whipstaff.Runtime.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Stream"/>.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Converts a stream to a byte array.
        /// </summary>
        /// <param name="stream">Stream to convert.</param>
        /// <returns>Byte array representation of the stream content.</returns>
        public static byte[] ToByteArray(this Stream stream)
        {
            ArgumentNullException.ThrowIfNull(stream);
            if (stream is not MemoryStream memoryStream)
            {
                memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);
            }

            return memoryStream.ToArray();
        }
    }
}
