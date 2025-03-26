// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Whipstaff.Playwright;
using Xunit;

namespace Whipstaff.UnitTests.Playwright
{
    /// <summary>
    /// Unit Tests for <see cref="InstallationHelper"/>.
    /// </summary>
    public static class InstallationHelperTests
    {
        /// <summary>
        /// Unit tests for the constructor.
        /// </summary>
        public sealed class ConstructorMethod
        {
            /// <summary>
            /// Tests that the constructor returns an instance when no browser is passed.
            /// </summary>
            [Fact]
            public void ReturnsInstance()
            {
                var instance = new InstallationHelper(PlaywrightBrowserType.None);
                Assert.NotNull(instance);
            }

            /// <summary>
            /// Tests that the constructor returns an instance when a specific browser is passed.
            /// </summary>
            [Fact]
            public void ReturnsInstanceWithBrowser()
            {
                var instance = new InstallationHelper(PlaywrightBrowserType.Chromium);
                Assert.NotNull(instance);
            }
        }
    }
}
