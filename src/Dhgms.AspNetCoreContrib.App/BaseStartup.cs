using System;
using System.Reflection;
using Audit.WebApi;
using Dhgms.AspNetCoreContrib.Example.WebSite.Features.Apm;
using Dhgms.AspNetCoreContrib.Example.WebSite.Features.Apm.HealthChecks;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OwaspHeaders.Core.Extensions;
using Swashbuckle.AspNetCore.Swagger;
using Audit.Core.Providers;
using Dhgms.AspNetCoreContrib.Example.WebSite.Features.StartUp;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;

namespace Dhgms.AspNetCoreContrib.App
{
    public abstract class BaseStartup : IStartup
    {
        private readonly MiniProfilerApplicationStartHelper _miniProfilerApplicationStartHelper;

        protected BaseStartup(IConfiguration configuration)
        {
            this.Configuration = configuration;
            _miniProfilerApplicationStartHelper = new MiniProfilerApplicationStartHelper();
        }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetService<IWebHostEnvironment>();
            var logger = app.ApplicationServices.GetService<ILoggerFactory>();

            this.Configure(app, env, logger);
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddFeatureManagement();

            var controllerAssemblies = GetControllerAssemblies();
            foreach (var controllerAssembly in controllerAssemblies)
            {
                services.AddMvc()
                    .AddApplicationPart(controllerAssembly)
                    .SetCompatibilityVersion(CompatibilityVersion.Latest);
            }

            var mediatrAssemblies = GetMediatrAssemblies();

            if (mediatrAssemblies?.Length > 0)
            {
                services.AddMediatR(mediatrAssemblies);
            }

            //services.AddProblemDetails();

            services.AddAuthorization(configure => configure.AddPolicy("ViewSpreadSheet", builder => builder.RequireAssertion(_ => true).Build()));

            new HealthChecksApplicationStartHelper().ConfigureService(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                //c.OperationFilter<SwaggerClassMetaDataOperationFilter>();
            });

            _miniProfilerApplicationStartHelper.ConfigureService(services);

            return services.BuildServiceProvider();
        }

        protected abstract Assembly[] GetControllerAssemblies();

        protected abstract Assembly[] GetMediatrAssemblies();

        private void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
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

            //app.UseProblemDetails();

            var version = new Version(0, 1, 1, 9999);
            ApmApplicationStartHelper.Configure(this.Configuration, app, env, version);

            //var secureHeadersMiddlewareConfiguration = SecureHeadersMiddlewareExtensions.BuildDefaultConfiguration();
            //app.UseSecureHeadersMiddleware(secureHeadersMiddlewareConfiguration);

            app.UseStaticFiles();

            app.UseAuditMiddleware(_ => _
                .FilterByRequest(rq => !rq.Path.Value.EndsWith("favicon.ico", StringComparison.OrdinalIgnoreCase))
                .WithEventType("{verb}:{url}")
                .IncludeHeaders()
                .IncludeRequestBody()
                .IncludeResponseBody());
            var fileDataProvider = Audit.Core.Configuration.DataProvider as FileDataProvider;
            if (fileDataProvider != null)
            {
                // this was done so files don't get added to git
                // as the visual studio .gitignore ignores log folders.
                fileDataProvider.DirectoryPath = "log";
            }

            this._miniProfilerApplicationStartHelper.ConfigureApplication(app);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "get",
                    template: "api/{controller}/{id?}",
                    defaults: new {action = "GetAsync"},
                    constraints: new RouteValueDictionary(new {httpMethod = new HttpMethodRouteConstraint("GET")}));
                routes.MapRoute(
                    name: "post",
                    template: "api/{controller}/{id?}",
                    defaults: new {action = "PostAsync"},
                    constraints: new RouteValueDictionary(new {httpMethod = new HttpMethodRouteConstraint("POST")}));
                routes.MapRoute(
                    name: "delete",
                    template: "api/{controller}/{id?}",
                    defaults: new {action = "DeleteAsync"},
                    constraints: new RouteValueDictionary(new {httpMethod = new HttpMethodRouteConstraint("DELETE")}));
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
    }
}
