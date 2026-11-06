// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetTestRegimentation.XUnit.Logging;
using Rocks;
using TextCopy;
using Whipstaff.CommandLine.Hosting;
using Whipstaff.CommandLine.VersionWithCopyToClipboard;
using Whipstaff.Testing.CommandLine;
using Xunit;

namespace Whipstaff.UnitTests.CommandLine.VersionWithCopyToClipboard
{
    /// <summary>
    /// Unit Tests for <see cref="RootCommandExtensions"/>.
    /// </summary>
    public static class RootCommandExtensionsTests
    {
        /// <summary>
        /// Unit Tests for <see cref="RootCommandExtensions.EnableVersionWithClipboardCopy(RootCommand, IClipboard)"/>.
        /// </summary>
        public sealed class EnableVersionWithClipboardCopyMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="EnableVersionWithClipboardCopyMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public EnableVersionWithClipboardCopyMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure the root command is modified correctly.
            /// </summary>
            [Fact]
            public void ModifiesRootCommand()
            {
                var rootCommand = new System.CommandLine.RootCommand();
                rootCommand.EnableVersionWithClipboardCopy(new MockClipboard());

                Assert.Empty(rootCommand.Options.OfType<VersionOption>());
                _ = Assert.Single(rootCommand.Options.OfType<Whipstaff.CommandLine.VersionWithCopyToClipboard.VersionCopyOption>());
                _ = Assert.Single(rootCommand.Options.OfType<Whipstaff.CommandLine.VersionWithCopyToClipboard.VersionWithCopyToClipboardOption>());
            }

            /// <summary>
            /// Invokes the root command with `--version` after enabling version output with clipboard copy support and
            /// verifies successful execution.
            /// </summary>
            /// <remarks>Validates that parsing `--version` produces no errors and that invocation
            /// returns exit code `0`.</remarks>
            /// <returns>A task that represents the asynchronous test operation.</returns>
            [Fact]
            public async Task ReturnsVersionWithoutClipboardContent()
            {
                await DoConsoleTestAsync(
                    "--version",
                    false);
            }

            /// <summary>
            /// Verifies that the version information is written to the console and copied to the clipboard when the
            /// version and copy options are specified.
            /// </summary>
            /// <returns>A task that represents the asynchronous test operation.</returns>
            [Fact]
            public async Task OutputsVersionToConsoleAndClipboard()
            {
                await DoConsoleTestAsync(
                    "--version --copy",
                    true);
            }

            private async Task DoConsoleTestAsync(string args, bool expectClipboardToPopulate)
            {
                await using (var outputWriter = new StringWriter())
                await using (var errorWriter = new StringWriter())
                {
                    var clipboard = new InMemoryClipboard();
                    var rootCommand = new System.CommandLine.RootCommand();
                    rootCommand.EnableVersionWithClipboardCopy(clipboard);

                    var guid = Guid.NewGuid().ToString();
                    await clipboard.SetTextAsync(
                        guid,
                        TestContext.Current.CancellationToken);

                    var parseResult = rootCommand.Parse(args);
                    Assert.Empty(parseResult.Errors);

                    var result = await parseResult.InvokeAsync(
                        XUnitTestHelpers.CreateTestConsoleIntegration(outputWriter, errorWriter),
                        TestContext.Current.CancellationToken);

                    var consoleOutput = outputWriter.ToString().Trim();
                    var errorOutput = errorWriter.ToString().Trim();

                    Logger.LogInformation(
                        "Console output: {ConsoleOutput}",
                        consoleOutput);

                    Logger.LogInformation(
                        "Console error: {ConsoleError}",
                        errorOutput);

                    var clipboardContent = await clipboard.GetTextAsync(TestContext.Current.CancellationToken);
                    Assert.Equal(
                        expectClipboardToPopulate ? consoleOutput : guid,
                        clipboardContent);

                    Assert.Multiple(
                        () => Assert.NotEmpty(consoleOutput),
                        () => Assert.Empty(errorOutput));

                    Assert.Equal(0, result);
                }
            }
        }
    }
}
