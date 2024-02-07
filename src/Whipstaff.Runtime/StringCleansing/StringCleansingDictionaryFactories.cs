// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Whipstaff.Runtime.StringCleansing
{
    /// <summary>
    /// Factories for creating <see cref="Dictionary{TKey,TValue}"/> instances for string cleansing.
    /// </summary>
    public static class StringCleansingDictionaryFactories
    {
        /// <summary>
        /// Gets a dictionary of Microsoft Word smart quote replacements.
        /// </summary>
        /// <returns>Dictionary of replacements.</returns>
        public static Dictionary<char, string> GetMicrosoftWordSmartQuoteReplacements()
        {
            return new Dictionary<char, string>
            {
                { '\u2014', "-" },
                { '\u2015', "-" },
                { '\u2017', "_" },
                { '\u2018', "\'" },
                { '\u2019', "\'" },
                { '\u201a', "'," },
                { '\u201b', "\'" },
                { '\u201c', "\"" },
                { '\u201d', "\"" },
                { '\u201e', "\"" },
                { '\u2032', "\'" },
                { '\u2033', "\"" },
                { '\u0092', "\'" },
                { '\u2026', "..." },
                { '\u0085', "..." },
            };
        }
    }
}
