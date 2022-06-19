// Copyright (c) 2020 DHGMS Solutions and Contributors. All rights reserved.
// DHGMS Solutions and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Globalization;
using NodaTime;

namespace Whipstaff.NodaTime
{
    /// <summary>
    /// Extensions to NodaTime <see cref="LocalDate"/>
    /// </summary>
    public static class LocalDateExtensions
    {
        /// <summary>
        /// Outputs a string in yyyy-MM-dd format.
        /// </summary>
        /// <param name="instance">Local Date instance.</param>
        /// <returns></returns>
        public static string ToYearMonthDayString(this LocalDate instance)
        {
            return instance.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
        }
    }
}
