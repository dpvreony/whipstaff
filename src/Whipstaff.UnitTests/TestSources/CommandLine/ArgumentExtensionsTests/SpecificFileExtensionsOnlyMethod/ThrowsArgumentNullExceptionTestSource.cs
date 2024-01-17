// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Xunit;

namespace Whipstaff.UnitTests.TestSources.CommandLine.ArgumentExtensionsTests.SpecificFileExtensionsOnlyMethod
{
    /// <summary>
    /// Test Source for <see cref="UnitTests.CommandLine.ArgumentExtensionsTests.SpecificFileExtensionsOnlyMethod.ThrowsArgumentNullException"/>.
    /// </summary>
    public sealed class ThrowsArgumentNullExceptionTestSource : TheoryData<IFileSystem?, string[]?, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
        /// </summary>
        public ThrowsArgumentNullExceptionTestSource()
        {
            Add(null, [".txt"], "fileSystem");
            Add(new MockFileSystem(), null, "extensions");
        }
    }
}
