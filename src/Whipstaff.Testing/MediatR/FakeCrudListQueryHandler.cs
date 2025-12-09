// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Whipstaff.Testing.Cqrs;

namespace Whipstaff.Testing.MediatR
{
    /// <summary>
    /// CQRS Query Handler for the CRUD List Query.
    /// </summary>
    public sealed class FakeCrudListQueryHandler : IRequestHandler<FakeCrudListQuery, IList<int>?>
    {
        /// <inheritdoc />
        public async Task<IList<int>?> Handle(FakeCrudListQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<int>
            {
                1,
                2,
                3,
                4,
                5
            }).ConfigureAwait(false);
        }
    }
}
