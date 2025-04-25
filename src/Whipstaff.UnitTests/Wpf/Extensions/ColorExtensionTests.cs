// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Whipstaff.Wpf.Extensions;
using Xunit;

namespace Whipstaff.UnitTests.Wpf.Extensions
{
    /// <summary>
    /// Unit tests for <see cref="ColorExtensions"/>.
    /// </summary>
    public static class ColorExtensionTests
    {
        /// <summary>
        /// Unit Tests for <see cref="ColorExtensions.IsLightColor(global::Windows.UI.Color)"/>.
        /// </summary>
        public sealed class IsLightColorMethod
        {
            /// <summary>
            /// Test that the method returns the expected value.
            /// </summary>
            /// <param name="color">The color to test.</param>
            /// <param name="expectedResult">The expected result.</param>
            [Theory]
            [ClassData(typeof(ReturnsValueTestSource))]
            public void ReturnsValue(global::Windows.UI.Color color, bool expectedResult)
            {
                Assert.Equal(expectedResult, color.IsLightColor());
            }
        }

        /// <summary>
        /// Test data for <see cref="IsLightColorMethod"/>.
        /// </summary>
        public sealed class ReturnsValueTestSource : TheoryData<global::Windows.UI.Color, bool>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ReturnsValueTestSource"/> class.
            /// </summary>
            public ReturnsValueTestSource()
            {
                Add(global::Windows.UI.Color.FromArgb(0, 0, 0, 0), false);
                Add(global::Windows.UI.Color.FromArgb(0, 255, 255, 255), true);
            }
        }
    }
}
