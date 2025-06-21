// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.CommandLine;
using System.IO;
using Microsoft.Extensions.Logging;
using Whipstaff.CommandLine.DocumentationGenerator;
using Whipstaff.Testing.Logging;
using Xunit;

namespace Whipstaff.UnitTests.CommandLine.DocumentationGenerator
{
    /// <summary>
    /// Unit tests for the <see cref="MarkdownDocumentationGenerator"/>.
    /// </summary>
    public static class MarkdownDocumentationGeneratorTests
    {
        /// <summary>
        /// Unit test for the <see cref="MarkdownDocumentationGenerator.GenerateDocumentation"/> method.
        /// </summary>
        public sealed class GenerateDocumentationMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GenerateDocumentationMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public GenerateDocumentationMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure documentation is generated.
            /// </summary>
            [Fact]
            public void GeneratesDocumentation()
            {
                var rootCommand = new RootCommand();
                rootCommand.Arguments.Add(new Argument<FileInfo>("filename"));
                rootCommand.Arguments.Add(new Argument<string?>("name"));

                var result = MarkdownDocumentationGenerator.GenerateDocumentation(rootCommand);

                Logger.LogInformation(result);

                Assert.NotNull(result);
                Assert.NotEmpty(result);
            }
        }
    }
}
