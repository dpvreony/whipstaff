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
    /// Unit tests for the <see cref="Whipstaff.CommandLine.FileSystemAbstractions.ArgumentFactory"/> class.
    /// </summary>
    public static class ArgumentFactoryTests
    {
        /// <summary>
        /// Unit Tests for <see cref="Whipstaff.CommandLine.FileSystemAbstractions.ArgumentFactory.GetDirectoryInfoArgument"/>.
        /// </summary>
        public sealed class GetDirectoryInfoArgumentMethod
            : TestWithLoggingBase,
                ITestMethodWithNullableParameters<string, IFileSystem>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetDirectoryInfoArgumentMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public GetDirectoryInfoArgumentMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            [ClassData(typeof(Whipstaff.UnitTests.TestSources.CommandLine.FileSystemAbstractions.ArgumentFactoryTests.GetDirectoryInfoArgumentMethod.ThrowsArgumentNullExceptionTestSource))]
            [Theory]
            public void ThrowsArgumentNullException(
                string? arg1,
                IFileSystem? arg2,
                string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => ArgumentFactory.GetDirectoryInfoArgument(arg1!, arg2!));
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="Whipstaff.CommandLine.FileSystemAbstractions.ArgumentFactory.GetDriveInfoArgument"/>.
        /// </summary>
        public sealed class GetDriveInfoArgumentMethod
            : TestWithLoggingBase,
                ITestMethodWithNullableParameters<string, IFileSystem>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetDriveInfoArgumentMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public GetDriveInfoArgumentMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            [ClassData(typeof(Whipstaff.UnitTests.TestSources.CommandLine.FileSystemAbstractions.ArgumentFactoryTests.GetDriveInfoArgumentMethod.ThrowsArgumentNullExceptionTestSource))]
            [Theory]
            public void ThrowsArgumentNullException(
                string? arg1,
                IFileSystem? arg2,
                string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => ArgumentFactory.GetDriveInfoArgument(arg1!, arg2!));
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="Whipstaff.CommandLine.FileSystemAbstractions.ArgumentFactory.GetFileInfoArgument"/>.
        /// </summary>
        public sealed class GetFileInfoArgumentMethod
            : TestWithLoggingBase,
                ITestMethodWithNullableParameters<string, IFileSystem>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetFileInfoArgumentMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public GetFileInfoArgumentMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            [ClassData(typeof(Whipstaff.UnitTests.TestSources.CommandLine.FileSystemAbstractions.ArgumentFactoryTests.GetFileInfoArgumentMethod.ThrowsArgumentNullExceptionTestSource))]
            [Theory]
            public void ThrowsArgumentNullException(
                string? arg1,
                IFileSystem? arg2,
                string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => ArgumentFactory.GetFileInfoArgument(arg1!, arg2!));
            }
        }
    }
}
