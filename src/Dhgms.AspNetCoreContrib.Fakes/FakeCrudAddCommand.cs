using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Dhgms.AspNetCoreContrib.Abstractions;
using Dhgms.AspNetCoreContrib.Controllers;

namespace Dhgms.AspNetCoreContrib.Fakes
{
    public class FakeCrudAddCommand : AuditableRequest<int, int>
    {
        public FakeCrudAddCommand(int requestDto, ClaimsPrincipal claimsPrincipal) : base(requestDto, claimsPrincipal)
        {
        }
    }
}
