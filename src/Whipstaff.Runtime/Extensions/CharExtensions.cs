// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Runtime.Extensions
{
    /// <summary>
    /// Extension methods for working with <see cref="char"/>.
    /// </summary>
    public static class CharExtensions
    {
        /// <summary>
        /// Checks to see if a character is Hexadecimal.
        /// </summary>
        /// <param name="instance">Character to check.</param>
        /// <returns>Whether the string is Hexadecimal.</returns>
        public static bool IsHexadecimal(this char instance)
        {
            return instance is >= '0' and <= '9' or >= 'a' and <= 'f' or >= 'A' and <= 'F';
        }

        /// <summary>
        /// Checks whether a character is an ASCII letter or number.
        /// </summary>
        /// <param name="instance">character to check.</param>
        /// <returns>Whether a character is an ASCII letter or number.</returns>
        public static bool IsAsciiLetterOrNumber(this char instance)
        {
            return instance is >= '0' and <= '9' or >= 'a' and <= 'z' or >= 'A' and <= 'Z';
        }
    }
}
