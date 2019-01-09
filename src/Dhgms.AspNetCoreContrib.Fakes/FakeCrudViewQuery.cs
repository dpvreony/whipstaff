namespace Dhgms.AspNetCoreContrib.Fakes
{
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Claims;
    using Dhgms.AspNetCoreContrib.Controllers;

    [ExcludeFromCodeCoverage]
    public class FakeCrudViewQuery : AuditableRequest<long, FakeCrudViewResponse>
    {
        public FakeCrudViewQuery(long requestDto, ClaimsPrincipal claimsPrincipal) : base(requestDto, claimsPrincipal)
        {
        }
    }
}
