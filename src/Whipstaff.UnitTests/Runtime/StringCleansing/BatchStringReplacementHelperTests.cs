// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Whipstaff.Runtime.StringCleansing;
using Xunit;

namespace Whipstaff.UnitTests.Runtime.StringCleansing
{
    /// <summary>
    /// Unit tests for the <see cref="BatchStringReplacementHelper"/> class.
    /// </summary>
    public static class BatchStringReplacementHelperTests
    {
        /// <summary>
        /// Unit tests for the <see cref="Whipstaff.Runtime.StringCleansing.BatchStringReplacementHelper.Replace"/> method.
        /// </summary>
        public sealed class ReplaceMethod
        {
            /// <summary>
            /// Tests that the method returns the expected result.
            /// </summary>
            [Fact]
            public void ReturnsResultOnSuccess()
            {
                var instance = BatchStringReplacementHelper.GetMicrosoftWordSmartQuoteReplacements();
                var result = instance.Replace("\u2026\u0092\u2014\u0085");
                var result2 = instance.Replace("\u2026\u0092\u2014\u0085");

                Assert.NotNull(result);
                Assert.Equal("...'-...", result);
                Assert.NotNull(result2);
                Assert.Equal("...'-...", result2);
            }
        }
    }
}
