// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

namespace Whipstaff.Runtime.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IList{T}"/>.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Adds an item to the list if the type is not already present.
        /// </summary>
        /// <typeparam name="TListType">The type for the list being checked.</typeparam>
        /// <typeparam name="TRequiredType">The type of object required to be in the list.</typeparam>
        /// <param name="list">The list to check.</param>
        public static void AddIfTypeNotPresent<TListType, TRequiredType>(this IList<TListType> list)
            where TListType : class
            where TRequiredType : TListType, new()
        {
            if (list.All(t => t.GetType() != typeof(TRequiredType)))
            {
                list.Add(new TRequiredType());
            }
        }
    }
}
