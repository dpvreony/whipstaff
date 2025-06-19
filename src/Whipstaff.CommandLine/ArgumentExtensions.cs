// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.CommandLine;
using System.IO;
using System.IO.Abstractions;
using Whipstaff.Runtime.Extensions;

#if ARGUMENT_NULL_EXCEPTION_SHIM
using ArgumentNullException = Whipstaff.Runtime.Exceptions.ArgumentNullException;
#endif

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
        /// <param name="fileSystem">File system abstraction.</param>
        /// <param name="extension">Valid file extension.</param>
        /// <returns><see cref="Argument"/> object for use in Fluent API calls.</returns>
        public static Argument<FileInfo> SpecificFileExtensionOnly(
            this Argument<FileInfo> argument,
            IFileSystem fileSystem,
            string extension)
        {
            ArgumentNullException.ThrowIfNull(fileSystem);
            extension.ThrowIfNullOrWhitespace();
            argument.Validators.Add(result => SymbolResultHelpers.FileHasSupportedExtension(
                result,
                fileSystem,
                extension));
            return argument;
        }

        /// <summary>
        /// Adds a validator to check that a file has from a collection of extensions.
        /// </summary>
        /// <param name="argument">Argument to validate.</param>
        /// <param name="fileSystem">File system abstraction.</param>
        /// <param name="extensions">Array of valid file extension.</param>
        /// <returns><see cref="Argument"/> object for use in Fluent API calls.</returns>
        public static Argument<FileInfo> SpecificFileExtensionsOnly(
            this Argument<FileInfo> argument,
            IFileSystem fileSystem,
            string[] extensions)
        {
            ArgumentNullException.ThrowIfNull(fileSystem);
            ArgumentNullException.ThrowIfNull(extensions);
            argument.Validators.Add(result => SymbolResultHelpers.FileHasSupportedExtension(
                result,
                fileSystem,
                extensions));
            return argument;
        }
    }
}
