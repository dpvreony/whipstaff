// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using NodaTime;
using Whipstaff.NodaTime;
using Whipstaff.Testing.Logging;
using Xunit;

namespace Whipstaff.UnitTests.NodaTime
{
    /// <summary>
    /// Unit Tests for <see cref="LocalDateExtensions"/>.
    /// </summary>
    public static class LocalDateExtensionsTests
    {
        /// <summary>
        /// Unit Tests for <see cref="LocalDateExtensions.ToYearMonthDayString"/>.
        /// </summary>
        public sealed class ToYearMonthDayStringMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ToYearMonthDayStringMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit Test Output helper.</param>
            public ToYearMonthDayStringMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Checks to ensure a valid year month day string is returned.
            /// </summary>
            [Fact]
            public void ReturnsDateString()
            {
                var instance = new LocalDate(2001, 12, 31);
                var output = instance.ToYearMonthDayString();

                Assert.NotNull(output);
                Assert.Equal("2001-12-31", output);
            }
        }
    }
}
