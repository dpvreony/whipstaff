// Copyright (c) 2020 DHGMS Solutions and Contributors. All rights reserved.
// DHGMS Solutions and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Whipstaff.Runtime.JobSequencing
{
    /// <summary>
    /// Helper to act on an enumerable.
    /// </summary>
    /// <typeparam name="T">The type for the object being processed.</typeparam>
    public abstract class ActOnEnumerable<T> : ActOnCollection<IEnumerable<T>, T>
    {
        /// <inheritdoc/>
        public sealed override async Task ActOnCollectionAsync(IEnumerable<T> queryable)
        {
            foreach (var item in queryable)
            {
                await ActOnItemAsync(item).ConfigureAwait(false);
                return;
            }

            // no records handled
            await OnNoItemsHandledAsync().ConfigureAwait(false);
        }
    }
}
