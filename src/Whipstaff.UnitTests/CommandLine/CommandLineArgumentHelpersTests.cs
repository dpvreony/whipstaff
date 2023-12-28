// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.CommandLine.IO;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetTestRegimentation;
using Whipstaff.CommandLine;
using Whipstaff.Testing.CommandLine;
using Xunit;
using Xunit.Abstractions;

namespace Whipstaff.UnitTests.CommandLine
{
    /// <summary>
    /// Unit Tests for <see cref="Whipstaff.CommandLine.CommandLineArgumentHelpers"/>.
    /// </summary>
    public static class CommandLineArgumentHelpersTests
    {
        /// <summary>
        /// Unit Tests for <see cref="Whipstaff.CommandLine.CommandLineArgumentHelpers.GetResultFromRootCommand{TCommandLineArg, TCommandLineArgModelBinder}"/>.
        /// </summary>
        public sealed class GetResultFromRootCommandMethod
            : Foundatio.Xunit.TestWithLoggingBase,
                ITestAsyncMethodWithNullableParameters<string[], Func<RootCommandAndBinderModel<FakeCommandLineArgModelBinder>>, Func<FakeCommandLineArgModel, Task<int>>>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetResultFromRootCommandMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public GetResultFromRootCommandMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            [ClassData(typeof(Whipstaff.UnitTests.TestSources.CommandLine.CommandLineArgumentHelpersTests.GetResultFromRootCommandMethod.ThrowsArgumentNullExceptionTestSource))]
            [Theory]
            public async Task ThrowsArgumentNullExceptionAsync(
                string[] arg1,
                Func<RootCommandAndBinderModel<FakeCommandLineArgModelBinder>> arg2,
                Func<FakeCommandLineArgModel, Task<int>> arg3,
                string expectedParameterNameForException)
            {
                _ = await Assert.ThrowsAsync<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => CommandLineArgumentHelpers.GetResultFromRootCommand(
                        arg1,
                        arg2,
                        arg3));
            }

            /// <summary>
            /// Test to ensure that the method returns an instance on a successful run.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsInstance()
            {
                var args = new[] { "testfile.txt", "testname" };

                var rootCommandAndBinderModelFunc = new Func<RootCommandAndBinderModel<FakeCommandLineArgModelBinder>>(
                    () =>
                    {
                        var fileArgument = new Argument<FileInfo>("filename");
                        var nameArgument = new Argument<string?>("name");

                        var rootCommand = new RootCommand();
                        rootCommand.AddArgument(fileArgument);
                        rootCommand.AddArgument(nameArgument);

                        var binder = new FakeCommandLineArgModelBinder(fileArgument, nameArgument);

                        return new RootCommandAndBinderModel<FakeCommandLineArgModelBinder>(
                            rootCommand,
                            binder);
                    });

                var console = new TestConsole();

                var result = await CommandLineArgumentHelpers.GetResultFromRootCommand<FakeCommandLineArgModel, FakeCommandLineArgModelBinder>(
                    args,
                    rootCommandAndBinderModelFunc,
                    arg => Task.FromResult(0),
                    console);

                _logger.LogInformation("Console output: {ConsoleOutput}", console.Out.ToString());
                _logger.LogInformation("Console error: {ConsoleError}", console.Error.ToString());

                Assert.Equal(0, result);
            }
        }
    }
}
