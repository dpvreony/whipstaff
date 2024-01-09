// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO.Abstractions;

namespace Whipstaff.CommandLine
{
    /// <summary>
    /// Factory for creating a root command and binder.
    /// </summary>
    /// <typeparam name="TCommandLineArgModelBinder">The type for the Command Line Argument Binder.</typeparam>
    public interface IRootCommandAndBinderFactory<TCommandLineArgModelBinder>
    {
        /// <summary>
        /// Gets the root command and binder.
        /// </summary>
        /// <param name="fileSystem">File System abstraction.</param>
        /// <returns>The root command and binder.</returns>
        RootCommandAndBinderModel<TCommandLineArgModelBinder> GetRootCommandAndBinder(IFileSystem fileSystem);
    }
}
