namespace Dhgms.AspNetCoreContrib.Example.WebSite
{
    using Exceptionless;
    using Exceptionless.Extensions.Logging;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Logging;

    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging(ConfigureLogging)
                .Build();

        private static void ConfigureLogging(WebHostBuilderContext webHostBuilderContext, ILoggingBuilder loggingBuilder)
        {
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/loggermessage?view=aspnetcore-2.2
            loggingBuilder.AddConsole();
            loggingBuilder.AddDebug();
            loggingBuilder.AddEventSourceLogger();
            loggingBuilder.AddEventLog();
            //loggingBuilder.AddTraceSource();

#pragma warning disable CC0022
            loggingBuilder.AddProvider(new ExceptionlessLoggerProvider(ExceptionlessClient.Default));
#pragma warning restore CC0022
        }
    }
}
