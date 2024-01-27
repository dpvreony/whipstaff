// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Xunit;

namespace Whipstaff.UnitTests.TestSources.CommandLine.ReflectionHelpersTests.IsRootCommandAndBinderTypeMethod
{
    /// <summary>
    /// Test Source for <see cref="Whipstaff.UnitTests.CommandLine.ReflectionHelpersTests.IsRootCommandAndBinderTypeMethod.ReturnsTrue"/>.
    /// </summary>
    public sealed class ReturnsTrueTestSource : TheoryData<Type>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReturnsTrueTestSource"/> class.
        /// </summary>
        public ReturnsTrueTestSource()
        {
            Add(typeof(Whipstaff.CommandLine.MarkdownGen.DotNetTool.CommandLine.CommandLineHandlerFactory));
        }
    }
}
