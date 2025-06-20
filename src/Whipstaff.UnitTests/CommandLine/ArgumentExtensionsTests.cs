﻿// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using NetTestRegimentation;
using Whipstaff.CommandLine;
using Whipstaff.Testing.Logging;
using Xunit;

namespace Whipstaff.UnitTests.CommandLine
{
    /// <summary>
    /// Unit Tests for <see cref="Whipstaff.CommandLine.ArgumentExtensions"/>.
    /// </summary>
    public static class ArgumentExtensionsTests
    {
        /// <summary>
        /// Unit Tests for <see cref="Whipstaff.CommandLine.ArgumentExtensions.SpecificFileExtensionOnly"/>.
        /// </summary>
        public sealed class SpecificFileExtensionOnlyMethod
            : TestWithLoggingBase,
                ITestMethodWithNullableParameters<IFileSystem, string>
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
            [ClassData(typeof(Whipstaff.UnitTests.TestSources.CommandLine.ArgumentExtensionsTests.SpecificFileExtensionOnlyMethod.ThrowsArgumentNullExceptionTestSource))]
            [Theory]
            public void ThrowsArgumentNullException(
                IFileSystem? arg1,
                string? arg2,
                string expectedParameterNameForException)
            {
                var instance = new Argument<IFileInfo>("--arg");

                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => instance.SpecificFileExtensionOnly(arg1!, arg2!));
            }

            /// <summary>
            /// Test to ensure that the method returns an instance on a successful run.
            /// </summary>
            [Fact]
            public void ReturnsInstance()
            {
                var instance = new Argument<IFileInfo>("--arg");
                var fileSystem = new MockFileSystem();
                var result = instance.SpecificFileExtensionOnly(fileSystem, ".txt");

                Assert.NotNull(result);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="Whipstaff.CommandLine.ArgumentExtensions.SpecificFileExtensionsOnly"/>.
        /// </summary>
        public sealed class SpecificFileExtensionsOnlyMethod
            : TestWithLoggingBase,
                ITestMethodWithNullableParameters<IFileSystem, string[]>
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
            [ClassData(typeof(Whipstaff.UnitTests.TestSources.CommandLine.ArgumentExtensionsTests.SpecificFileExtensionsOnlyMethod.ThrowsArgumentNullExceptionTestSource))]
            [Theory]
            public void ThrowsArgumentNullException(
                IFileSystem? arg1,
                string[]? arg2,
                string expectedParameterNameForException)
            {
                var instance = new Argument<IFileInfo>("--arg");

                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => instance.SpecificFileExtensionsOnly(arg1!, arg2!));
            }

            /// <summary>
            /// Test to ensure that the method returns an instance on a successful run.
            /// </summary>
            [Fact]
            public void ReturnsInstance()
            {
                var instance = new Argument<IFileInfo>("--arg");
                var fileSystem = new MockFileSystem();
                var result = instance.SpecificFileExtensionsOnly(fileSystem, _extensions);

                Assert.NotNull(result);
            }
        }
    }
}
