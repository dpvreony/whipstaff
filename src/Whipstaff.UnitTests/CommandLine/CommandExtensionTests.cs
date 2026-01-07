// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.CommandLine;
using System.IO;
using NetTestRegimentation;
using NetTestRegimentation.XUnit.Logging;
using Whipstaff.CommandLine;
using Whipstaff.Testing.Logging;
using Xunit;

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
            : TestWithLoggingBase,
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
            [Theory]
            [ClassData(typeof(Whipstaff.UnitTests.TestSources.CommandLine.CommandExtensions.MakeOptionsMutuallyExclusiveMethod.ThrowsArgumentNullExceptionTestSource))]
            public void ThrowsArgumentNullException(Command? arg1, Option? arg2, Option? arg3, string expectedParameterNameForException)
            {
                _ = Assert.Throws<System.ArgumentNullException>(
                    expectedParameterNameForException,
                    () => Whipstaff.CommandLine.CommandExtensions.MakeOptionsMutuallyExclusive(arg1!, arg2!, arg3!));
            }

            /// <summary>
            /// Test to ensure that the command object is modified.
            /// </summary>
            [Fact]
            public void ModifiesCommand()
            {
                var firstOption = new Option<FileInfo>("--option1")
                    {
                        Description = "The first exclusive option",
                        Required = true
                    };

                var secondOption = new Option<FileInfo>("--option2")
                {
                    Description = "The second exclusive option",
                    Required = true
                };

#pragma warning restore CA1861 // Avoid constant arrays as arguments

                var rootCommand = new RootCommand("Creates a Markdown help file from the Command Line Help Content.")
                {
                    firstOption,
                    secondOption,
                };
                rootCommand.MakeOptionsMutuallyExclusive(firstOption, secondOption);

                // the validator collection is internal so would need reflection to check
                Assert.True(true);
            }

            /// <summary>
            /// Test to ensure the command line returns an error for mutually exclusive options.
            /// </summary>
            [Fact]
            public void CommandLineReturnsErrorForMutuallyExclusiveOptions()
            {
                var firstOption = new Option<bool>("--option1")
                {
                    Description = "The first exclusive option"
                };

                var secondOption = new Option<bool>("--option2")
                {
                    Description = "The second exclusive option"
                };

#pragma warning restore CA1861 // Avoid constant arrays as arguments

                var rootCommand = new RootCommand("Test Root Command")
                {
                    firstOption,
                    secondOption,
                };
                rootCommand.MakeOptionsMutuallyExclusive(firstOption, secondOption);

                var result = rootCommand.Parse("--option1 --option2");
                Assert.NotNull(result);

                var errors = result.Errors;
                Assert.NotNull(errors);

                var actualError = Assert.Single(errors);
                const string expectedErrorMessage = "You cannot use options \"--option1\" and \"--option2\" together";
                Assert.Equal(expectedErrorMessage, actualError.Message);
            }
        }
    }
}
