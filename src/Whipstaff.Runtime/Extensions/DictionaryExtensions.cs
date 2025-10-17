// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#if ARGUMENT_NULL_EXCEPTION_SHIM
using ArgumentNullException = Whipstaff.Runtime.Exceptions.ArgumentNullException;
#endif

namespace Whipstaff.Runtime.Extensions
{
    /// <summary>
    /// Extensions methods for dictionaries.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Gets the keys from a dictionary where the predicate is true.
        /// </summary>
        /// <typeparam name="TKey">The type for the dictionary keys.</typeparam>
        /// <typeparam name="TValue">The type for the dictionary values.</typeparam>
        /// <param name="dictionary">Dictionary to search.</param>
        /// <param name="predicate">Predicate to apply to the dictionary items.</param>
        /// <returns>Enumeration of keys that meet the predicate.</returns>
        public static IEnumerable<TKey> KeysWhere<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            Func<KeyValuePair<TKey, TValue>, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(dictionary);
            ArgumentNullException.ThrowIfNull(predicate);

            return dictionary.KeysWhereInternal(predicate);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IEnumerable<TKey> KeysWhereInternal<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            Func<KeyValuePair<TKey, TValue>, bool> predicate)
        {
#pragma warning disable S3267
            foreach (var kvp in dictionary)
#pragma warning restore S3267
            {
                if (predicate(kvp))
                {
                    yield return kvp.Key;
                }
            }
        }
    }
}
