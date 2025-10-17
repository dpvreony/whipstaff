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
        /// Deserializes an object from a stream by using Data Contracts.
        /// </summary>
        /// <typeparam name="TResult">The target type for deserialization.</typeparam>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>The deserialized object.</returns>
        public static Task<TResult?> DeserializeFromJsonViaDataContractAsync<TResult>(this Stream stream)
        {
            return Task.FromResult(DeserializeFromJsonViaDataContract<TResult>(stream));
        }

        /// <summary>
        /// Deserializes an object from a stream by using Data Contracts. This version is for use with a cached <see cref="DataContractJsonSerializer"/>.
        /// </summary>
        /// <typeparam name="TResult">The target type for deserialization.</typeparam>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="dataContractJsonSerializer">The Json Data Contract Serializer to use.</param>
        /// <returns>The deserialized object.</returns>
        public static Task<TResult?> DeserializeFromJsonViaDataContractAsync<TResult>(this Stream stream, DataContractJsonSerializer dataContractJsonSerializer)
        {
            return Task.FromResult(DeserializeFromJsonViaDataContract<TResult>(stream, dataContractJsonSerializer));
        }

        /// <summary>
        /// Deserializes an object from a stream by using Data Contracts.
        /// </summary>
        /// <typeparam name="TResult">The target type for deserialization.</typeparam>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>The deserialized object.</returns>
        public static TResult? DeserializeFromJsonViaDataContract<TResult>(this Stream stream)
        {
            var serializer = new DataContractJsonSerializer(typeof(TResult));
            return (TResult?)serializer.ReadObject(stream);
        }

        /// <summary>
        /// Deserializes an object from a stream by using Data Contracts. This version is for use with a cached <see cref="DataContractJsonSerializer"/>.
        /// </summary>
        /// <typeparam name="TResult">The target type for deserialization.</typeparam>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="dataContractJsonSerializer">The Json Data Contract Serializer to use.</param>
        /// <returns>The deserialized object.</returns>
        public static TResult? DeserializeFromJsonViaDataContract<TResult>(this Stream stream, DataContractJsonSerializer dataContractJsonSerializer)
        {
            ArgumentNullException.ThrowIfNull(dataContractJsonSerializer);

#pragma warning disable GR0007
            var serializerType = dataContractJsonSerializer.GetType();
#pragma warning restore GR0007
            if (serializerType is not TResult)
            {
                throw new ArgumentException($"Type on serializer is incorrect. Expected: {typeof(TResult)}, Got: {serializerType}", nameof(dataContractJsonSerializer));
            }

            return (TResult?)dataContractJsonSerializer.ReadObject(stream);
        }

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
