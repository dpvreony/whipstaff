using Microsoft.AspNetCore.Builder;

namespace Dhgms.AspNetCoreContrib.Abstractions.Features.ApplicationStartup
{
    public interface IConfigureApplication
    {
        void ConfigureApplication(IApplicationBuilder app);
    }
}