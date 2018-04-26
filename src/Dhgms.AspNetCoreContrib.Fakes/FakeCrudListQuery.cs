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
    public class FakeCrudListQuery : AuditableRequest<FakeCrudListRequest, IList<int>>
    {
        public FakeCrudListQuery(FakeCrudListRequest requestDto,
            ClaimsPrincipal claimsPrincipal,
            IPAddress ipAddress,
            IDictionary<string, string> clientHeaders)
            : base(requestDto, claimsPrincipal, ipAddress, clientHeaders)
        {
        }
    }
}
