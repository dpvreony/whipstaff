// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Data;
using System.Threading.Tasks;

namespace Whipstaff.Runtime.JobSequencing
{
    /// <summary>
    /// Helper to act on a data reader collection of results.
    /// </summary>
    public abstract class AbstractAbstractActOnDataReader : AbstractActOnCollectionHelper<IDataReader, IDataReader>
    {
        /// <inheritdoc/>
        public sealed override async Task ActOnCollectionAsync(IDataReader queryable)
        {
            if (queryable != null && queryable.Read())
            {
                do
                {
                    await ActOnItemAsync(queryable).ConfigureAwait(false);
                }
                while (queryable.Read());

                return;
            }

            await OnNoItemsHandledAsync().ConfigureAwait(false);
        }
    }
}
