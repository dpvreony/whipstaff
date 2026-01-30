// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.IO.Abstractions;

namespace Whipstaff.CommandLine.FileSystemAbstractions
{
    /// <summary>
    /// Factory for creating CLI arguments that parse into file system abstractions.
    /// </summary>
    public static class ArgumentFactory
    {
        /// <summary>
        /// Gets an argument that parses to a directory info abstraction.
        /// </summary>
        /// <param name="name">Name of the argument on the command line.</param>
        /// <param name="fileSystem">Abstracted file system instance.</param>
        /// <param name="argumentAction">Action to carry out any additional configuration of the argument.</param>
        /// <returns>Argument that processes into an abstracted directory info.</returns>
        public static Argument<IDirectoryInfo> GetDirectoryInfoArgument(
            string name,
            IFileSystem fileSystem,
            Action<Argument<IDirectoryInfo>>? argumentAction = null)
        {
            var arg = new Argument<IDirectoryInfo>(name)
            {
                CustomParser = argumentResult => argumentResult.GetDirectoryInfo(fileSystem)
            };

            argumentAction?.Invoke(arg);

            return arg;
        }

        /// <summary>
        /// Gets an argument that parses to a drive info abstraction.
        /// </summary>
        /// <param name="name">Name of the argument on the command line.</param>
        /// <param name="fileSystem">Abstracted file system instance.</param>
        /// <param name="argumentAction">Action to carry out any additional configuration of the argument.</param>
        /// <returns>Argument that processes into an abstracted drive info.</returns>
        public static Argument<IDriveInfo> GetDriveInfoArgument(
            string name,
            IFileSystem fileSystem,
            Action<Argument<IDirectoryInfo>>? argumentAction = null)
        {
            var arg = new Argument<IDriveInfo>(name)
            {
                CustomParser = argumentResult => argumentResult.GetDriveInfo(fileSystem)
            };

            argumentAction?.Invoke(arg);

            return arg;
        }

        /// <summary>
        /// Gets an argument that parses to a file info abstraction.
        /// </summary>
        /// <param name="name">Name of the argument on the command line.</param>
        /// <param name="fileSystem">Abstracted file system instance.</param>
        /// <param name="argumentAction">Action to carry out any additional configuration of the argument.</param>
        /// <returns>Argument that processes into an abstracted file info.</returns>
        public static Argument<IFileInfo> GetFileInfoArgument(
            string name,
            IFileSystem fileSystem,
            Action<Argument<IDirectoryInfo>>? argumentAction = null)
        {
            var arg = new Argument<IFileInfo>(name)
            {
                CustomParser = argumentResult => argumentResult.GetFileInfo(fileSystem)
            };

            argumentAction?.Invoke(arg);

            return arg;
        }
    }
}
