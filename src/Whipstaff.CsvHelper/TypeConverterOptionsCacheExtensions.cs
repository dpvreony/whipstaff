// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using CsvHelper.TypeConversion;

namespace Whipstaff.CsvHelper
{
    /// <summary>
    /// Extension methods for <see cref="TypeConverterOptionsCache" />.
    /// </summary>
    public static class TypeConverterOptionsCacheExtensions
    {
        /// <summary>
        /// Adds the ISO 8601 date time converter options to the type converter options cache.
        /// </summary>
        /// <param name="typeConverterOptionsCache">Type convertor cache to modify.</param>
        public static void AddTypeIso8601DateTimeConverterOptions(this TypeConverterOptionsCache typeConverterOptionsCache)
        {
            ArgumentNullException.ThrowIfNull(typeConverterOptionsCache);

            var dateOptions = new TypeConverterOptions
            {
                Formats = [
                    "yyyy-MM-dd"
                ]
            };
            typeConverterOptionsCache.AddOptions<DateOnly>(dateOptions);
            typeConverterOptionsCache.AddOptions<DateOnly?>(dateOptions);

            var dateTimeOptions = new TypeConverterOptions
            {
                Formats = [
                    "O"
                ]
            };
            typeConverterOptionsCache.AddOptions<DateTime>(dateTimeOptions);
            typeConverterOptionsCache.AddOptions<DateTime?>(dateTimeOptions);
        }
    }
}
