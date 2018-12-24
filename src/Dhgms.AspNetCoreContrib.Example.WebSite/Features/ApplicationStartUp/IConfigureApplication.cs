namespace Dhgms.AspNetCoreContrib.Example.WebSite.Features.ApplicationStartUp
{
    using Microsoft.AspNetCore.Builder;

    public interface IConfigureApplication
    {
        void ConfigureApplication(IApplicationBuilder app);
    }
}