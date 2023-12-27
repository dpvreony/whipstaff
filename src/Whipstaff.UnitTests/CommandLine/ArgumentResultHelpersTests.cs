// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine.Parsing;
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
        }
    }
}
