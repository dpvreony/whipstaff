// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.IO;
using NetTestRegimentation;
using Whipstaff.CommandLine;
using Xunit;
using Xunit.Abstractions;

namespace Whipstaff.UnitTests.CommandLine
{
    /// <summary>
    /// Unit Tests for <see cref="Whipstaff.CommandLine.OptionExtensions"/>.
    /// </summary>
    public static class OptionExtensionsTests
    {
        /// <summary>
        /// Unit Tests for <see cref="Whipstaff.CommandLine.OptionExtensions.SpecificFileExtensionOnly"/>.
        /// </summary>
        public sealed class SpecificFileExtensionOnlyMethod
            : Foundatio.Xunit.TestWithLoggingBase,
                ITestMethodWithNullableParameters<string>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SpecificFileExtensionOnlyMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public SpecificFileExtensionOnlyMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            [ClassData(typeof(Whipstaff.UnitTests.TestSources.CommandLine.OptionExtensionsTests.SpecificFileExtensionOnlyMethod.ThrowsArgumentNullExceptionTestSource))]
            [Theory]
            public void ThrowsArgumentNullException(
                string arg,
                string expectedParameterNameForException)
            {
                var instance = new Option<FileInfo>("someoption");

                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => instance.SpecificFileExtensionOnly(arg));
            }

            /// <summary>
            /// Test to ensure that the method returns an instance on a successful run.
            /// </summary>
            [Fact]
            public void ReturnsInstance()
            {
                var instance = new Option<FileInfo>("someoption");
                var result = instance.SpecificFileExtensionOnly(".txt");

                Assert.NotNull(result);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="Whipstaff.CommandLine.OptionExtensions.SpecificFileExtensionsOnly"/>.
        /// </summary>
        public sealed class SpecificFileExtensionsOnlyMethod
            : Foundatio.Xunit.TestWithLoggingBase,
                ITestMethodWithNullableParameters<string[]>
        {
            private static readonly string[] _extensions = [".txt", ".docx"];

            /// <summary>
            /// Initializes a new instance of the <see cref="SpecificFileExtensionsOnlyMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public SpecificFileExtensionsOnlyMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            [ClassData(typeof(Whipstaff.UnitTests.TestSources.CommandLine.OptionExtensionsTests.SpecificFileExtensionsOnlyMethod.ThrowsArgumentNullExceptionTestSource))]
            [Theory]
            public void ThrowsArgumentNullException(
                string[] arg,
                string expectedParameterNameForException)
            {
                var instance = new Option<FileInfo>("someoption");

                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => instance.SpecificFileExtensionsOnly(arg));
            }

            /// <summary>
            /// Test to ensure that the method returns an instance on a successful run.
            /// </summary>
            [Fact]
            public void ReturnsInstance()
            {
                var instance = new Option<FileInfo>("someoption");
                var result = instance.SpecificFileExtensionsOnly(_extensions);

                Assert.NotNull(result);
            }
        }
    }
}
