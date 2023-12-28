// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.CommandLine;
using System.IO;
using Whipstaff.Runtime.Extensions;

#if ARGUMENT_NULL_EXCEPTION_SHIM
using ArgumentNullException = Whipstaff.Runtime.Exceptions.ArgumentNullException;
#endif

namespace Whipstaff.CommandLine
{
    /// <summary>
    /// Extensions for Option manipulation.
    /// </summary>
    public static class OptionExtensions
    {
        /// <summary>
        /// Adds a validator to check that a file has a specific extension.
        /// </summary>
        /// <param name="option">Option to validate.</param>
        /// <param name="extension">Valid file extension.</param>
        /// <returns><see cref="Option"/> object for use in Fluent API calls.</returns>
        public static Option<FileInfo> SpecificFileExtensionOnly(this Option<FileInfo> option, string extension)
        {
            extension.ThrowIfNullOrWhitespace();
            option.AddValidator(result => SymbolResultHelpers.FileHasSupportedExtension(result, extension));

            return option;
        }

        /// <summary>
        /// Adds a validator to check that a file has from a collection of extensions.
        /// </summary>
        /// <param name="option">Option to validate.</param>
        /// <param name="extensions">Array of valid file extension.</param>
        /// <returns><see cref="Argument"/> object for use in Fluent API calls.</returns>
        public static Option<FileInfo> SpecificFileExtensionsOnly(this Option<FileInfo> option, string[] extensions)
        {
            ArgumentNullException.ThrowIfNull(extensions);
            option.AddValidator(result => SymbolResultHelpers.FileHasSupportedExtension(result, extensions));
            return option;
        }
    }
}
