// Copyright (c) 2020 DHGMS Solutions and Contributors. All rights reserved.
// DHGMS Solutions and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Dhgms.NetContrib.UnitTests.Features.Extensions
{
    /// <summary>
    /// Extensions for String manipulation.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Removes all instances of a string.
        /// </summary>
        /// <param name="instance">The string to work on.</param>
        /// <param name="value">The string to remove.</param>
        /// <param name="ignoreCase">Whether to ignore the case, or be case-sensitive.</param>
        /// <returns>Altered string.</returns>
        public static string Remove(
            this string instance,
            string value,
            bool ignoreCase = true)
        {
            return instance.Replace(
                value,
                string.Empty,
                ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
        }
    }
}
