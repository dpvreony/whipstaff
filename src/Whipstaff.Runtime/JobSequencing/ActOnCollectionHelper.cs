// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace Whipstaff.Runtime.JobSequencing
{
    /// <summary>
    /// Base class for describing actions on a collection.
    /// </summary>
    /// <typeparam name="TCollection">The type for the collection.</typeparam>
    /// <typeparam name="TItem">The type for the item in the collection.</typeparam>
#pragma warning disable S1694 // An abstract class should have both abstract and concrete methods
    public abstract class ActOnCollectionHelper<TCollection, TItem>
#pragma warning restore S1694 // An abstract class should have both abstract and concrete methods
    {
        /// <summary>
        /// Action to carry out on the collection.
        /// </summary>
        /// <param name="queryable">The queryable to check.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public abstract Task ActOnCollectionAsync(TCollection queryable);

        /// <summary>
        /// Action to carry out on each item.
        /// </summary>
        /// <param name="item">The item to process.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        protected abstract Task ActOnItemAsync(TItem item);

        /// <summary>
        /// Action to carry out if there were no items to process.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        protected abstract Task OnNoItemsHandledAsync();
    }
}
