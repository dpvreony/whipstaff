using Hellang.Middleware.ProblemDetails;
using Microsoft.Extensions.Logging;

namespace Dhgms.AspNetCoreContrib.Example.WebSite
{
    using System;
    using Audit.WebApi;
    using Dhgms.AspNetCoreContrib.Example.WebSite.Features.Apm;
    using Dhgms.AspNetCoreContrib.Example.WebSite.Features.Apm.HealthChecks;
    using Dhgms.AspNetCoreContrib.Fakes;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using OwaspHeaders.Core.Extensions;
    using Swashbuckle.AspNetCore.Swagger;

    public sealed class Startup : IStartup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetService<IHostingEnvironment>();
            var logger = app.ApplicationServices.GetService<ILoggerFactory>();

            Configure(app, env, logger);
        }

        private void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseProblemDetails();

            var version = new Version(0, 1, 1, 9999);
            ApmApplicationStartHelper.Configure(this.Configuration, app, env, version);

            var secureHeadersMiddlewareConfiguration = SecureHeadersMiddlewareExtensions.BuildDefaultConfiguration();
            app.UseSecureHeadersMiddleware(secureHeadersMiddlewareConfiguration);

            app.UseStaticFiles();

            app.UseAuditMiddleware(_ => _
                .FilterByRequest(rq => !rq.Path.Value.EndsWith("favicon.ico", StringComparison.OrdinalIgnoreCase))
                .WithEventType("{verb}:{url}")
                .IncludeHeaders()
                .IncludeRequestBody()
                .IncludeResponseBody());

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var fakeControllerAssembly = typeof(FakeCrudController).Assembly;

            services.AddProblemDetails();
            services.AddMvc().AddApplicationPart(fakeControllerAssembly);
            services.AddMediatR(fakeControllerAssembly);

            new HealthChecksApplicationStartHelper().ConfigureService(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                c.OperationFilter<SwaggerClassMetaDataOperationFilter>();
            });

            return services.BuildServiceProvider();
        }
    }
}
