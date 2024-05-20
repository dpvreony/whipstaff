// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using NetTestRegimentation.XUnit.Theories.ArgumentNullException;

namespace Whipstaff.UnitTests.TestSources.Runtime.Extensions.DictionaryExtensionTests.KeysWhereMethod
{
    /// <summary>
    /// Test Source for <see cref="Whipstaff.UnitTests.Runtime.Extensions.DictionaryExtensionTests.KeysWhereMethod.ThrowsArgumentNullException"/>.
    /// </summary>
    public sealed class ThrowsArgumentNullExceptionTestSource : ArgumentNullExceptionTheoryData<Dictionary<string, string>, Func<KeyValuePair<string, string>, bool>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
        /// </summary>
        public ThrowsArgumentNullExceptionTestSource()
            : base(
                new NamedParameterInput<Dictionary<string, string>>(
                    "dictionary",
                    () => new Dictionary<string, string> { { "key1", "value1" }, { "key2", "value2" }, }),
                new NamedParameterInput<Func<KeyValuePair<string, string>, bool>>(
                    "predicate",
                    () => _ => true))
        {
        }
    }
}
