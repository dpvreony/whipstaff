using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Example.WebSite.Features.Apm.ApplicationInsights;
using Dhgms.AspNetCoreContrib.Example.WebSite.Features.Apm.Exceptionless;
using Exceptionless;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Dhgms.AspNetCoreContrib.Example.WebSite.Features.Apm
{
    public static class ApmApplicationStartHelper
    {
        public static void Configure(
            IConfiguration configuration,
            IApplicationBuilder app,
            IHostingEnvironment env,
            Version version)
        {
            ExceptionlessApplicationStartHelper.Configure(configuration, app, env, version);
            new ApplicationInsightsApplicationStartHelper().ConfigureApplication(app);
        }
    }
}
