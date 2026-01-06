// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Whipstaff.Runtime.Extensions;

#if ARGUMENT_NULL_EXCEPTION_SHIM
using ArgumentNullException = Whipstaff.Runtime.Exceptions.ArgumentNullException;
#endif

namespace Whipstaff.Runtime.StringCleansing
{
    /// <summary>
    /// Helper class for carrying out a batch of string replacements using the same dictionary.
    /// </summary>
    public sealed class BatchStringReplacementHelper
    {
        private readonly Dictionary<char, string> _replacements;

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchStringReplacementHelper"/> class.
        /// </summary>
        /// <param name="replacements">Dictionary of replacements.</param>
#pragma warning disable GR0027 // Constructor should have a logging framework instance as the final parameter.
        public BatchStringReplacementHelper(Dictionary<char, string> replacements)
        {
            ArgumentNullException.ThrowIfNull(replacements);
            _replacements = replacements;
        }
#pragma warning restore GR0027 // Constructor should have a logging framework instance as the final parameter.

        /// <summary>
        /// Gets a batch string replacement helper for Microsoft Word smart quote replacements.
        /// </summary>
        /// <returns>Batch replacement helper.</returns>
        public static BatchStringReplacementHelper GetMicrosoftWordSmartQuoteReplacements()
        {
            return new BatchStringReplacementHelper(StringCleansingDictionaryFactories.GetMicrosoftWordSmartQuoteReplacements());
        }

        /// <summary>
        /// Replaces characters in a string using a dictionary.
        /// </summary>
        /// <param name="input">String to modify.</param>
        /// <returns>Modified string.</returns>
        public string Replace(string input)
        {
            ArgumentNullException.ThrowIfNull(input);

            return input.Replace(_replacements);
        }
    }
}
