using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Security.Claims;
using System.Text;
using Dhgms.AspNetCoreContrib.Abstractions;
using Dhgms.AspNetCoreContrib.Controllers;

namespace Dhgms.AspNetCoreContrib.Fakes
{
    [ExcludeFromCodeCoverage]
    public class FakeCrudAddCommand : AuditableRequest<int, int>
    {
        public FakeCrudAddCommand(
            int requestDto,
            ClaimsPrincipal claimsPrincipal,
            IPAddress ipAddress,
            IDictionary<string, string> clientHeaders)
            : base(requestDto, claimsPrincipal, ipAddress, clientHeaders)
        {
        }
    }
}
