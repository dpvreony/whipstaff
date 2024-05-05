// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.CommandLine;
using System.IO.Abstractions.TestingHelpers;
using System.IO;
using NetTestRegimentation;
using Xunit;
using Xunit.Abstractions;

namespace Whipstaff.UnitTests.CommandLine
{
    /// <summary>
    /// Unit tests for the <see cref="Whipstaff.CommandLine.CommandExtensions"/> class.
    /// </summary>
    public static class CommandExtensionTests
    {
        /// <summary>
        /// Unit tests for the <see cref="Whipstaff.CommandLine.CommandExtensions.MakeOptionsMutuallyExclusive"/> method.
        /// </summary>
        public sealed class MakeOptionsMutuallyExclusiveMethod
            : Foundatio.Xunit.TestWithLoggingBase,
                ITestMethodWithNullableParameters<Command, Option, Option>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="MakeOptionsMutuallyExclusiveMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public MakeOptionsMutuallyExclusiveMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            [Fact]
            public void ThrowsArgumentNullException(Command arg1, Option arg2, Option arg3, string expectedParameterNameForException)
            {
            }

            /// <summary>
            /// Test to ensure that the command object is modified.
            /// </summary>
            [Fact]
            public void ModifiesCommand()
            {
            }

            /// <summary>
            /// Test to ensure the command line returns an error for mutually exclusive options.
            /// </summary>
            [Fact]
            public void CommandLineReturnsErrorForMutuallyExclusiveOptions()
            {
            }
        }
    }
}
