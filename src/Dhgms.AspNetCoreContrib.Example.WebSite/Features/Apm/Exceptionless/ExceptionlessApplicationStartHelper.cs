using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exceptionless;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Dhgms.AspNetCoreContrib.Example.WebSite.Features.Apm.Exceptionless
{
    public static class ExceptionlessApplicationStartHelper
    {
        public static void Configure(IConfiguration configuration, IApplicationBuilder app)
        {
            var exceptionlessApiKey = configuration.GetValue<string>("APM_EXCEPTIONLESS_API_KEY", null);
            if (!string.IsNullOrWhiteSpace(exceptionlessApiKey))
            {
                app.UseExceptionless();
            }
        }
    }
}
