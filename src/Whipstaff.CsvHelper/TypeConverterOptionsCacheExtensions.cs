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
        /// <param name="cache">Type convertor cache to modify.</param>
        public static void AddTypeIso8601DateTimeConverterOptions(this TypeConverterOptionsCache cache)
        {
            ArgumentNullException.ThrowIfNull(cache);

            var dateOptions = new TypeConverterOptions
            {
                Formats = [
                    "yyyy-MM-dd"
                ]
            };
            cache.AddOptions<DateOnly>(dateOptions);
            cache.AddOptions<DateOnly?>(dateOptions);

            var dateTimeOptions = new TypeConverterOptions
            {
                Formats = [
                    "O"
                ]
            };
            cache.AddOptions<DateTime>(dateTimeOptions);
            cache.AddOptions<DateTime?>(dateTimeOptions);
        }
    }
}
