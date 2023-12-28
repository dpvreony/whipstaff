// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine.Parsing;
using System.IO;
using System.Linq;
using Whipstaff.Runtime.Extensions;

#if ARGUMENT_NULL_EXCEPTION_SHIM
using ArgumentNullException = Whipstaff.Runtime.Exceptions.ArgumentNullException;
#endif

namespace Whipstaff.CommandLine
{
    /// <summary>
    /// Helpers for <see cref="SymbolResult"/>.
    /// </summary>
    public static class SymbolResultHelpers
    {
        /// <summary>
        /// Checks that a file has a specific extension.
        /// </summary>
        /// <param name="result">Argument result to check.</param>
        /// <param name="extension">Expected file extension.</param>
        public static void FileHasSupportedExtension(
            SymbolResult result,
            string extension)
        {
            ArgumentNullException.ThrowIfNull(result);
            extension.ThrowIfNullOrWhitespace();

            foreach (var token in result.Tokens)
            {
                var rawValue = token.Value;
                var tokenExtension = Path.GetExtension(rawValue);

                if (string.IsNullOrWhiteSpace(tokenExtension)
                    || !tokenExtension.Equals(extension, StringComparison.OrdinalIgnoreCase))
                {
                    result.ErrorMessage = $"Filename \"{rawValue}\" does not have a supported extension of \"{extension}\".";
                    return;
                }
            }
        }

        /// <summary>
        /// Checks that a file one of any supported extension.
        /// </summary>
        /// <param name="result">Argument result to check.</param>
        /// <param name="extensions">Supported file extensions.</param>
        public static void FileHasSupportedExtension(
            SymbolResult result,
            string[] extensions)
        {
            ArgumentNullException.ThrowIfNull(result);
            ArgumentNullException.ThrowIfNull(extensions);

            foreach (var token in result.Tokens)
            {
                var rawValue = token.Value;
                var tokenExtension = Path.GetExtension(rawValue);

                if (string.IsNullOrWhiteSpace(tokenExtension)
                    || !extensions.Any(extension => tokenExtension.Equals(extension, StringComparison.OrdinalIgnoreCase)))
                {
                    result.ErrorMessage = $"Filename \"{rawValue}\" does not have a supported extension of \"{string.Join(",", extensions)}\".";
                    return;
                }
            }
        }
    }
}
