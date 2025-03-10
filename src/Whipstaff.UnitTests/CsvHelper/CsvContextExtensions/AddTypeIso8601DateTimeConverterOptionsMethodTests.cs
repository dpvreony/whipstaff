// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using Microsoft.Extensions.Logging;
using Whipstaff.CsvHelper;
using Whipstaff.Testing.Logging;
using Xunit;

namespace Whipstaff.UnitTests.CsvHelper.CsvContextExtensions
{
    /// <summary>
    /// Unit tests for the <see cref="Whipstaff.CsvHelper.CsvContextExtensions.AddTypeIso8601DateTimeConverterOptions"/> method.
    /// </summary>
    public sealed class AddTypeIso8601DateTimeConverterOptionsMethodTests : TestWithLoggingBase, NetTestRegimentation.ITestMethodWithNullableParameters<CsvContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddTypeIso8601DateTimeConverterOptionsMethodTests"/> class.
        /// </summary>
        /// <param name="output">XUnit logging output helper.</param>
        public AddTypeIso8601DateTimeConverterOptionsMethodTests(ITestOutputHelper output)
            : base(output)
        {
        }

        /// <inheritdoc />
        [Theory]
        [ClassData(typeof(TestSources.CsvHelper.CsvContextExtensions.ThrowsArgumentNullExceptionTestSource))]
        public void ThrowsArgumentNullException(CsvContext arg, string expectedParameterNameForException)
        {
            _ = Assert.Throws<ArgumentNullException>(expectedParameterNameForException, () => Whipstaff.CsvHelper.CsvContextExtensions.AddTypeIso8601DateTimeConverterOptions(arg));
        }

        /// <summary>
        /// Succeeds when called with a valid argument.
        /// </summary>
        [Fact]
        public void Succeeds()
        {
            var now = DateTime.Now;
            var stringWriter = new StringWriter();
            using (var csvWriter = new CsvWriter(stringWriter, CultureInfo.InvariantCulture))
            {
                csvWriter.Context.AddTypeIso8601DateTimeConverterOptions();
                Assert.NotNull(csvWriter.Context.TypeConverterOptionsCache.GetOptions<DateOnly>());
                Assert.NotNull(csvWriter.Context.TypeConverterOptionsCache.GetOptions<DateTime>());
                csvWriter.WriteField(DateOnly.FromDateTime(now));
                csvWriter.WriteField(now);
            }

            var actual = stringWriter.ToString();
            _logger.LogInformation(stringWriter.ToString());

            var expected = $"{DateOnly.FromDateTime(now):yyyy-MM-dd},{now:O}";

            Assert.Equal(
                expected,
                actual);
        }
    }
}
