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
            extension.ThrowIfNullOrWhitespace();
            argument.AddValidator(result => ArgumentResultHelpers.FileHasSupportedExtension(result, extension));
            return argument;
        }

        /// <summary>
        /// Adds a validator to check that a file has from a collection of extensions.
        /// </summary>
        /// <param name="argument">Argument to validate.</param>
        /// <param name="extensions">Array of valid file extension.</param>
        /// <returns><see cref="Argument"/> object for use in Fluent API calls.</returns>
        public static Argument<FileInfo> SpecificFileExtensionsOnly(this Argument<FileInfo> argument, string[] extensions)
        {
            ArgumentNullException.ThrowIfNull(extensions);
            argument.AddValidator(result => ArgumentResultHelpers.FileHasSupportedExtension(result, extensions));
            return argument;
        }
    }
}
