// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Whipstaff.Runtime.StringCleansing;

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

#pragma warning disable CS8604
            action(input);
#pragma warning restore CS8604
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

        /// <summary>
        /// Replaces characters in a string using a dictionary.
        /// </summary>
        /// <param name="instance">String to check.</param>
        /// <param name="replacements">Dictionary of replacements to carry out.</param>
        /// <returns>Altered string.</returns>
        public static string Replace(
            this string instance,
            Dictionary<char, string> replacements)
        {
            ArgumentNullException.ThrowIfNull(instance);
            ArgumentNullException.ThrowIfNull(replacements);

            var linkedList = new LinkedList<char>(instance);
            var currentNode = linkedList.First;
            while (currentNode != null)
            {
                var currentChar = currentNode.Value;

                if (replacements.TryGetValue(currentChar, out var value))
                {
                    if (value == null)
                    {
                        // remove the character
                        continue;
                    }

                    switch (value.Length)
                    {
                        case 0:
                            // remove the character
                            var oldNode = currentNode;
                            currentNode = currentNode.Next;
                            linkedList.Remove(oldNode);
                            continue;
                        case 1:
                            currentNode.Value = value[0];
                            break;
                        default:
                            currentNode.Value = value[0];

                            for (int i = 1; i < value.Length; i++)
                            {
                                currentNode = linkedList.AddAfter(currentNode, value[i]);
                            }

                            break;
                    }
                }

                currentNode = currentNode.Next;
            }

            var stringBuilder = new StringBuilder();
            var linkedListEnumerable = linkedList.Select(c => c).ToArray();
            _ = stringBuilder.Append(linkedListEnumerable);

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Replaces Microsoft Word smart quotes with ASCII equivalents.
        /// </summary>
        /// <param name="instance">The string to work on.</param>
        /// <returns>Altered string.</returns>
        public static string ReplaceMsWordSmartQuotesWithAscii(this string instance)
        {
            var replacements = StringCleansingDictionaryFactories.GetMicrosoftWordSmartQuoteReplacements();

            return instance.Replace(replacements);
        }

#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
        /// <summary>
        /// Removes all instances of a string.
        /// </summary>
        /// <param name="instance">The string to work on.</param>
        /// <param name="value">The string to remove.</param>
        /// <param name="comparisonType">One of the enumeration values that determines how value is searched within this instance.</param>
        /// <returns>Altered string.</returns>
        public static string Remove(
            this string instance,
            string value,
            StringComparison comparisonType)
        {
            ArgumentNullException.ThrowIfNull(instance);

            return instance.Replace(
                value,
                string.Empty,
                comparisonType);
        }
#endif

        /// <summary>
        /// Converts a string to a memory stream.
        /// </summary>
        /// <param name="instance">String to convert.</param>
        /// <returns>Memory stream.</returns>
        public static MemoryStream ToMemoryStream(this string instance)
        {
            ArgumentNullException.ThrowIfNull(instance);

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
        /// Throws a <see cref="ArgumentNullException"/> if a string is null or whitespace.
        /// </summary>
        /// <param name="argument">string to check.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="argument" /> corresponds.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNullOrWhitespace(
            this string? argument,
            [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        {
            if (argument == null)
            {
                throw new System.ArgumentNullException(paramName);
            }

            if (argument.Any(c => !char.IsWhiteSpace(c)))
            {
                return;
            }

            throw new ArgumentException(
                "input is all whitespace",
                paramName);
        }
    }
}
