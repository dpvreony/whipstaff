// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO.Abstractions.TestingHelpers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetTestRegimentation;
using Whipstaff.CommandLine.MarkdownGen.DotNetTool;
using Whipstaff.CommandLine.MarkdownGen.DotNetTool.CommandLine;
using Whipstaff.Testing.Logging;
using Xunit;

namespace Whipstaff.UnitTests.CommandLine.MarkdownGen.DotNetTool
{
    /// <summary>
    /// Unit tests for the <see cref="CommandLineJob"/> class.
    /// </summary>
    public static class CommandLineJobTests
    {
        /// <summary>
        /// Unit test for <see cref="CommandLineJob"/> constructor.
        /// </summary>
        public sealed class ConstructorMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ConstructorMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public ConstructorMethod(ITestOutputHelper output)
                : base(output)
            {
            }
        }

        /// <summary>
        /// Unit test for <see cref="CommandLineJob.OnHandleCommandAsync(CommandLineArgModel, CancellationToken)"/> method.
        /// </summary>
        public sealed class HandleCommandMethod
            : TestWithLoggingBase,
                ITestAsyncMethodWithNullableParameters<CommandLineArgModel>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="HandleCommandMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public HandleCommandMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            [ClassData(typeof(Whipstaff.UnitTests.TestSources.CommandLine.MarkdownGen.DotNetTool.CommandLineJobTests.HandleCommand.ThrowsArgumentNullExceptionAsyncTestSource))]
            [Theory]
            public async Task ThrowsArgumentNullExceptionAsync(
                CommandLineArgModel? arg,
                string expectedParameterNameForException)
            {
                var logger = LoggerFactory.CreateLogger<CommandLineJob>();
                var instance = new CommandLineJob(
                    new CommandLineJobLogMessageActionsWrapper(
                        new CommandLineJobLogMessageActions(),
                        logger),
                    new MockFileSystem());

                _ = await Assert.ThrowsAsync<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => instance.HandleCommandAsync(arg!, CancellationToken.None));
            }
        }
    }
}
