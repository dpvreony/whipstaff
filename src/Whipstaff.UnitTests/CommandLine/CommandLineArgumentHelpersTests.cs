// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
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
        }
    }
}
