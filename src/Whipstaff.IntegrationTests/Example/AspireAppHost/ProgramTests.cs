// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Whipstaff.Example.AspireAppHost;
using Xunit;

namespace Whipstaff.IntegrationTests.Example.AspireAppHost
{
    /// <summary>
    /// Integration tests for <see cref="Program"/>.
    /// </summary>
    public static class ProgramTests
    {
        /// <summary>
        /// Integration Tests for the <see cref="Program.GetApplication"/> method.
        /// </summary>
        public sealed class GetApplicationMethod
        {
            /// <summary>
            /// Tests that the method returns an instance.
            /// </summary>
            [Fact]
            public void ReturnsInstance()
            {
                string[] args = [];

                var result = Program.GetApplication(args);

                Assert.NotNull(result);
            }
        }
    }
}
