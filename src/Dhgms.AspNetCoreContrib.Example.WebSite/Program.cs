using System.Diagnostics;

namespace Dhgms.AspNetCoreContrib.Example.WebSite
{
    using Exceptionless;
    using Exceptionless.Extensions.Logging;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Logging;

    public static class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging(ConfigureLogging);

        private static void ConfigureLogging(WebHostBuilderContext webHostBuilderContext, ILoggingBuilder loggingBuilder)
        {
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/loggermessage?view=aspnetcore-2.2
            if (Debugger.IsAttached)
            {
                loggingBuilder.AddDebug();
            }

            loggingBuilder.AddEventSourceLogger();
            loggingBuilder.AddEventLog();
            // loggingBuilder.AddTraceSource();

#pragma warning disable CC0022
            loggingBuilder.AddProvider(new ExceptionlessLoggerProvider(ExceptionlessClient.Default));
#pragma warning restore CC0022
        }
    }
}
