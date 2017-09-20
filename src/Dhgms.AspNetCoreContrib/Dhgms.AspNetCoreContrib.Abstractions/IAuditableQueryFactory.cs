using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dhgms.AspNetCoreContrib.Abstractions
{
    public interface IAuditableQueryFactory<TListRequestDto, TEntity>
    {
        Task<IAuditableRequest<TListRequestDto, IList<TEntity>>> GetListQueryAsync(
            TListRequestDto requestDto,
            System.Security.Claims.ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken);

        Task<IAuditableRequest<long, TEntity>> GetViewQueryAsync(
            long id,
            System.Security.Claims.ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken);
    }
}
