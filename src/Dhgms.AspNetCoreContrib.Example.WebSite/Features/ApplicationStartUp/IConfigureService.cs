namespace Dhgms.AspNetCoreContrib.Example.WebSite.Features.ApplicationStartUp
{
    using Microsoft.Extensions.DependencyInjection;

    public interface IConfigureService
    {
        void ConfigureService(IServiceCollection services);
    }
}