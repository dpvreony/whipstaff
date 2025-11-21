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
    /// Extensions for Option manipulation.
    /// </summary>
    public static class OptionExtensions
    {
        /// <summary>
        /// Adds a validator to check that a file has a specific extension.
        /// </summary>
        /// <param name="option">Option to validate.</param>
        /// <param name="fileSystem">File system abstraction.</param>
        /// <param name="extension">Valid file extension.</param>
        /// <returns><see cref="Option{T}"/> object for use in Fluent API calls.</returns>
        public static Option<FileInfo> SpecificFileExtensionOnly(
            this Option<FileInfo> option,
            IFileSystem fileSystem,
            string extension)
        {
            ArgumentNullException.ThrowIfNull(fileSystem);
            extension.ThrowIfNullOrWhitespace();
            option.Validators.Add(result => SymbolResultHelpers.FileHasSupportedExtension(
                result,
                fileSystem,
                extension));

            return option;
        }

        /// <summary>
        /// Adds a validator to check that a file has from a collection of extensions.
        /// </summary>
        /// <param name="option">Option to validate.</param>
        /// <param name="fileSystem">File system abstraction.</param>
        /// <param name="extensions">Array of valid file extension.</param>
        /// <returns><see cref="Option{T}"/> object for use in Fluent API calls.</returns>
        public static Option<FileInfo> SpecificFileExtensionsOnly(
            this Option<FileInfo> option,
            IFileSystem fileSystem,
            string[] extensions)
        {
            ArgumentNullException.ThrowIfNull(fileSystem);
            ArgumentNullException.ThrowIfNull(extensions);
            option.Validators.Add(result => SymbolResultHelpers.FileHasSupportedExtension(
                result,
                fileSystem,
                extensions));

            return option;
        }

        /// <summary>
        /// Adds a validator to check that a file exists using an abstracted file system.
        /// </summary>
        /// <param name="option">Option to validate.</param>
        /// <param name="fileSystem">File System abstraction.</param>
        /// <returns><see cref="Option{T}"/> object for use in Fluent API calls.</returns>
        public static Option<FileInfo> ExistingOnly(this Option<FileInfo> option, IFileSystem fileSystem)
        {
            ArgumentNullException.ThrowIfNull(fileSystem);
            option.Validators.Add(result => SymbolResultHelpers.FileExists(result, fileSystem));
            return option;
        }
    }
}
