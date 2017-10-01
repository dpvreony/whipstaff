using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dhgms.AspNetCoreContrib.Abstractions
{
    public interface IAuditableQueryFactory<TListRequestDto, TListResponse, TViewResponse>
    {
        Task<IAuditableRequest<TListRequestDto, TListResponse>> GetListQueryAsync(
            TListRequestDto requestDto,
            System.Security.Claims.ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken);

        Task<IAuditableRequest<long, TViewResponse>> GetViewQueryAsync(
            long id,
            System.Security.Claims.ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken);
    }

}
