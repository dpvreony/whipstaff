// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using NetTestRegimentation;
using Whipstaff.EntityFramework.Diagram.DotNetTool;
using Xunit;

namespace Whipstaff.UnitTests.EntityFramework.Diagram.DotNetTool
{
    /// <summary>
    /// Unit tests for the <see cref="CommandLineJobLogMessageActions"/> class.
    /// </summary>
    public static class CommandLineJobLogMessageActionsTests
    {
        /// <summary>
        /// Unit test for <see cref="CommandLineJobLogMessageActions"/> constructor.
        /// </summary>
        public sealed class ConstructorMethod : ITestConstructorMethod
        {
            /// <inheritdoc />
            [Fact]
            public void ReturnsInstance()
            {
                Assert.NotNull(new CommandLineJobLogMessageActions());
            }
        }
    }
}
