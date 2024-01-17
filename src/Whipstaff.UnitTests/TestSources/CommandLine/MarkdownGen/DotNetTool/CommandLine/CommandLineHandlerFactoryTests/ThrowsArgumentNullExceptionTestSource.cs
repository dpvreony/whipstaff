﻿// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO.Abstractions;
using NetTestRegimentation.XUnit.Theories.ArgumentNullException;

namespace Whipstaff.UnitTests.TestSources.CommandLine.MarkdownGen.DotNetTool.CommandLine.CommandLineHandlerFactoryTests
{
    /// <summary>
    /// Test Source for <see cref="Whipstaff.UnitTests.CommandLine.MarkdownGen.DotNetTool.CommandLine.CommandLineHandlerFactoryTests.GetRootCommandAndBinderMethod.ThrowsArgumentNullException"/>.
    /// </summary>
    public sealed class ThrowsArgumentNullExceptionTestSource : ArgumentNullExceptionTheoryData<IFileSystem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
        /// </summary>
        public ThrowsArgumentNullExceptionTestSource()
            : base("fileSystem")
        {
        }
    }
}
