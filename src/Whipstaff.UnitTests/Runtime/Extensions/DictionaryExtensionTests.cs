// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using NetTestRegimentation;
using Xunit;
using Xunit.Abstractions;

namespace Whipstaff.UnitTests.Runtime.Extensions
{
    /// <summary>
    /// Unit Tests for <see cref="Whipstaff.Runtime.Extensions.DictionaryExtensions"/>.
    /// </summary>
    public static class DictionaryExtensionTests
    {
        /// <summary>
        /// Unit Tests for <see cref="Whipstaff.Runtime.Extensions.DictionaryExtensions.KeysWhere{TKey, TValue}(IDictionary{TKey, TValue}, Func{KeyValuePair{TKey, TValue}, bool}"/> method.
        /// </summary>
        public sealed class KeysWhereMethod
            : Foundatio.Xunit.TestWithLoggingBase,
                ITestMethodWithNullableParameters<Dictionary<string, string>, Func<string, bool>, IEnumerable<string>>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="KeysWhereMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public KeysWhereMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            [Theory]
            public void ThrowsArgumentNullException(
                Dictionary<string, string> arg1,
                Func<string, bool> arg2,
                IEnumerable<string> arg3,
                string expectedParameterNameForException)
            {
            }

            [Fact]
            public void ReturnsKeysWherePredicateReturnsTrue()
            {
            }

            [Fact]
            public void ReturnsEmptyEnumerableWhenPredicateReturnsFalse()
            {
            }
        }
    }
}
