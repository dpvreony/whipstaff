// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.TypeConversion;
using Whipstaff.CsvHelper;
using Xunit;

namespace Whipstaff.UnitTests.CsvHelper.TypeConverterOptionsCacheExtensions
{
    /// <summary>
    /// Unit tests for the <see cref="Whipstaff.CsvHelper.TypeConverterOptionsCacheExtensions.AddTypeIso8601DateTimeConverterOptions"/> method.
    /// </summary>
    public sealed class AddTypeIso8601DateTimeConverterOptionsMethodTests : NetTestRegimentation.ITestMethodWithNullableParameters<TypeConverterOptionsCache>
    {
        /// <inheritdoc />
        [Theory]
        [ClassData(typeof(TestSources.CsvHelper.TypeConverterOptionsCacheExtensions.ThrowsArgumentNullExceptionTestSource))]
        public void ThrowsArgumentNullException(TypeConverterOptionsCache? arg, string expectedParameterNameForException)
        {
            _ = Assert.Throws<ArgumentNullException>(expectedParameterNameForException, () => Whipstaff.CsvHelper.TypeConverterOptionsCacheExtensions.AddTypeIso8601DateTimeConverterOptions(arg!));
        }

        /// <summary>
        /// Succeeds when called with a valid argument.
        /// </summary>
        [Fact]
        public void Succeeds()
        {
            using (var csvWriter = new CsvWriter(TextWriter.Null, CultureInfo.InvariantCulture))
            {
                csvWriter.Context.TypeConverterOptionsCache.AddTypeIso8601DateTimeConverterOptions();
                Assert.NotNull(csvWriter.Context.TypeConverterOptionsCache.GetOptions<DateOnly>());
                Assert.NotNull(csvWriter.Context.TypeConverterOptionsCache.GetOptions<DateTime>());
            }
        }
    }
}
