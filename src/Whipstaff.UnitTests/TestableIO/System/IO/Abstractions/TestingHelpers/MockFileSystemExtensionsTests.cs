// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using Whipstaff.TestableIO.System.IO.Abstractions.TestingHelpers;
using Xunit;

namespace Whipstaff.UnitTests.TestableIO.System.IO.Abstractions.TestingHelpers
{
    /// <summary>
    /// Unit tests for the <see cref="MockFileSystemExtensions"/> class.
    /// </summary>
    public static class MockFileSystemExtensionsTests
    {
        /// <summary>
        /// Unit tests for the <see cref="MockFileSystemExtensions.AddAllEmbeddedFilesFromNamespace"/> method.
        /// </summary>
        public sealed class AddAllEmbeddedFilesFromNamespaceMethod
        {
            /// <summary>
            /// Tests that the method succeeds.
            /// </summary>
            [Fact]
            public void Succeeds()
            {
                var mockFileSystem = new MockFileSystem();
                mockFileSystem.AddAllEmbeddedFilesFromNamespace(
                    typeof(MockFileSystemExtensionsTests).Assembly,
                    "Whipstaff.UnitTests.Resources",
                    "root");

                var counter = mockFileSystem.AllFiles.Count();
                Assert.Equal(2, counter);
            }
        }
    }
}
