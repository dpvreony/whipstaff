// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.IO;
using NetTestRegimentation;
using Whipstaff.CommandLine;
using Xunit;
using Xunit.Abstractions;

namespace Whipstaff.UnitTests.CommandLine
{
    /// <summary>
    /// Unit Tests for <see cref="Whipstaff.CommandLine.ArgumentResultHelpers"/>.
    /// </summary>
    public static class ArgumentResultHelpersTests
    {
        /// <summary>
        /// Unit Tests for <see cref="Whipstaff.CommandLine.ArgumentResultHelpers.FileHasSupportedExtension(ArgumentResult, string)"/>.
        /// </summary>
        public sealed class FileHasSupportedExtensionMethod
            : Foundatio.Xunit.TestWithLoggingBase,
                ITestMethodWithNullableParameters<ArgumentResult, string>
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
            [ClassData(typeof(Whipstaff.UnitTests.TestSources.CommandLine.ArgumentResultHelpersTests.FileHasSupportedExtensionMethod.ThrowsArgumentNullExceptionTestSource))]
            [Theory]
            public void ThrowsArgumentNullException(
                ArgumentResult arg1,
                string arg2,
                string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => ArgumentResultHelpers.FileHasSupportedExtension(arg1, arg2));
            }

            /// <summary>
            /// Test to ensure that the method returns an instance on a successful run.
            /// </summary>
            [Fact]
            public void ReturnsErrorMessageForUnsupportedExtension()
            {
                var argument1Builder = new Argument<string>("filename");
                var filename = "somefilename.cer";
                var argumentResult = argument1Builder.Parse(filename).FindResultFor(argument1Builder);

                Assert.NotNull(argumentResult);
                var extension = ".txt";

                ArgumentResultHelpers.FileHasSupportedExtension(argumentResult, extension);

                Assert.Equal(
                    argumentResult.ErrorMessage,
                    $"Filename \"{filename}\" does not have a supported extension of \"{extension}\".");
            }

            /// <summary>
            /// Test to ensure that the method returns an instance on a successful run.
            /// </summary>
            [Fact]
            public void SucceedsForSupportedExtension()
            {
                var argument1Builder = new Argument<string>("filename");
                var argumentResult = argument1Builder.Parse("somefilename.txt")
                    .FindResultFor(argument1Builder);

                Assert.NotNull(argumentResult);
                var extension = ".txt";

                ArgumentResultHelpers.FileHasSupportedExtension(
                    argumentResult,
                    extension);

                Assert.Null(argumentResult.ErrorMessage);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="Whipstaff.CommandLine.ArgumentResultHelpers.FileHasSupportedExtension(ArgumentResult, string)"/>.
        /// </summary>
        public sealed class FileHasSupportedExtension2Method
            : Foundatio.Xunit.TestWithLoggingBase,
                ITestMethodWithNullableParameters<ArgumentResult, string[]>
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
            [ClassData(typeof(Whipstaff.UnitTests.TestSources.CommandLine.ArgumentResultHelpersTests.FileHasSupportedExtension2Method.ThrowsArgumentNullExceptionTestSource))]
            [Theory]
            public void ThrowsArgumentNullException(
                ArgumentResult arg1,
                string[] arg2,
                string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => ArgumentResultHelpers.FileHasSupportedExtension(arg1, arg2));
            }

            /// <summary>
            /// Test to ensure that the method returns an instance on a successful run.
            /// </summary>
            [Fact]
            public void ReturnsErrorMessageForUnsupportedExtension()
            {
                var argument1Builder = new Argument<string>("filename");
                var filename = "somefilename.cer";
                var argumentResult = argument1Builder.Parse(filename).FindResultFor(argument1Builder);

                Assert.NotNull(argumentResult);
                var extensions = new[] { ".txt", ".docx" };

                ArgumentResultHelpers.FileHasSupportedExtension(argumentResult, extensions);

                Assert.Equal(
                    argumentResult.ErrorMessage,
                    $"Filename \"{filename}\" does not have a supported extension of \"{string.Join(",", extensions)}\".");
            }

            /// <summary>
            /// Test to ensure that the method returns an instance on a successful run.
            /// </summary>
            [Fact]
            public void SucceedsForSupportedExtension()
            {
                var argument1Builder = new Argument<string>("filename");
                var argumentResult = argument1Builder.Parse("somefilename.txt")
                    .FindResultFor(argument1Builder);

                Assert.NotNull(argumentResult);
                var extension = new[] { ".txt", ".docx" };

                ArgumentResultHelpers.FileHasSupportedExtension(
                    argumentResult,
                    extension);

                Assert.Null(argumentResult.ErrorMessage);
            }
        }
    }
}
