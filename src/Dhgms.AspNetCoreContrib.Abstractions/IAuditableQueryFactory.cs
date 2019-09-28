// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;

namespace Dhgms.AspNetCoreContrib.Abstractions
{
    public interface IAuditableQueryFactory<TListQuery, in TListRequestDto, TListResponse, TViewQuery, TViewResponse>
        where TListQuery : IAuditableRequest<TListRequestDto, TListResponse>
        where TViewQuery : IAuditableRequest<long, TViewResponse>
    {
        Task<TListQuery> GetListQueryAsync(
            TListRequestDto requestDto,
            System.Security.Claims.ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken);

        Task<TViewQuery> GetViewQueryAsync(
            long id,
            System.Security.Claims.ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken);
    }
}
