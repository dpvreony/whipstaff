// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO.Abstractions;
using NetTestRegimentation;
using NetTestRegimentation.XUnit.Logging;
using Whipstaff.CommandLine.FileSystemAbstractions;
using Xunit;

namespace Whipstaff.UnitTests.CommandLine.FileSystemAbstractions
{
    /// <summary>
    /// Unit tests for the <see cref="Whipstaff.CommandLine.FileSystemAbstractions.OptionFactory"/> class.
    /// </summary>
    public static class OptionFactoryTests
    {
        /// <summary>
        /// Unit Tests for <see cref="Whipstaff.CommandLine.FileSystemAbstractions.OptionFactory.GetDirectoryInfoOption"/>.
        /// </summary>
        public sealed class GetDirectoryInfoOptionMethod
            : TestWithLoggingBase,
                ITestMethodWithNullableParameters<string, IFileSystem>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetDirectoryInfoOptionMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public GetDirectoryInfoOptionMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            [ClassData(typeof(Whipstaff.UnitTests.TestSources.CommandLine.FileSystemAbstractions.OptionFactoryTests.GetDirectoryInfoOptionMethod.ThrowsArgumentNullExceptionTestSource))]
            [Theory]
            public void ThrowsArgumentNullException(
                string? arg1,
                IFileSystem? arg2,
                string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => OptionFactory.GetDirectoryInfoOption(arg1!, arg2!));
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="Whipstaff.CommandLine.FileSystemAbstractions.OptionFactory.GetDriveInfoOption"/>.
        /// </summary>
        public sealed class GetDriveInfoOptionMethod
            : TestWithLoggingBase,
                ITestMethodWithNullableParameters<string, IFileSystem>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetDriveInfoOptionMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public GetDriveInfoOptionMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            [ClassData(typeof(Whipstaff.UnitTests.TestSources.CommandLine.FileSystemAbstractions.OptionFactoryTests.GetDriveInfoOptionMethod.ThrowsArgumentNullExceptionTestSource))]
            [Theory]
            public void ThrowsArgumentNullException(
                string? arg1,
                IFileSystem? arg2,
                string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => OptionFactory.GetDriveInfoOption(arg1!, arg2!));
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="Whipstaff.CommandLine.FileSystemAbstractions.OptionFactory.GetFileInfoOption"/>.
        /// </summary>
        public sealed class GetFileInfoOptionMethod
            : TestWithLoggingBase,
                ITestMethodWithNullableParameters<string, IFileSystem>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetFileInfoOptionMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public GetFileInfoOptionMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            [ClassData(typeof(Whipstaff.UnitTests.TestSources.CommandLine.FileSystemAbstractions.OptionFactoryTests.GetFileInfoOptionMethod.ThrowsArgumentNullExceptionTestSource))]
            [Theory]
            public void ThrowsArgumentNullException(
                string? arg1,
                IFileSystem? arg2,
                string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => OptionFactory.GetFileInfoOption(arg1!, arg2!));
            }
        }
    }
}
