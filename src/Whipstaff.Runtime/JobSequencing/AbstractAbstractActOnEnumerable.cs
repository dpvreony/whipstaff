// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Whipstaff.Runtime.JobSequencing
{
    /// <summary>
    /// Helper to act on an enumerable.
    /// </summary>
    /// <typeparam name="T">The type for the object being processed.</typeparam>
    public abstract class AbstractAbstractActOnEnumerable<T> : AbstractActOnCollectionHelper<IEnumerable<T>, T>
    {
        /// <inheritdoc/>
        public sealed override async Task ActOnCollectionAsync(IEnumerable<T> queryable)
        {
            if (queryable == null)
            {
                return;
            }

            var handled = false;
            foreach (var item in queryable)
            {
                await ActOnItemAsync(item).ConfigureAwait(false);

                handled = true;
            }

            if (handled)
            {
                return;
            }

            // no records handled
            await OnNoItemsHandledAsync().ConfigureAwait(false);
        }
    }
}
