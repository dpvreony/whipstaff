// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using Whipstaff.CsvHelper;
using Xunit;

namespace Whipstaff.UnitTests.CsvHelper.CsvReaderExtensions
{
    /// <summary>
    /// Unit tests for the <see cref="Whipstaff.CsvHelper.CsvReaderExtensions.AddTypeIso8601DateTimeConverterOptions"/> method.
    /// </summary>
    public sealed class AddTypeIso8601DateTimeConverterOptionsMethodTests : NetTestRegimentation.ITestMethodWithNullableParameters<CsvReader>
    {
        /// <inheritdoc />
        [Theory]
        [ClassData(typeof(TestSources.CsvHelper.CsvReaderExtensions.ThrowsArgumentNullExceptionTestSource))]
        public void ThrowsArgumentNullException(CsvReader? arg, string expectedParameterNameForException)
        {
            _ = Assert.Throws<ArgumentNullException>(expectedParameterNameForException, () => Whipstaff.CsvHelper.CsvReaderExtensions.AddTypeIso8601DateTimeConverterOptions(arg));
        }

        /// <summary>
        /// Succeeds when called with a valid argument.
        /// </summary>
        [Fact]
        public void Succeeds()
        {
            var stringReader = new StringReader("2024-06-22,2024-06-22T10:29:14.0466273+01:00");
            using (var csvReader = new CsvReader(stringReader, CultureInfo.InvariantCulture))
            {
                csvReader.AddTypeIso8601DateTimeConverterOptions();
                Assert.NotNull(csvReader.Context.TypeConverterOptionsCache.GetOptions<DateOnly>());
                Assert.NotNull(csvReader.Context.TypeConverterOptionsCache.GetOptions<DateTime>());

                Assert.True(csvReader.Read());
                Assert.Equal(new DateOnly(2024, 6, 22), csvReader.GetField<DateOnly>(0));
                Assert.Equal(DateTime.Parse("2024-06-22T10:29:14.0466273+01:00", CultureInfo.InvariantCulture), csvReader.GetField<DateTime>(1));
            }
        }
    }
}
