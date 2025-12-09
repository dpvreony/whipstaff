// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Whipstaff.Mediator;

namespace Whipstaff.Testing.Cqrs
{
    /// <inheritdoc/>
    public sealed class FakeAuditableQueryFactory : IAuditableQueryFactory<FakeCrudListQuery, FakeCrudListRequest, IList<int>, FakeCrudViewQuery, FakeCrudViewResponse>
    {
        /// <inheritdoc/>
        public Task<FakeCrudListQuery> GetListQueryAsync(
            FakeCrudListRequest requestDto,
            ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(new FakeCrudListQuery(requestDto, claimsPrincipal));
        }

        /// <inheritdoc/>
        public Task<FakeCrudViewQuery> GetViewQueryAsync(long id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            return Task.FromResult(new FakeCrudViewQuery(id, claimsPrincipal));
        }
    }
}
