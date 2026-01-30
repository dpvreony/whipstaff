// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.IO.Abstractions;

namespace Whipstaff.CommandLine.FileSystemAbstractions
{
    /// <summary>
    /// Factory for creating CLI options that parse into file system abstractions.
    /// </summary>
    public static class OptionFactory
    {
        /// <summary>
        /// Gets an option that parses to a directory info abstraction.
        /// </summary>
        /// <param name="name">Name of the option on the command line.</param>
        /// <param name="fileSystem">Abstracted file system instance.</param>
        /// <param name="optionAction">Action to carry out any additional configuration of the option.</param>
        /// <returns>Option that processes into an abstracted directory info.</returns>
        public static Option<IDirectoryInfo> GetDirectoryInfoOption(
            string name,
            IFileSystem fileSystem,
            Action<Option<IDirectoryInfo>>? optionAction = null)
        {
            var option = new Option<IDirectoryInfo>(name)
            {
                CustomParser = argumentResult => argumentResult.GetDirectoryInfo(fileSystem)
            };

            optionAction?.Invoke(option);

            return option;
        }

        /// <summary>
        /// Gets an option that parses to a drive info abstraction.
        /// </summary>
        /// <param name="name">Name of the option on the command line.</param>
        /// <param name="fileSystem">Abstracted file system instance.</param>
        /// <param name="optionAction">Action to carry out any additional configuration of the option.</param>
        /// <returns>Option that processes into an abstracted drive info.</returns>
        public static Option<IDriveInfo> GetDriveInfoOption(
            string name,
            IFileSystem fileSystem,
            Action<Option<IDriveInfo>>? optionAction = null)
        {
            var option = new Option<IDriveInfo>(name)
            {
                CustomParser = argumentResult => argumentResult.GetDriveInfo(fileSystem)
            };

            optionAction?.Invoke(option);

            return option;
        }

        /// <summary>
        /// Gets an option that parses to a file info abstraction.
        /// </summary>
        /// <param name="name">Name of the option on the command line.</param>
        /// <param name="fileSystem">Abstracted file system instance.</param>
        /// <param name="optionAction">Action to carry out any additional configuration of the option.</param>
        /// <returns>Option that processes into an abstracted file info.</returns>
        public static Option<IFileInfo> GetFileInfoArgument(
            string name,
            IFileSystem fileSystem,
            Action<Option<IFileInfo>>? optionAction = null)
        {
            var option = new Option<IFileInfo>(name)
            {
                CustomParser = argumentResult => argumentResult.GetFileInfo(fileSystem)
            };

            optionAction?.Invoke(option);

            return option;
        }
    }
}
