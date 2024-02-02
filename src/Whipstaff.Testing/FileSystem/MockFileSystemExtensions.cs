// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Reflection;
using Whipstaff.Runtime.Extensions;

namespace Whipstaff.Testing.FileSystem
{
    /// <summary>
    /// Extensions for the <see cref="System.IO.Abstractions.TestingHelpers.MockFileSystem"/> class.
    /// </summary>
    public static class MockFileSystemExtensions
    {
        /// <summary>
        /// Adds all embedded files from a namespace to the mock file system.
        /// </summary>
        /// <param name="mockFileSystem">The mock file system to populate.</param>
        /// <param name="assembly">The assembly containing the resources.</param>
        /// <param name="namespaceName">The namespace name to scan.</param>
        /// <param name="rootPath">The root path to place the files under in the virtual file system.</param>
        public static void AddAllEmbeddedFilesFromNamespace(
            this MockFileSystem mockFileSystem,
            Assembly assembly,
            string namespaceName,
            string rootPath)
        {
            ArgumentNullException.ThrowIfNull(mockFileSystem);
            ArgumentNullException.ThrowIfNull(assembly);
            namespaceName.ThrowIfNullOrWhitespace();
            rootPath.ThrowIfNullOrWhitespace();

            var resources = assembly.GetManifestResourceNames()
                .Where(x => x.StartsWith(namespaceName, StringComparison.Ordinal));

            foreach (var resource in resources)
            {
                try
                {
                    var byteContent = assembly.GetManifestResourceStreamAsByteArray(resource);

                    var path = mockFileSystem.Path.Combine(
                        rootPath,
                        resource.Replace(
                            '.',
                            Path.DirectorySeparatorChar));

                    mockFileSystem.AddFile(
                        path,
                        new MockFileData(byteContent));
                }
#pragma warning disable CA1031
                catch
#pragma warning restore CA1031
                {
                    // no op
                }
            }
        }
    }
}
