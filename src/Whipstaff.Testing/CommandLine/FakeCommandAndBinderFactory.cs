// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.CommandLine;
using System.IO.Abstractions;
using Whipstaff.CommandLine;

namespace Whipstaff.Testing.CommandLine
{
    /// <summary>
    /// Fake Command and Binder Factory for testing.
    /// </summary>
    public sealed class FakeCommandAndBinderFactory : IRootCommandAndBinderFactory<FakeCommandLineArgModelBinder>
    {
        /// <inheritdoc/>
        public RootCommandAndBinderModel<FakeCommandLineArgModelBinder> GetRootCommandAndBinder(IFileSystem fileSystem)
        {
            var fileArgument = new Argument<IFileInfo>("filename");
            var nameArgument = new Argument<string?>("name");

            return new RootCommandAndBinderModel<FakeCommandLineArgModelBinder>(
                [
                    fileArgument,
                    nameArgument
                ],
                new FakeCommandLineArgModelBinder(
                    fileArgument,
                    nameArgument));
        }
    }
}
