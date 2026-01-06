// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reflection;
using Whipstaff.Runtime.Extensions;
using Xunit;

namespace Whipstaff.UnitTests.Runtime.Extensions
{
    /// <summary>
    /// Unit tests for <see cref="Whipstaff.Runtime.Extensions.AssemblyExtensions"/>.
    /// </summary>
    public static class AssemblyExtensionsTests
    {
        /// <summary>
        /// Unit test for <see cref="Whipstaff.Runtime.Extensions.AssemblyExtensions.LoadStringFromResource(Assembly, string, string)"/>.
        /// </summary>
        public sealed class LoadStringFromResource
        {
            /// <summary>
            /// Tests that the method returns a string on success.
            /// </summary>
            [Fact]
            public void ReturnsResultOnSuccess()
            {
                var result = Assembly.GetExecutingAssembly()
                    .LoadStringFromResource(
                        "Whipstaff.UnitTests.Resources",
                        "test.txt");

                Assert.NotNull(result);
            }
        }

        /// <summary>
        /// Unit test for <see cref="Whipstaff.Runtime.Extensions.AssemblyExtensions.GetManifestResourceStreamAsByteArray(Assembly, string, string)"/>.
        /// </summary>
        public sealed class GetManifestResourceStreamAsByteArrayMethodWithAssemblyThenStringAndStringArgs
        {
            /// <summary>
            /// Tests that the method returns a byte array on success.
            /// </summary>
            [Fact]
            public void ReturnsResultOnSuccess()
            {
                var byteArray = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStreamAsByteArray(
                        "Whipstaff.UnitTests.Resources",
                        "1x1.jpg");

                Assert.NotNull(byteArray);
            }
        }

        /// <summary>
        /// Unit test for <see cref="Whipstaff.Runtime.Extensions.AssemblyExtensions.GetManifestResourceStreamAsByteArray(Assembly, string)"/>.
        /// </summary>
        public sealed class GetManifestResourceStreamAsByteArrayMethodWithAssemblyAndStringArgs
        {
            /// <summary>
            /// Tests that the method returns a byte array on success.
            /// </summary>
            [Fact]
            public void ReturnsResultOnSuccess()
            {
                var byteArray = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStreamAsByteArray("Whipstaff.UnitTests.Resources.1x1.jpg");

                Assert.NotNull(byteArray);
            }
        }
    }
}
