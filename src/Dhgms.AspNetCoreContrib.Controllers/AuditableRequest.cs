using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Dhgms.AspNetCoreContrib.Abstractions;

namespace Dhgms.AspNetCoreContrib.Controllers
{
    public class AuditableRequest<TRequestDto, TResponse> : IAuditableRequest<TRequestDto, TResponse>
    {
        public AuditableRequest(
            TRequestDto requestDto,
            ClaimsPrincipal claimsPrincipal)
        {
            RequestDto = requestDto;
            ClaimsPrincipal = claimsPrincipal;
        }

        public TRequestDto RequestDto { get; }
        public ClaimsPrincipal ClaimsPrincipal { get; }
    }
}
