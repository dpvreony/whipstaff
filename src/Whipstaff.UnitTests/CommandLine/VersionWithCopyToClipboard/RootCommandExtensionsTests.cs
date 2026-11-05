// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.CommandLine;
using System.Linq;
using Whipstaff.CommandLine.VersionWithCopyToClipboard;
using Xunit;

namespace Whipstaff.UnitTests.CommandLine.VersionWithCopyToClipboard
{
    /// <summary>
    /// Unit Tests for <see cref="RootCommandExtensions"/>.
    /// </summary>
    public static class RootCommandExtensionsTests
    {
        /// <summary>
        /// Unit Tests for <see cref="RootCommandExtensions.EnableVersionWithClipboardCopy(RootCommand)"/>.
        /// </summary>
        public sealed class EnableVersionWithClipboardCopyMethod
        {
            /// <summary>
            /// Test to ensure the root command is modified correctly.
            /// </summary>
            [Fact]
            public void ModifiesRootCommand()
            {
                var rootCommand = new System.CommandLine.RootCommand();
                rootCommand.EnableVersionWithClipboardCopy();

                Assert.Empty(rootCommand.Options.OfType<VersionOption>());
                _ = Assert.Single(rootCommand.Options.OfType<Whipstaff.CommandLine.VersionWithCopyToClipboard.VersionCopyOption>());
                _ = Assert.Single(rootCommand.Options.OfType<Whipstaff.CommandLine.VersionWithCopyToClipboard.VersionWithCopyToClipboardOption>());
            }
        }
    }
}
