// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.CommandLine;
using System.CommandLine.Parsing;
using Xunit;

namespace Whipstaff.UnitTests.TestSources.CommandLine.ArgumentResultHelpersTests.FileHasSupportedExtensionMethod
{
    /// <summary>
    /// Test Source for <see cref="Whipstaff.UnitTests.CommandLine.ArgumentResultHelpersTests.FileHasSupportedExtensionMethod.ThrowsArgumentNullException"/>.
    /// </summary>
    public sealed class ThrowsArgumentNullExceptionTestSource : TheoryData<ArgumentResult?, string?, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
        /// </summary>
        public ThrowsArgumentNullExceptionTestSource()
        {
            var argument1Builder = new Argument<string>();
            var arg1 = argument1Builder.Parse("somefilename.txt").FindResultFor(argument1Builder);

            var arg2 = ".txt";
            Add(null, arg2, "result");
            Add(arg1, null, "extension");
        }
    }
}
