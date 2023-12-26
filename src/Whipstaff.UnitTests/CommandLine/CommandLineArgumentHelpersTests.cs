// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using NetTestRegimentation;
using Whipstaff.CommandLine;
using Whipstaff.Testing.CommandLine;
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
                ITestMethodWithNullableParameters<string[], Func<RootCommandAndBinderModel<FakeCommandLineArgModelBinder>>, Func<FakeCommandLineArgModel, Task<int>>>
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
            public void ThrowsArgumentNullException(
                string[] arg1,
                Func<RootCommandAndBinderModel<FakeCommandLineArgModelBinder>> arg2,
                Func<FakeCommandLineArgModel, Task<int>> arg3,
                string expectedParameterNameForException)
            {
            }
        }
    }
}
