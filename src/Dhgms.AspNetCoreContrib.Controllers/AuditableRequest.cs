using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Text;
using Dhgms.AspNetCoreContrib.Abstractions;

namespace Dhgms.AspNetCoreContrib.Controllers
{
    public class AuditableRequest<TRequestDto, TResponse> : IAuditableRequest<TRequestDto, TResponse>
    {
        public AuditableRequest(
            TRequestDto requestDto,
            ClaimsPrincipal claimsPrincipal,
            IPAddress ipAddress,
            IDictionary<string, string> clientHeaders)
        {
            RequestDto = requestDto;
            ClaimsPrincipal = claimsPrincipal;
            IpAddress = ipAddress;
            ClientHeaders = clientHeaders;
        }

        public TRequestDto RequestDto { get; }
        public ClaimsPrincipal ClaimsPrincipal { get; }
        public IPAddress IpAddress { get; }
        public IDictionary<string, string> ClientHeaders { get; }
    }
}
