// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using CsvHelper;

namespace Whipstaff.CsvHelper
{
    /// <summary>
    /// Extension methods for <see cref="CsvWriter" />.
    /// </summary>
    public static class CsvWriterExtensions
    {
        /// <summary>
        /// Adds the ISO 8601 date time converter options to the type converter options cache on the CSV reader.
        /// </summary>
        /// <param name="csvWriter">CSV writer to modify.</param>
        public static void AddTypeIso8601DateTimeConverterOptions(this CsvWriter csvWriter)
        {
            ArgumentNullException.ThrowIfNull(csvWriter);
            csvWriter.Context.TypeConverterOptionsCache.AddTypeIso8601DateTimeConverterOptions();
        }
    }
}
