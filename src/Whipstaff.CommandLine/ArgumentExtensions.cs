// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.IO;
using System.Linq;

namespace Whipstaff.CommandLine
{
    /// <summary>
    /// Extensions for Argument manipulation.
    /// </summary>
    public static class ArgumentExtensions
    {
        /// <summary>
        /// Adds a validator to check that a file has a specific extension.
        /// </summary>
        /// <param name="argument">Argument to validate.</param>
        /// <param name="extension">Valid file extension.</param>
        /// <returns><see cref="Argument"/> object for use in Fluent API calls.</returns>
        public static Argument<FileInfo> SpecificFileExtensionOnly(this Argument<FileInfo> argument, string extension)
        {
            argument.AddValidator(result => FileHasSupportedExtension(result, extension));
            return argument;
        }

        /// <summary>
        /// Adds a validator to check that a file has from a collection of extensions.
        /// </summary>
        /// <param name="argument">Argument to validate.</param>
        /// <param name="extensions">Array of valid file extension.</param>
        /// <returns><see cref="Argument"/> object for use in Fluent API calls.</returns>
        public static Argument<FileInfo> SpecificFileExtensionOnly(this Argument<FileInfo> argument, string[] extensions)
        {
            argument.AddValidator(result => FileHasSupportedExtension(result, extensions));
            return argument;
        }

        private static void FileHasSupportedExtension(ArgumentResult result, string extension)
        {
            foreach (var token in result.Tokens)
            {
                var tokenExtension = Path.GetExtension(token.Value);

                if (string.IsNullOrWhiteSpace(tokenExtension)
                    || !tokenExtension.Equals(extension, StringComparison.OrdinalIgnoreCase))
                {
                    result.ErrorMessage = $"Filename does not have a supported extension of \"{extension}\".";
                    return;
                }
            }
        }

        private static void FileHasSupportedExtension(ArgumentResult result, string[] extensions)
        {
            foreach (var token in result.Tokens)
            {
                var tokenExtension = Path.GetExtension(token.Value);

                if (string.IsNullOrWhiteSpace(tokenExtension)
                    || !extensions.Any(extension => tokenExtension.Equals(extension, StringComparison.OrdinalIgnoreCase)))
                {
                    result.ErrorMessage = $"Filename does not have a supported extension of \"{string.Join(",", extensions)}\".";
                    return;
                }
            }
        }
    }
}
