// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.IO;
using System.Threading.Tasks;
using Whipstaff.CommandLine;
using Whipstaff.Testing.CommandLine;
using Xunit;

namespace Whipstaff.UnitTests.TestSources.CommandLine.CommandLineArgumentHelpersTests.GetResultFromRootCommandMethod
{
    /// <summary>
    /// Test Source for <see cref="Whipstaff.UnitTests.CommandLine.CommandLineArgumentHelpersTests.GetResultFromRootCommandMethod.ThrowsArgumentNullExceptionAsync"/>.
    /// </summary>
    public sealed class ThrowsArgumentNullExceptionTestSource : TheoryData<string[]?, Func<RootCommandAndBinderModel<FakeCommandLineArgModelBinder>>?, Func<FakeCommandLineArgModel, Task<int>>?, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
        /// </summary>
        public ThrowsArgumentNullExceptionTestSource()
        {
            var arg1 = new[] { ".txt", ".docx" };
            var arg2 = new Func<RootCommandAndBinderModel<FakeCommandLineArgModelBinder>>(() => new RootCommandAndBinderModel<FakeCommandLineArgModelBinder>(
                new RootCommand(),
                new FakeCommandLineArgModelBinder(
                    new Argument<FileInfo>(), new Argument<string?>())));
            var arg3 = new Func<FakeCommandLineArgModel, Task<int>>(_ => Task.FromResult(0));

            Add(null, arg2, arg3, "args");

            Add(arg1, null, arg3, "rootCommandAndBinderModelFunc");

            Add(arg1, arg2, null, "rootCommandHandlerFunc");
        }
    }
}
