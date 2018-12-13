using Microsoft.AspNetCore.Builder;

namespace Dhgms.AspNetCoreContrib.Example.WebSite.Features.ApplicationStartUp
{
    public interface IConfigureApplication
    {
        void ConfigureApplication(IApplicationBuilder app);
    }
}