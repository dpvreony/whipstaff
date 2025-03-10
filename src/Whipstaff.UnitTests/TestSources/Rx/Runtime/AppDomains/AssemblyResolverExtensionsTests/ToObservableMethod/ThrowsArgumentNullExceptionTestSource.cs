// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using NetTestRegimentation.XUnit.Theories.ArgumentNullException;
using Whipstaff.Runtime.AppDomains;

namespace Whipstaff.UnitTests.TestSources.Rx.Runtime.AppDomains.AssemblyResolverExtensionsTests.ToObservableMethod
{
    /// <summary>
    /// Test Source for <see cref="Whipstaff.UnitTests.Runtime.Extensions.DictionaryExtensionTests.KeysWhereMethod.ThrowsArgumentNullException"/>.
    /// </summary>
    public sealed class ThrowsArgumentNullExceptionTestSource : ArgumentNullExceptionTheoryData<IAssemblyResolveHelper>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
        /// </summary>
        public ThrowsArgumentNullExceptionTestSource()
            : base("assemblyResolveHelper")
        {
        }
    }
}
