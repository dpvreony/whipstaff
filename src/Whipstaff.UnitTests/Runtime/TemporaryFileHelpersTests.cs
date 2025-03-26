// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Whipstaff.Runtime.FileProcessing;
using Xunit;

namespace Whipstaff.UnitTests.Runtime
{
    /// <summary>
    /// Unit Tests for <see cref="TemporaryFileHelpers"/>.
    /// </summary>
    public static class TemporaryFileHelpersTests
    {
        /// <summary>
        /// Unit Tests for <see cref="TemporaryFileHelpers.WithTempFile"/>.
        /// </summary>
        public sealed class WithTempFileMethod : NetTestRegimentation.ITestMethodWithNullableParameters<byte[], string, Action<string>>
        {
            /// <inheritdoc />
            [Theory]
            [ClassData(typeof(Whipstaff.UnitTests.TestSources.Runtime.TemporaryFileHelpersTests.WithTempFileMethod.ThrowsArgumentNullExceptionTestSource))]
            public void ThrowsArgumentNullException(
                byte[]? arg1,
                string? arg2,
                Action<string>? arg3,
                string expectedParameterNameForException)
            {
                var exception = Assert.Throws<ArgumentNullException>(() => TemporaryFileHelpers.WithTempFile(
                    arg1!,
                    arg2!,
                    arg3!,
                    false));

                Assert.Equal(expectedParameterNameForException, exception.ParamName);
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="TemporaryFileHelpers.WithTempFile{TResult}"/>.
        /// </summary>
        public sealed class WithTempFileT1Method : NetTestRegimentation.ITestMethodWithNullableParameters<byte[], string, Func<string, int>>
        {
            /// <inheritdoc />
            [Theory]
            [ClassData(typeof(Whipstaff.UnitTests.TestSources.Runtime.TemporaryFileHelpersTests.WithTempFileT1Method.ThrowsArgumentNullExceptionTestSource))]
            public void ThrowsArgumentNullException(
                byte[]? arg1,
                string? arg2,
                Func<string, int>? arg3,
                string expectedParameterNameForException)
            {
                var exception = Assert.Throws<ArgumentNullException>(() => TemporaryFileHelpers.WithTempFile(
                    arg1!,
                    arg2!,
                    arg3!,
                    false));

                Assert.Equal(expectedParameterNameForException, exception.ParamName);
            }
        }
    }
}
