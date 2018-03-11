using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Dhgms.AspNetCoreContrib.Abstractions;
using Dhgms.AspNetCoreContrib.Controllers;

namespace Dhgms.AspNetCoreContrib.Fakes
{
    public class FakeCrudDeleteCommand : AuditableRequest<long, int>
    {
        public FakeCrudDeleteCommand(long requestDto, ClaimsPrincipal claimsPrincipal) : base(requestDto, claimsPrincipal)
        {
        }
    }
}
