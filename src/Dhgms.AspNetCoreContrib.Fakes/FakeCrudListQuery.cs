namespace Dhgms.AspNetCoreContrib.Fakes
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Claims;
    using Dhgms.AspNetCoreContrib.Controllers;

    [ExcludeFromCodeCoverage]
    public class FakeCrudListQuery : AuditableRequest<FakeCrudListRequest, IList<int>>
    {
        public FakeCrudListQuery(FakeCrudListRequest requestDto, ClaimsPrincipal claimsPrincipal) : base(requestDto, claimsPrincipal)
        {
        }
    }
}
