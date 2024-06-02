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
        public void ThrowsArgumentNullException(CsvReader arg, string expectedParameterNameForException)
        {
            _ = Assert.Throws<ArgumentNullException>(expectedParameterNameForException, () => Whipstaff.CsvHelper.CsvReaderExtensions.AddTypeIso8601DateTimeConverterOptions(arg));
        }

        /// <summary>
        /// Succeeds when called with a valid argument.
        /// </summary>
        [Fact]
        public void Succeeds()
        {
            using (var csvReader = new CsvReader(TextReader.Null, CultureInfo.InvariantCulture))
            {
                csvReader.AddTypeIso8601DateTimeConverterOptions();
                Assert.NotNull(csvReader.Context.TypeConverterOptionsCache.GetOptions<DateOnly>());
                Assert.NotNull(csvReader.Context.TypeConverterOptionsCache.GetOptions<DateTime>());
            }
        }
    }
}
