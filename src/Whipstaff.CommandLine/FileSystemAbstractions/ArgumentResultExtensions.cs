// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.CommandLine.Parsing;
using System.IO.Abstractions;

namespace Whipstaff.CommandLine.FileSystemAbstractions
{
    /// <summary>
    /// Command Line Custom parser extensions for abstracted file system types.
    /// </summary>
    public static class ArgumentResultExtensions
    {
        /// <summary>
        /// Gets an abstracted directory info from an argument result.
        /// </summary>
        /// <param name="argumentResult">
        /// Argument result to parse directory info from.
        /// </param>
        /// <param name="fileSystem">File System abstraction to use to process parsed argument result.</param>
        /// <returns>Representation of the processed argument as abstracted directory information.</returns>
        public static IDirectoryInfo? GetDirectoryInfo(
            this ArgumentResult argumentResult,
            IFileSystem fileSystem)
        {
            if (argumentResult.Tokens.Count != 1)
            {
                argumentResult.AddSingleArgumentError();
                return null;
            }

            return fileSystem.DirectoryInfo.New(argumentResult.Tokens[0].Value);
        }

        /// <summary>
        /// Gets an abstracted drive info from an argument result.
        /// </summary>
        /// <param name="argumentResult">
        /// Argument result to parse drive info from.
        /// </param>
        /// <param name="fileSystem">File System abstraction to use to process parsed argument result.</param>
        /// <returns>Representation of the processed argument as abstracted drive information.</returns>
        public static IDriveInfo? GetDriveInfo(
            this ArgumentResult argumentResult,
            IFileSystem fileSystem)
        {
            if (argumentResult.Tokens.Count != 1)
            {
                argumentResult.AddSingleArgumentError();
                return null;
            }

            return fileSystem.DriveInfo.New(argumentResult.Tokens[0].Value);
        }

        /// <summary>
        /// Gets an abstracted file info from an argument result.
        /// </summary>
        /// <param name="argumentResult">
        /// Argument result to parse file info from.
        /// </param>
        /// <param name="fileSystem">File System abstraction to use to process parsed argument result.</param>
        /// <returns>Representation of the processed argument as abstracted file information.</returns>
        public static IFileInfo? GetFileInfo(
            this ArgumentResult argumentResult,
            IFileSystem fileSystem)
        {
            if (argumentResult.Tokens.Count != 1)
            {
                argumentResult.AddSingleArgumentError();
                return null;
            }

            return fileSystem.FileInfo.New(argumentResult.Tokens[0].Value);
        }

        private static void AddSingleArgumentError(this ArgumentResult argumentResult) => argumentResult.AddError($"{argumentResult.Argument.Name} requires single argument");
    }
}
