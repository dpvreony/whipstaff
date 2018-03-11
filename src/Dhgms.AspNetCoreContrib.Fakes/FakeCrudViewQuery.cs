using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Dhgms.AspNetCoreContrib.Controllers;

namespace Dhgms.AspNetCoreContrib.Fakes
{
    public class FakeCrudViewQuery : AuditableRequest<long, int>
    {
        public FakeCrudViewQuery(int requestDto, ClaimsPrincipal claimsPrincipal) : base(requestDto, claimsPrincipal)
        {
        }
    }
}
