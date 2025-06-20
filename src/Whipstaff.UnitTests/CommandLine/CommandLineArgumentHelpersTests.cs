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
using Microsoft.Extensions.Logging;
using NetTestRegimentation;
using Whipstaff.CommandLine;
using Whipstaff.Testing.CommandLine;
using Whipstaff.Testing.Logging;
using Xunit;

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
            : TestWithLoggingBase,
                ITestAsyncMethodWithNullableParameters<string[], Func<IFileSystem, RootCommandAndBinderModel<FakeCommandLineArgModelBinder>>, Func<FakeCommandLineArgModel, CancellationToken, Task<int>>, IFileSystem>
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
                string[]? arg1,
                Func<IFileSystem, RootCommandAndBinderModel<FakeCommandLineArgModelBinder>>? arg2,
                Func<FakeCommandLineArgModel, CancellationToken, Task<int>>? arg3,
                IFileSystem? arg4,
                string expectedParameterNameForException)
            {
                _ = await Assert.ThrowsAsync<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => CommandLineArgumentHelpers.GetResultFromRootCommand(
                        arg1!,
                        arg2!,
                        arg3!,
                        arg4!,
                        null,
                        CancellationToken.None));
            }

            /// <summary>
            /// Test to ensure that the method returns an instance on a successful run.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsInstance()
            {
                var args = new[] { "testfile.txt", "testname" };

                var rootCommandAndBinderModelFunc = new Func<IFileSystem, RootCommandAndBinderModel<FakeCommandLineArgModelBinder>>(
                    fileSystem =>
                    {
                        var fileArgument = new Argument<IFileInfo>("filename")
                        {
                            CustomParser = result => fileSystem.FileInfo.New(result.Tokens[0].Value)
                        };
                        var nameArgument = new Argument<string?>("name");

                        var rootCommand = new RootCommand();
                        rootCommand.Arguments.Add(fileArgument);
                        rootCommand.Arguments.Add(nameArgument);

                        var binder = new FakeCommandLineArgModelBinder(fileArgument, nameArgument);

                        return new RootCommandAndBinderModel<FakeCommandLineArgModelBinder>(
                            rootCommand,
                            binder);
                    });

                var fileSystem = new MockFileSystem();

                await using (var outputWriter = new StringWriter())
                await using (var errorWriter = new StringWriter())
                {
                    var result = await CommandLineArgumentHelpers.GetResultFromRootCommand<FakeCommandLineArgModel, FakeCommandLineArgModelBinder>(
                        args,
                        rootCommandAndBinderModelFunc,
                        (_, _) => Task.FromResult(0),
                        fileSystem,
                        rootCommand => XUnitTestHelpers.CreateTestConsoleIntegration(rootCommand, outputWriter, errorWriter),
                        TestContext.Current.CancellationToken);

                    Logger.LogInformation("Console output: {ConsoleOutput}", outputWriter.ToString());
                    Logger.LogInformation("Console error: {ConsoleError}", errorWriter.ToString());

                    Assert.Equal(0, result);
                }
            }
        }
    }
}
