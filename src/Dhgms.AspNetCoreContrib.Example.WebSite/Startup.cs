using Audit.Core.Providers;
using Dhgms.AspNetCoreContrib.Example.WebSite.Features.StartUp;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;
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
        private readonly MiniProfilerApplicationStartHelper _miniProfilerApplicationStartHelper;

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
            _miniProfilerApplicationStartHelper = new MiniProfilerApplicationStartHelper();
        }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetService<IHostingEnvironment>();
            var logger = app.ApplicationServices.GetService<ILoggerFactory>();

            this.Configure(app, env, logger);
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var fakeControllerAssembly = typeof(FakeCrudController).Assembly;
            var examplesAssembly = typeof(Startup).Assembly;

            //services.AddProblemDetails();
            services.AddMvc().AddApplicationPart(fakeControllerAssembly).SetCompatibilityVersion(CompatibilityVersion.Latest);
            services.AddMediatR(fakeControllerAssembly, examplesAssembly);

            services.AddAuthorization(configure => configure.AddPolicy("ViewSpreadSheet", builder => builder.RequireAssertion(_ => true).Build()));

            new HealthChecksApplicationStartHelper().ConfigureService(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                c.OperationFilter<SwaggerClassMetaDataOperationFilter>();
            });

            _miniProfilerApplicationStartHelper.ConfigureService(services);

            return services.BuildServiceProvider();
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
