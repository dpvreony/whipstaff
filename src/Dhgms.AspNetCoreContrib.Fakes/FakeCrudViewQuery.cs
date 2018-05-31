using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Text;
using Dhgms.AspNetCoreContrib.Controllers;

namespace Dhgms.AspNetCoreContrib.Fakes
{
    [ExcludeFromCodeCoverage]
    public class FakeCrudViewQuery : AuditableRequest<long, long?>
    {
        public FakeCrudViewQuery(long requestDto, ClaimsPrincipal claimsPrincipal) : base(requestDto, claimsPrincipal)
        {
        }
    }
}
