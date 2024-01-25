// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Whipstaff.Runtime.Counters;
using Xunit;

namespace Whipstaff.UnitTests.Runtime.Counters
{
    /// <summary>
    /// Unit tests for the <see cref="IncrementOnlyInt"/> class.
    /// </summary>
    public static class IncrementOnlyIntTests
    {
        /// <summary>
        /// Unit test for the <see cref="IncrementOnlyInt"/> constructor.
        /// </summary>
        public sealed class ConstructorMethod
        {
            /// <summary>
            /// Tests that the constructor returns an instance.
            /// </summary>
            [Fact]
            public void ReturnsInstance()
            {
                var instance = new IncrementOnlyInt();
                Assert.NotNull(instance);
                Assert.Equal(0, instance.Value);
            }
        }

        /// <summary>
        /// Unit test for the <see cref="IncrementOnlyInt.Increment"/> method.
        /// </summary>
        public sealed class IncrementMethod
        {
            /// <summary>
            /// Tests that the method returns the incremented value.
            /// </summary>
            [Fact]
            public void ReturnsIncrementedValue()
            {
                var instance = new IncrementOnlyInt();
                var result = instance.Increment();
                Assert.Equal(1, result);
                Assert.Equal(1, instance.Value);
            }
        }
    }
}
