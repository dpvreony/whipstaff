using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using MediatR;

namespace Dhgms.AspNetCoreContrib.Abstractions
{
    public interface IAuditableRequest<out TRequestDto, out TResponse> : IRequest<TResponse>
    {
        TRequestDto RequestDto { get; }

        ClaimsPrincipal ClaimsPrincipal { get; }

        IPAddress IpAddress { get; }

        IDictionary<string, string> ClientHeaders { get; }
    }
}