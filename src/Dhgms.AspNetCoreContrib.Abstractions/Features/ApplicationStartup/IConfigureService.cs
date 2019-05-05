using Microsoft.Extensions.DependencyInjection;

namespace Dhgms.AspNetCoreContrib.Abstractions.Features.ApplicationStartup
{
    public interface IConfigureService
    {
        void ConfigureService(IServiceCollection services);
    }
}