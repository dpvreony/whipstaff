// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Whipstaff.Runtime.AppDomains;
using Whipstaff.Rx.Runtime.AppDomains;
using Xunit;

namespace Whipstaff.UnitTests.Rx.Runtime.AppDomains
{
    /// <summary>
    /// Unit tests for the <see cref="AssemblyResolverExtensions"/> class.
    /// </summary>
    public static class AssemblyResolverExtensionsTests
    {
        /// <summary>
        /// Test for the <see cref="AssemblyResolverExtensions.ToObservable"/> method.
        /// </summary>
        public sealed class ToObservableMethod : NetTestRegimentation.ITestMethodWithNullableParameters<IAssemblyResolveHelper>
        {
            /// <inheritdoc />
            [Theory]
            [ClassData(typeof(Whipstaff.UnitTests.TestSources.Rx.Runtime.AppDomains.AssemblyResolverExtensionsTests.ToObservableMethod.ThrowsArgumentNullExceptionTestSource))]
            public void ThrowsArgumentNullException(IAssemblyResolveHelper arg, string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => _ = arg.ToObservable());
            }
        }
    }
}
