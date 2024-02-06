// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

#if ARGUMENT_NULL_EXCEPTION_SHIM
using ArgumentNullException = Whipstaff.Runtime.Exceptions.ArgumentNullException;
#endif

namespace Whipstaff.Runtime.Extensions
{
    /// <summary>
    /// Extensions for String manipulation.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Carries out an action if a string is not null or whitespace.
        /// </summary>
        /// <param name="input">The input string to check.</param>
        /// <param name="action">The action to carry out.</param>
        /// <exception cref="ArgumentNullException">No action is provided.</exception>
        public static void ActIfNotNullOrWhiteSpace(this string? input, Action<string> action)
        {
            ArgumentNullException.ThrowIfNull(action);

            if (string.IsNullOrWhiteSpace(input))
            {
                return;
            }

#if NET48
#pragma warning disable CS8604
#endif
            action(input);
#if NET48
#pragma warning restore CS8604
#endif
        }

        /// <summary>
        /// Checks to see if a string is Hexadecimal.
        /// </summary>
        /// <param name="instance">String to check.</param>
        /// <returns>Whether the string is Hexadecimal.</returns>
        public static bool IsHexadecimal(this string instance)
        {
            return instance.All(character => character.IsHexadecimal());
        }

        /// <summary>
        /// Replaces characters in a string using a dictionary.
        /// </summary>
        /// <param name="instance">String to check.</param>
        /// <param name="replacements">Dictionary of replacements to carry out.</param>
        /// <returns>Altered string.</returns>
        public static string Replace(
            this string instance,
            Dictionary<char, char> replacements)
        {
            ArgumentNullException.ThrowIfNull(instance);
            ArgumentNullException.ThrowIfNull(replacements);

            var charArray = instance.ToCharArray();

            for (var i = 0; i < charArray.Length; i++)
            {
                var currentChar = charArray[i];

                if (replacements.TryGetValue(currentChar, out var value))
                {
                    charArray[i] = value;
                }
            }

            return new string(charArray);
        }


#if NETSTANDARD2_1_OR_GREATER
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
            bool ignoreCase = false)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return instance.Replace(
                value,
                string.Empty,
                ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
        }
#endif

        /// <summary>
        /// Converts a string to a memory stream.
        /// </summary>
        /// <param name="instance">String to convert.</param>
        /// <returns>Memory stream.</returns>
        public static MemoryStream ToMemoryStream(this string instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var byteArray = Encoding.Unicode.GetBytes(instance);
            return new MemoryStream(byteArray);
        }

        /// <summary>
        /// Checks whether a string is all ASCII letter or number.
        /// </summary>
        /// <param name="instance">string to check.</param>
        /// <returns>Whether a character is an ASCII letter or number.</returns>
        public static bool IsAsciiLettersOrNumbers(this string instance)
        {
            return instance.All(character => character.IsAsciiLetterOrNumber());
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if a string is null or whitespace.
        /// </summary>
        /// <param name="argument">string to check.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="argument" /> corresponds.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNullOrWhitespace(this string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                throw new ArgumentNullException(paramName);
            }
        }
    }
}
