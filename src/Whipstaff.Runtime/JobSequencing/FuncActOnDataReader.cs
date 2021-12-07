// Copyright (c) 2020 DHGMS Solutions and Contributors. All rights reserved.
// DHGMS Solutions and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Data;
using System.Threading.Tasks;

namespace Whipstaff.Runtime.JobSequencing
{
    /// <summary>
    /// Represents a data reader actor that takes a func.
    /// </summary>
    public sealed class FuncActOnDataReader : ActOnDataReader
    {
        private readonly Func<IDataReader, Task> _actOnItemAsyncFunc;
        private readonly Func<Task> _onNoItemsHandledAsync;

        /// <summary>
        /// Initializes a new instance of the <see cref="FuncActOnDataReader"/> class.
        /// </summary>
        /// <param name="actOnItemAsyncFunc">The action to carry out on each item.</param>
        /// <param name="onNoItemsHandledAsync">The action to carry out if there are no items in the data reader.</param>
        public FuncActOnDataReader(
            Func<IDataReader, Task> actOnItemAsyncFunc,
            Func<Task> onNoItemsHandledAsync)
        {
            _actOnItemAsyncFunc = actOnItemAsyncFunc ?? throw new ArgumentNullException(nameof(actOnItemAsyncFunc));
            _onNoItemsHandledAsync = onNoItemsHandledAsync ?? throw new ArgumentNullException(nameof(onNoItemsHandledAsync));
        }

        /// <summary>
        /// Helper to create a Func to act on a data reader.
        /// </summary>
        /// <param name="dataReader">The datareader to act upon.</param>
        /// <param name="actOnItemAsyncFunc">The action to carry out.</param>
        /// <param name="onNoItemsHandledAsync">The action to carry out if there are no items in the data reader.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public static Task ActOnCollectionAsync(
            IDataReader dataReader,
            Func<IDataReader, Task> actOnItemAsyncFunc,
            Func<Task> onNoItemsHandledAsync)
        {
            var instance = new FuncActOnDataReader(
                actOnItemAsyncFunc,
                onNoItemsHandledAsync);
            return instance.ActOnCollectionAsync(dataReader);
        }

        /// <inheritdoc/>
        protected override Task ActOnItemAsync(IDataReader item)
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
