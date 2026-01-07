// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Playwright;
using NetTestRegimentation;
using NetTestRegimentation.XUnit.Logging;
using NetTestRegimentation.XUnit.Theories.ArgumentNullException;
using Whipstaff.Playwright;
using Whipstaff.Testing.Logging;
using Xunit;

namespace Whipstaff.UnitTests.Playwright
{
    /// <summary>
    /// Unit Tests for <see cref="PlaywrightExtensions"/>.
    /// </summary>
    public static class PlaywrightExtensionsTests
    {
        /// <summary>
        /// Unit tests for the <see cref="PlaywrightExtensions.GetBrowserType"/> method.
        /// </summary>
        public sealed class GetBrowserTypeMethod : TestWithLoggingBase, ITestMethodWithNullableParameters<IPlaywright>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetBrowserTypeMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output instance.</param>
            public GetBrowserTypeMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc />
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionTestSource))]
            public void ThrowsArgumentNullException(
                IPlaywright? arg,
                string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => arg!.GetBrowserType(PlaywrightBrowserType.Chromium));
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullException"/>.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionTestSource : ArgumentNullExceptionTheoryData<IPlaywright>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionTestSource()
                    : base("playwright")
                {
                }
            }
        }
    }
}
