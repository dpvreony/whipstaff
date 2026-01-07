// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using NetTestRegimentation;
using NetTestRegimentation.XUnit.Logging;
using Whipstaff.CommandLine.MarkdownGen.DotNetTool.CommandLine;
using Xunit;

namespace Whipstaff.UnitTests.CommandLine.MarkdownGen.DotNetTool.CommandLine
{
    /// <summary>
    /// Unit tests for the <see cref="CommandLineHandlerFactory"/> class.
    /// </summary>
    public static class CommandLineHandlerFactoryTests
    {
        /// <summary>
        /// Tests for the <see cref="CommandLineHandlerFactory.GetRootCommandAndBinder"/> method.
        /// </summary>
        public sealed class GetRootCommandAndBinderMethod
            : TestWithLoggingBase,
                ITestMethodWithNullableParameters<IFileSystem>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetRootCommandAndBinderMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public GetRootCommandAndBinderMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            [ClassData(typeof(Whipstaff.UnitTests.TestSources.CommandLine.MarkdownGen.DotNetTool.CommandLine.CommandLineHandlerFactoryTests.ThrowsArgumentNullExceptionTestSource))]
            [Theory]
            public void ThrowsArgumentNullException(
                IFileSystem? arg,
                string expectedParameterNameForException)
            {
                var sut = new CommandLineHandlerFactory();
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => sut.GetRootCommandAndBinder(arg!));
            }

            /// <summary>
            /// Test to ensure that the method returns an instance on a successful run.
            /// </summary>
            [Fact]
            public void ReturnsInstance()
            {
                var sut = new CommandLineHandlerFactory();
                var fileSystem = new MockFileSystem();
                var result = sut.GetRootCommandAndBinder(fileSystem);
                Assert.NotNull(result);
            }
        }
    }
}
