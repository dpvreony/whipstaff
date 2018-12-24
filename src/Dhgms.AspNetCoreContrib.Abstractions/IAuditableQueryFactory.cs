namespace Dhgms.AspNetCoreContrib.Abstractions
{
    using System.Threading;
    using System.Threading.Tasks;

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
