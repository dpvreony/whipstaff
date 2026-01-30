// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using NetTestRegimentation.XUnit.Theories.ArgumentNullException;

namespace Whipstaff.UnitTests.TestSources.CommandLine.FileSystemAbstractions.ArgumentFactoryTests.GetDirectoryInfoArgumentMethod
{
    /// <summary>
    /// Test Source for <see cref="Whipstaff.UnitTests.CommandLine.FileSystemAbstractions.ArgumentFactoryTests.GetDirectoryInfoArgumentMethod.ThrowsArgumentNullException"/>.
    /// </summary>
    public sealed class ThrowsArgumentNullExceptionTestSource : ArgumentNullExceptionTheoryData<string, IFileSystem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
        /// </summary>
        public ThrowsArgumentNullExceptionTestSource()
            : base(
                new NamedParameterInput<string>(
                    "name",
                    static () => "somename"),
                new NamedParameterInput<IFileSystem>(
                    "fileSystem",
                    static () => new MockFileSystem()))
        {
        }
    }
}
