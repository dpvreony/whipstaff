namespace Dhgms.AspNetCoreContrib.Abstractions
{
    using System.Security.Claims;
    using MediatR;

    public interface IAuditableRequest<out TRequestDto, out TResponse> : IRequest<TResponse>
    {
        TRequestDto RequestDto { get; }

        ClaimsPrincipal ClaimsPrincipal { get; }
    }
}