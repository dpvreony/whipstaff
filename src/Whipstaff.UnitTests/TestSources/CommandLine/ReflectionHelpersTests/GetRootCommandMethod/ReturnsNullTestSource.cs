// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reflection;
using Xunit;

namespace Whipstaff.UnitTests.TestSources.CommandLine.ReflectionHelpersTests.GetRootCommandMethod
{
    /// <summary>
    /// Test Source for <see cref="Whipstaff.UnitTests.CommandLine.ReflectionHelpersTests.GetRootCommandMethod.ReturnsNull"/>.
    /// </summary>
    public sealed class ReturnsNullTestSource : TheoryData<Assembly>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReturnsNullTestSource"/> class.
        /// </summary>
        public ReturnsNullTestSource()
        {
            Add(typeof(ReturnsNullTestSource).Assembly);
        }
    }
}
