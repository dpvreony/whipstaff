// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using NetTestRegimentation;
using Whipstaff.CommandLine;
using Whipstaff.Testing.Logging;
using Xunit;

namespace Whipstaff.UnitTests.CommandLine
{
    /// <summary>
    /// Unit Tests for <see cref="Whipstaff.CommandLine.SymbolResultHelpers"/>.
    /// </summary>
    public static class SymbolResultHelpersTests
    {
        /// <summary>
        /// Unit Tests for <see cref="Whipstaff.CommandLine.SymbolResultHelpers.FileHasSupportedExtension(SymbolResult, IFileSystem, string)"/>.
        /// </summary>
        public sealed class FileHasSupportedExtensionMethod
            : TestWithLoggingBase,
                ITestMethodWithNullableParameters<SymbolResult, IFileSystem, string>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="FileHasSupportedExtensionMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public FileHasSupportedExtensionMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            [ClassData(typeof(Whipstaff.UnitTests.TestSources.CommandLine.SymbolResultHelpersTests.FileHasSupportedExtensionMethod.ThrowsArgumentNullExceptionTestSource))]
            [Theory]
            public void ThrowsArgumentNullException(
                SymbolResult? arg1,
                IFileSystem? arg2,
                string? arg3,
                string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => SymbolResultHelpers.FileHasSupportedExtension(arg1!, arg2!, arg3!));
            }

            /// <summary>
            /// Test to ensure that the method returns an instance on a successful run.
            /// </summary>
            [Fact]
            public void ReturnsErrorMessageForUnsupportedExtension()
            {
                const string argName = "filename";
                const string argValue = "somefilename.cer";
                const string extension = ".txt";

                var fileSystem = new MockFileSystem();
                var argument1Builder = new Argument<IFileInfo>(argName);

                string[] args =
                [
                    argValue
                ];

                var rootCommand = new RootCommand { argument1Builder };
                var parseResult = rootCommand.Parse(args);

                var argumentResult = parseResult.GetResult(argument1Builder);

                Assert.NotNull(argumentResult);

                SymbolResultHelpers.FileHasSupportedExtension(argumentResult, fileSystem, extension);
                var errorMessage = argumentResult.Errors.First().Message;

                Assert.Equal(
                    $"Filename \"{argValue}\" does not have a supported extension of \"{extension}\".",
                    errorMessage);
            }

            /// <summary>
            /// Test to ensure that the method returns an instance on a successful run.
            /// </summary>
            [Fact]
            public void SucceedsForSupportedExtension()
            {
                const string argName = "filename";
                const string argValue = "somefilename.txt";
                const string extension = ".txt";

                var fileSystem = new MockFileSystem();
                var argument1Builder = new Argument<IFileInfo>(argName);

                string[] args =
                [
                    argValue
                ];

                var rootCommand = new RootCommand { argument1Builder };
                var parseResult = rootCommand.Parse(args);
                var argumentResult = parseResult.GetResult(argument1Builder);

                Assert.NotNull(argumentResult);

                SymbolResultHelpers.FileHasSupportedExtension(
                    argumentResult,
                    fileSystem,
                    extension);

                Assert.Empty(argumentResult.Errors);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="Whipstaff.CommandLine.SymbolResultHelpers.FileHasSupportedExtension(SymbolResult, IFileSystem, string)"/>.
        /// </summary>
        public sealed class FileHasSupportedExtension2Method
            : TestWithLoggingBase,
                ITestMethodWithNullableParameters<SymbolResult, IFileSystem, string[]>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="FileHasSupportedExtension2Method"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public FileHasSupportedExtension2Method(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            [ClassData(typeof(Whipstaff.UnitTests.TestSources.CommandLine.SymbolResultHelpersTests.FileHasSupportedExtension2Method.ThrowsArgumentNullExceptionTestSource))]
            [Theory]
            public void ThrowsArgumentNullException(
                SymbolResult? arg1,
                IFileSystem? arg2,
                string[]? arg3,
                string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => SymbolResultHelpers.FileHasSupportedExtension(arg1!, arg2!, arg3!));
            }

            /// <summary>
            /// Test to ensure that the method returns an instance on a successful run.
            /// </summary>
            [Fact]
            public void ReturnsErrorMessageForUnsupportedExtension()
            {
                const string argName = "filename";
                const string argValue = "somefilename.cer";
                var extensions = new[] { ".txt", ".docx" };

                var fileSystem = new MockFileSystem();
                var argument1Builder = new Argument<IFileInfo>(argName);

                string[] args =
                [
                    argValue
                ];

                var rootCommand = new RootCommand { argument1Builder };
                var parseResult = rootCommand.Parse(args);
                var argumentResult = parseResult.GetResult(argument1Builder);

                Assert.NotNull(argumentResult);

                SymbolResultHelpers.FileHasSupportedExtension(
                    argumentResult,
                    fileSystem,
                    extensions);

                var errorMessage = argumentResult.Errors.First().Message;

                Assert.Equal(
                    $"Filename \"{argValue}\" does not have a supported extension of \"{string.Join(",", extensions)}\".",
                    errorMessage);
            }

            /// <summary>
            /// Test to ensure that the method returns an instance on a successful run.
            /// </summary>
            [Fact]
            public void SucceedsForSupportedExtension()
            {
                const string argName = "filename";
                const string argValue = "somefilename.txt";
                var extensions = new[] { ".txt", ".docx" };

                var fileSystem = new MockFileSystem();
                var argument1Builder = new Argument<IFileInfo>(argName);

                string[] args =
                [
                    argValue
                ];

                var rootCommand = new RootCommand { argument1Builder };
                var parseResult = rootCommand.Parse(args);
                var argumentResult = parseResult.GetResult(argument1Builder);

                Assert.NotNull(argumentResult);

                SymbolResultHelpers.FileHasSupportedExtension(
                    argumentResult,
                    fileSystem,
                    extensions);

                Assert.Empty(argumentResult.Errors);
            }
        }
    }
}
