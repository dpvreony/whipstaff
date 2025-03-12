// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NetTestRegimentation;
using Whipstaff.Runtime.Extensions;
using Whipstaff.Testing.Logging;
using Xunit;

namespace Whipstaff.UnitTests.Runtime.Extensions
{
    /// <summary>
    /// Unit Tests for <see cref="Whipstaff.Runtime.Extensions.DictionaryExtensions"/>.
    /// </summary>
    public static class DictionaryExtensionTests
    {
        /// <summary>
        /// Unit Tests for <see cref="Whipstaff.Runtime.Extensions.DictionaryExtensions.KeysWhere{TKey, TValue}(IDictionary{TKey, TValue}, Func{KeyValuePair{TKey, TValue}, bool})"/> method.
        /// </summary>
        public sealed class KeysWhereMethod
            : TestWithLoggingBase,
                ITestMethodWithNullableParameters<Dictionary<string, string>, Func<KeyValuePair<string, string>, bool>>
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
            [ClassData(typeof(Whipstaff.UnitTests.TestSources.Runtime.Extensions.DictionaryExtensionTests.KeysWhereMethod.ThrowsArgumentNullExceptionTestSource))]
            public void ThrowsArgumentNullException(
                Dictionary<string, string>? arg1,
                Func<KeyValuePair<string, string>, bool>? arg2,
                string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => arg1!.KeysWhere(arg2!).ToArray());
            }

            /// <summary>
            /// Test to ensure that the method returns the keys where the predicate returns true.
            /// </summary>
            [Fact]
            public void ReturnsKeysWherePredicateReturnsTrue()
            {
                var dictionary = new Dictionary<string, string>
                {
                    { "key1", "value1" },
                    { "key2", "value2" },
                };

                var result = dictionary.KeysWhere(kvp => kvp.Value == "value1");

                _ = Assert.Single(
                    result,
                    item => item == "key1");
            }

            /// <summary>
            /// Test to ensure that the method returns an empty enumerable when the predicate returns false.
            /// </summary>
            [Fact]
            public void ReturnsEmptyEnumerableWhenPredicateReturnsFalse()
            {
                var dictionary = new Dictionary<string, string>
                {
                    { "key1", "value1" },
                    { "key2", "value2" },
                };

                var result = dictionary.KeysWhere(kvp => kvp.Value == "value3");

                Assert.Empty(result);
            }
        }
    }
}
