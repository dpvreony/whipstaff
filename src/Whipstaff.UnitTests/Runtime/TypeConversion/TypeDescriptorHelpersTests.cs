// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;
using Whipstaff.Runtime.TypeConversion;
using Xunit;

namespace Whipstaff.UnitTests.Runtime.TypeConversion
{
    /// <summary>
    /// Unit Tests for <see cref="TypeDescriptorHelpers"/>.
    /// </summary>
    public static class TypeDescriptorHelpersTests
    {
        /// <summary>
        /// Unit Tests for <see cref="TypeDescriptorHelpers.AddConverter{TTarget,TConvertor}"/>.
        /// </summary>
        public sealed class AddConverterMethod
        {
            /// <summary>
            /// Test to ensure a converter is registered.
            /// </summary>
            [Fact]
            public void ShouldAddConvertor()
            {
                var result = TypeDescriptorHelpers.AddConverter<SomeType, SomeTypeConverter>();
                Assert.NotNull(result);

                Assert.True(result.IsSupportedType(typeof(SomeType)));
            }
        }

        /// <summary>
        /// Fake type for the converter test.
        /// </summary>
        public sealed class SomeType
        {
        }

        /// <summary>
        /// Fake TypeConverter for the type.
        /// </summary>
        public sealed class SomeTypeConverter : TypeConverter
        {
        }
    }
}
