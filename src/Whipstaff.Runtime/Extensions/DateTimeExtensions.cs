// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Whipstaff.Runtime.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToRfc3339String(this DateTime dateTime)
        {
            // https://stackoverflow.com/questions/17017/how-do-i-parse-and-convert-a-datetime-to-the-rfc-3339-date-time-format
            return dateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss") + "Z";
        }
    }
}
