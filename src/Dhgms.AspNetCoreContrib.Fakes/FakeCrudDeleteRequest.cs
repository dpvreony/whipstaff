using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Dhgms.AspNetCoreContrib.Controllers;

namespace Dhgms.AspNetCoreContrib.Fakes
{
    public class FakeCrudDeleteRequest : AuditableRequest<long, int>
    {
        public FakeCrudDeleteRequest(long requestDto, ClaimsPrincipal claimsPrincipal) : base(requestDto, claimsPrincipal)
        {
        }
    }
}
