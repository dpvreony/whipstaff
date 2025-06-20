// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Threading;
using System.Threading.Tasks;
using NetTestRegimentation.XUnit.Theories.ArgumentNullException;
using Whipstaff.CommandLine;
using Whipstaff.Testing.CommandLine;

namespace Whipstaff.UnitTests.TestSources.CommandLine.CommandLineArgumentHelpersTests.GetResultFromRootCommandMethod
{
    /// <summary>
    /// Test Source for <see cref="Whipstaff.UnitTests.CommandLine.CommandLineArgumentHelpersTests.GetResultFromRootCommandMethod.ThrowsArgumentNullExceptionAsync"/>.
    /// </summary>
    public sealed class ThrowsArgumentNullExceptionTestSource : ArgumentNullExceptionTheoryData<
        string[],
        Func<IFileSystem, RootCommandAndBinderModel<FakeCommandLineArgModelBinder>>,
        Func<FakeCommandLineArgModel, CancellationToken, Task<int>>,
        IFileSystem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
        /// </summary>
        public ThrowsArgumentNullExceptionTestSource()
            : base(
                new NamedParameterInput<string[]>(
                    "args",
                    () => [".txt", ".docx"]),
                new NamedParameterInput<Func<IFileSystem, RootCommandAndBinderModel<FakeCommandLineArgModelBinder>>>(
                    "rootCommandAndBinderModelFunc",
                    () => _ => new RootCommandAndBinderModel<FakeCommandLineArgModelBinder>(
                    new RootCommand(),
                    new FakeCommandLineArgModelBinder(
                        new Argument<IFileInfo>("--file"), new Argument<string?>("--name")))),
                new NamedParameterInput<Func<FakeCommandLineArgModel, CancellationToken, Task<int>>>(
                    "rootCommandHandlerFunc",
                    () => (_, _) => Task.FromResult(0)),
                new NamedParameterInput<IFileSystem>(
                    "fileSystem",
                    () => new MockFileSystem()))
        {
        }
    }
}
