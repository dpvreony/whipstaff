// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace Whipstaff.Runtime.Extensions
{
    public static class StreamExtensions
    {
        public static async Task<TResult> GetPocoResponseFromJsonViaDataContractAsync<TResult>(this Stream stream)
        {
            return await Task.FromResult(GetPocoResponseFromJsonViaDataContract<TResult>(stream)).ConfigureAwait(false);
        }

        public static async Task<TResult> GetPocoResponseFromJsonViaDataContractAsync<TResult>(this Stream stream, DataContractJsonSerializer dataContractJsonSerializer)
        {
            return await Task.FromResult(GetPocoResponseFromJsonViaDataContract<TResult>(stream, dataContractJsonSerializer)).ConfigureAwait(false);
        }

        public static TResult GetPocoResponseFromJsonViaDataContract<TResult>(this Stream stream)
        {
            var serializer = new DataContractJsonSerializer(typeof(TResult));
            return (TResult)serializer.ReadObject(stream);
        }

        public static TResult GetPocoResponseFromJsonViaDataContract<TResult>(this Stream stream, DataContractJsonSerializer dataContractJsonSerializer)
        {
            if (dataContractJsonSerializer == null)
            {
                throw new ArgumentNullException(nameof(dataContractJsonSerializer));
            }

            var serializerType = dataContractJsonSerializer.GetType();
            if (serializerType == typeof(TResult))
            {
                throw new ArgumentException($"Type on serializer is incorrect. Expected: {typeof(TResult)}, Got: {serializerType}", nameof(dataContractJsonSerializer));
            }

            return (TResult)dataContractJsonSerializer.ReadObject(stream);
        }
    }
}
