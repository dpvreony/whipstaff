using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exceptionless;
using Exceptionless.NLog;
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

                //ConfigureLogging(exceptionlessApiKey);
            }
        }

        //private static void ConfigureNLog(string exceptionlessApiKey)
        //{
        //    global::Exceptionless.Extensions.Logging.ExceptionlessLogger

        //    var nLogTarget = new global::Exceptionless.NLog.ExceptionlessTarget
        //    {
        //        Name = "Exceptionless",
        //        ApiKey = exceptionlessApiKey,
        //    };
        //    nLogTarget.Fields.Add(new ExceptionlessField {Name = "host", Layout = "${machinename}"});
        //    nLogTarget.Fields.Add(new ExceptionlessField {Name = "identity", Layout = "${identity}"});
        //    nLogTarget.Fields.Add(new ExceptionlessField
        //        {Name = "windows-identity", Layout = "${windows-identity:userName=True:domain=False}"});
        //    nLogTarget.Fields.Add(new ExceptionlessField {Name = "process", Layout = "${processname}"});

        //    NLog.LogManager.Configuration.AddTarget(nLogTarget);
        //}
    }
}
