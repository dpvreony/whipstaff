// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.CommandLine.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetTestRegimentation;
using Whipstaff.CommandLine.Hosting;
using Whipstaff.Testing.CommandLine;
using Whipstaff.Testing.Logging;
using Xunit;

namespace Whipstaff.UnitTests.CommandLine.Hosting
{
    /// <summary>
    /// Unit tests for the <see cref="HostRunner"/> class.
    /// </summary>
    public static class HostRunnerTests
    {
        /// <summary>
        /// Unit test for the <see cref="HostRunner.RunSimpleCliJobAsync{TJob, TCommandLineArgModel, TCommandLineArgModelBinder, TRootCommandAndBinderFactory}(string[], Func{IFileSystem, Microsoft.Extensions.Logging.ILogger{TJob}, TJob}, IFileSystem, IConsole)"/> method.
        /// </summary>
        public sealed class RunSimpleCliJobMethod
        : TestWithLoggingBase,
                ITestAsyncMethodWithNullableParameters<string[], Func<IFileSystem, ILogger<FakeCommandLineHandler>, FakeCommandLineHandler>, IFileSystem>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="RunSimpleCliJobMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public RunSimpleCliJobMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            [ClassData(typeof(Whipstaff.UnitTests.TestSources.CommandLine.Hosting.HostRunnerTests.RunSimpleCliJobMethod.ThrowsArgumentNullExceptionAsyncTestSource))]
            [Theory]
            public async Task ThrowsArgumentNullExceptionAsync(
                string[]? arg1,
                Func<IFileSystem, ILogger<FakeCommandLineHandler>, FakeCommandLineHandler>? arg2,
                IFileSystem? arg3,
                string expectedParameterNameForException)
            {
                _ = await Assert.ThrowsAsync<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => HostRunner
                        .RunSimpleCliJobAsync<
                            FakeCommandLineHandler,
                            FakeCommandLineArgModel,
                            FakeCommandLineArgModelBinder,
                            FakeCommandAndBinderFactory>(
                            arg1!,
                            arg2!,
                            arg3!));
            }

            /// <summary>
            /// Test to ensure that the method returns 0 on a successful run.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsZero()
            {
                var testConsole = new TestConsole();
                var result = await HostRunner
                    .RunSimpleCliJobAsync<
                        FakeCommandLineHandler,
                        FakeCommandLineArgModel,
                        FakeCommandLineArgModelBinder,
                        FakeCommandAndBinderFactory>(
                        [
                            "filename",
                            "name"
                        ],
                        (_, _) => new FakeCommandLineHandler(new FakeCommandLineHandlerLogMessageActionsWrapper(LoggerFactory.CreateLogger<FakeCommandLineHandler>())),
                        new MockFileSystem(),
                        testConsole);

                Logger.LogInformation("Console output: {ConsoleOutput}", testConsole.Out.ToString());
                Logger.LogInformation("Console error: {ConsoleError}", testConsole.Error.ToString());

                Assert.Equal(0, result);
            }
        }
    }
}
