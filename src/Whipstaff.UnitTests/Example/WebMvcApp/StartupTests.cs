// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Dhgms.AspNetCoreContrib.Example.WebMvcApp;
using Xunit;

namespace Whipstaff.UnitTests.Example.WebMvcApp
{
    /// <summary>
    /// Tests for the Startup class.
    /// </summary>
    public sealed class StartupTests : AbstractStartupTests<Startup>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StartupTests"/> class.
        /// </summary>
        /// <param name="output">XUnit test output helper instance.</param>
        public StartupTests(ITestOutputHelper output)
            : base(output)
        {
        }
    }
}
