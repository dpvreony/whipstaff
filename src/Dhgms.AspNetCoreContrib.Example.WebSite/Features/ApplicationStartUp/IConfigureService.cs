using Microsoft.Extensions.DependencyInjection;

namespace Dhgms.AspNetCoreContrib.Example.WebSite.Features.ApplicationStartUp
{
    public interface IConfigureService
    {
        void ConfigureService(IServiceCollection services);
    }
}