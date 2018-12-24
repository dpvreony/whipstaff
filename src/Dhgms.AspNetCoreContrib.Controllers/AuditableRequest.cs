namespace Dhgms.AspNetCoreContrib.Controllers
{
    using System.Security.Claims;
    using Dhgms.AspNetCoreContrib.Abstractions;

    public class AuditableRequest<TRequestDto, TResponse> : IAuditableRequest<TRequestDto, TResponse>
    {
        public AuditableRequest(
            TRequestDto requestDto,
            ClaimsPrincipal claimsPrincipal)
        {
            this.RequestDto = requestDto;
            this.ClaimsPrincipal = claimsPrincipal;
        }

        public TRequestDto RequestDto { get; }
        public ClaimsPrincipal ClaimsPrincipal { get; }
    }
}
