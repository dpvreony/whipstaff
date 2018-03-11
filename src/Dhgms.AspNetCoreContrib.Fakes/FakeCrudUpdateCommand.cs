using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Dhgms.AspNetCoreContrib.Abstractions;

namespace Dhgms.AspNetCoreContrib.Fakes
{
    public class FakeCrudUpdateCommand : IAuditableRequest<int, int>
    {
        public int RequestDto { get; set; }
        public ClaimsPrincipal ClaimsPrincipal { get; set; }
    }
}
