// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Whipstaff.Runtime.JobSequencing
{
    /// <summary>
    /// Job Sequence helper for having a func work on an enumerable.
    /// </summary>
    /// <typeparam name="T">The type for the POCO object.</typeparam>
    public sealed class FuncAbstractAbstractActOnEnumerable<T> : AbstractAbstractActOnEnumerable<T>
    {
        private readonly Func<T, Task> _actOnItemAsyncFunc;
        private readonly Func<Task> _onNoItemsHandledAsync;

        /// <summary>
        /// Initializes a new instance of the <see cref="FuncAbstractAbstractActOnEnumerable{T}"/> class.
        /// </summary>
        /// <param name="actOnItemAsyncFunc">Function to execute on each item in the collection.</param>
        /// <param name="onNoItemsHandledAsync">Function to execute if the collection is empty.</param>
        public FuncAbstractAbstractActOnEnumerable(
            Func<T, Task> actOnItemAsyncFunc,
            Func<Task> onNoItemsHandledAsync)
        {
            _actOnItemAsyncFunc =
                actOnItemAsyncFunc ?? throw new ArgumentNullException(nameof(actOnItemAsyncFunc));
            _onNoItemsHandledAsync =
                onNoItemsHandledAsync ?? throw new ArgumentNullException(nameof(onNoItemsHandledAsync));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FuncAbstractAbstractActOnEnumerable{T}"/> class.
        /// </summary>
        /// <param name="enumerable">Enumerable object to iterate through.</param>
        /// <param name="actOnItemAsyncFunc">Function to execute on each item in the collection.</param>
        /// <param name="onNoItemsHandledAsync">Function to execute if the collection is empty.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public static Task ActOnCollectionAsync(
            IEnumerable<T> enumerable,
            Func<T, Task> actOnItemAsyncFunc,
            Func<Task> onNoItemsHandledAsync)
        {
            var instance = new FuncAbstractAbstractActOnEnumerable<T>(actOnItemAsyncFunc, onNoItemsHandledAsync);
            return instance.ActOnCollectionAsync(enumerable);
        }

        /// <inheritdoc/>
        protected override Task ActOnItemAsync(T item)
        {
            return _actOnItemAsyncFunc(item);
        }

        /// <inheritdoc/>
        protected override Task OnNoItemsHandledAsync()
        {
            return _onNoItemsHandledAsync();
        }
    }
}