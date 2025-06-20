// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine.Parsing;
using System.IO;
using System.IO.Abstractions;
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
        /// <param name="fileSystem">File system abstraction.</param>
        /// <param name="extension">Expected file extension.</param>
        public static void FileHasSupportedExtension(
            SymbolResult result,
            IFileSystem fileSystem,
            string extension)
        {
            ArgumentNullException.ThrowIfNull(result);
            ArgumentNullException.ThrowIfNull(fileSystem);
            extension.ThrowIfNullOrWhitespace();

            foreach (var rawValue in result.Tokens.Select(t => t.Value))
            {
                var tokenExtension = fileSystem.Path.GetExtension(rawValue);

                if (string.IsNullOrWhiteSpace(tokenExtension)
                    || !tokenExtension.Equals(extension, StringComparison.OrdinalIgnoreCase))
                {
                    result.AddError($"Filename \"{rawValue}\" does not have a supported extension of \"{extension}\".");
                    return;
                }
            }
        }

        /// <summary>
        /// Checks that a file one of any supported extension.
        /// </summary>
        /// <param name="result">Argument result to check.</param>
        /// <param name="fileSystem">File system abstraction.</param>
        /// <param name="extensions">Supported file extensions.</param>
        public static void FileHasSupportedExtension(
            SymbolResult result,
            IFileSystem fileSystem,
            string[] extensions)
        {
            ArgumentNullException.ThrowIfNull(result);
            ArgumentNullException.ThrowIfNull(fileSystem);
            ArgumentNullException.ThrowIfNull(extensions);

            foreach (var rawValue in result.Tokens.Select(t => t.Value))
            {
                var tokenExtension = fileSystem.Path.GetExtension(rawValue);

                if (string.IsNullOrWhiteSpace(tokenExtension)
                    || !Array.Exists(extensions, value => value.Equals(tokenExtension, StringComparison.OrdinalIgnoreCase)))
                {
                    result.AddError($"Filename \"{rawValue}\" does not have a supported extension of \"{string.Join(",", extensions)}\".");
                    return;
                }
            }
        }

        /// <summary>
        /// Checks that a file exists.
        /// </summary>
        /// <param name="result">Argument result to check.</param>
        /// <param name="fileSystem">File system abstraction.</param>
        public static void FileExists(
            SymbolResult result,
            IFileSystem fileSystem)
        {
            ArgumentNullException.ThrowIfNull(result);
            ArgumentNullException.ThrowIfNull(fileSystem);
            foreach (var rawValue in result.Tokens.Select(t => t.Value).Where(rawValue => !fileSystem.File.Exists(rawValue)))
            {
                result.AddError($"Filename \"{rawValue}\" was not found.");
            }
        }
    }
}
