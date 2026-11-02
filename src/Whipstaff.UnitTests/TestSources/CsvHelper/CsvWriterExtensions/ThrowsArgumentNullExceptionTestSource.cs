// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using CsvHelper;
using NetTestRegimentation.XUnit.Theories.ArgumentNullException;

namespace Whipstaff.UnitTests.TestSources.CsvHelper.CsvWriterExtensions
{
    /// <summary>
    /// Test Source for <see cref="ArgumentNullException" /> test of <see cref="Whipstaff.CsvHelper.CsvWriterExtensions.AddTypeIso8601DateTimeConverterOptions"/>.
    /// </summary>
    public sealed class ThrowsArgumentNullExceptionTestSource : ArgumentNullExceptionTheoryData<CsvWriter>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
        /// </summary>
        public ThrowsArgumentNullExceptionTestSource()
            : base("csvWriter")
        {
        }
    }
}
