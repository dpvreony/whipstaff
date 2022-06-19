using NodaTime;
using Whipstaff.NodaTime;
using Xunit;
using Xunit.Abstractions;

namespace Whipstaff.UnitTests.NodaTime
{
    /// <summary>
    /// Unit Tests for <see cref="LocalDateExtensions"/>.
    /// </summary>
    public static class LocalDateExtensionsTests
    {
        /// <summary>
        /// Unit Tests for <see cref="LocalDateExtensions.ToYearMonthDayString"/>
        /// </summary>
        public sealed class ToYearMonthDayStringMethod : Foundatio.Xunit.TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ToYearMonthDayStringMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit Test Output helper.</param>

            public ToYearMonthDayStringMethod(ITestOutputHelper output) : base(output)
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
