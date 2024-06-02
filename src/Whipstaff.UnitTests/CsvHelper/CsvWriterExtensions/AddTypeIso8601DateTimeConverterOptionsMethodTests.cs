// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using Whipstaff.CsvHelper;
using Xunit;

namespace Whipstaff.UnitTests.CsvHelper.CsvWriterExtensions
{
    /// <summary>
    /// Unit tests for the <see cref="Whipstaff.CsvHelper.CsvWriterExtensions.AddTypeIso8601DateTimeConverterOptions"/> method.
    /// </summary>
    public sealed class AddTypeIso8601DateTimeConverterOptionsMethodTests : NetTestRegimentation.ITestMethodWithNullableParameters<CsvWriter>
    {
        /// <inheritdoc />
        [Theory]
        [ClassData(typeof(TestSources.CsvHelper.CsvWriterExtensions.ThrowsArgumentNullExceptionTestSource))]
        public void ThrowsArgumentNullException(CsvWriter arg, string expectedParameterNameForException)
        {
            _ = Assert.Throws<ArgumentNullException>(expectedParameterNameForException, () => Whipstaff.CsvHelper.CsvWriterExtensions.AddTypeIso8601DateTimeConverterOptions(arg));
        }

        /// <summary>
        /// Succeeds when called with a valid argument.
        /// </summary>
        [Fact]
        public void Succeeds()
        {
            using (var csvWriter = new CsvWriter(TextWriter.Null, CultureInfo.InvariantCulture))
            {
                csvWriter.AddTypeIso8601DateTimeConverterOptions();
                Assert.NotNull(csvWriter.Context.TypeConverterOptionsCache.GetOptions<DateOnly>());
                Assert.NotNull(csvWriter.Context.TypeConverterOptionsCache.GetOptions<DateTime>());
            }
        }
    }
}
