// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Microsoft.Extensions.Logging;
using NetTestRegimentation.XUnit.Theories.ArgumentNullException;
using Whipstaff.Testing.CommandLine;

namespace Whipstaff.UnitTests.TestSources.CommandLine.Hosting.HostRunnerTests.RunSimpleCliJobMethod
{
    /// <summary>
    /// Test Source for <see cref="Whipstaff.UnitTests.CommandLine.Hosting.HostRunnerTests.RunSimpleCliJobMethod.ThrowsArgumentNullExceptionAsync"/>.
    /// </summary>
    public sealed class ThrowsArgumentNullExceptionAsyncTestSource : ArgumentNullExceptionTheoryData<string[], Func<IFileSystem, ILogger<FakeCommandLineHandler>, FakeCommandLineHandler>, IFileSystem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionAsyncTestSource"/> class.
        /// </summary>
        public ThrowsArgumentNullExceptionAsyncTestSource()
            : base(
                new NamedParameterInput<string[]>(
                    "args",
                    () => [
                        "arg1",
                        "arg2"
                    ]),
                new NamedParameterInput<Func<IFileSystem, ILogger<FakeCommandLineHandler>, FakeCommandLineHandler>>(
                    "commandLineHandlerFactoryFunc",
                    () => (_, logger) => new FakeCommandLineHandler(new FakeCommandLineHandlerLogMessageActionsWrapper(logger))),
                new NamedParameterInput<IFileSystem>(
                    "fileSystem",
                    () => new MockFileSystem()))
        {
        }
    }
}
