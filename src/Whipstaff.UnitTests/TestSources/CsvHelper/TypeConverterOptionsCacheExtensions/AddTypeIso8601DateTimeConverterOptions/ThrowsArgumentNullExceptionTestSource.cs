// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using CsvHelper.TypeConversion;
using NetTestRegimentation.XUnit.Theories.ArgumentNullException;

namespace Whipstaff.UnitTests.TestSources.CsvHelper.TypeConverterOptionsCacheExtensions.AddTypeIso8601DateTimeConverterOptions
{
    /// <summary>
    /// Test Source for <see cref="ArgumentNullException" /> test of <see cref="Whipstaff.CsvHelper.TypeConverterOptionsCacheExtensions.AddTypeIso8601DateTimeConverterOptions"/>.
    /// </summary>
    public sealed class ThrowsArgumentNullExceptionTestSource : ArgumentNullExceptionTheoryData<TypeConverterOptionsCache>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
        /// </summary>
        public ThrowsArgumentNullExceptionTestSource()
            : base("typeConverterOptionsCache")
        {
        }
    }
}
