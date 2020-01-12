// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Reflection;
using Audit.Core.Providers;
using Audit.WebApi;
using Ben.Diagnostics;
using Dhgms.AspNetCoreContrib.App.Features.Apm;
using Dhgms.AspNetCoreContrib.App.Features.Apm.HealthChecks;
using Dhgms.AspNetCoreContrib.App.Features.DiagnosticListener;
using Dhgms.AspNetCoreContrib.App.Features.StartUp;
using Dhgms.AspNetCoreContrib.App.Features.Swagger;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using OwaspHeaders.Core.Extensions;
using RimDev.ApplicationInsights.Filters;
using RimDev.ApplicationInsights.Filters.Processors;
using Swashbuckle.AspNetCore.Swagger;

namespace Dhgms.AspNetCoreContrib.App
{
    /// <summary>
    /// Core Initialization logic.
    /// </summary>
    public abstract class BaseStartup
    {
        private readonly MiniProfilerApplicationStartHelper _miniProfilerApplicationStartHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseStartup"/> class.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        protected BaseStartup(IConfiguration configuration)
        {
            Configuration = configuration;
            _miniProfilerApplicationStartHelper = new MiniProfilerApplicationStartHelper();
        }

        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configure the web application.
        /// </summary>
        /// <param name="app">Application Builder.</param>
        public void Configure(IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var env = app.ApplicationServices.GetService<IWebHostEnvironment>();
            var logger = app.ApplicationServices.GetService<ILoggerFactory>();

            Configure(app, env, logger);
        }

        /// <summary>
        /// Configure the services for the web application.
        /// </summary>
        /// <param name="services">DI services collection instance.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddFeatureManagement();
            services.AddMiddlewareAnalysis();

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

            services.AddProblemDetails();

            services.AddAuthorization(configure => configure.AddPolicy("ViewSpreadSheet", builder => builder.RequireAssertion(_ => true).Build()));

            new HealthChecksApplicationStartHelper().ConfigureService(services, Configuration);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                c.OperationFilter<SwaggerClassMetaDataOperationFilter>();
            });

            _miniProfilerApplicationStartHelper.ConfigureService(services, Configuration);

            /*
            services.AddSingleton(new IgnoreHangfireTelemetryOptions
            {
                SqlConnectionString = Configuration.GetConnectionString("hangfire")
            });
            services.AddSingleton(new IgnorePathsTelemetryOptions
            {
                Paths = new[] { "/_admin" }
            });
            */

            services.AddApplicationInsightsTelemetry();
            services.AddApplicationInsightsTelemetryProcessor<IgnoreHangfireTelemetry>();
            services.AddApplicationInsightsTelemetryProcessor<IgnorePathsTelemetry>();
            services.AddApplicationInsightsTelemetryProcessor<RemoveHttpUrlPasswordsTelemetry>();

            OnConfigureServices(services);
        }

        /// <summary>
        /// Configure app specific services.
        /// </summary>
        /// <param name="serviceCollection">Service Collection to modify.</param>
        protected abstract void OnConfigureServices(IServiceCollection serviceCollection);

        /// <summary>
        /// Carry out application specific configuration.
        /// </summary>
        /// <param name="app">Application instance.</param>
        /// <param name="env">Web Host Environment instance.</param>
        /// <param name="loggerFactory">Logger factory instance.</param>
        protected abstract void OnConfigure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            ILoggerFactory loggerFactory);

        /// <summary>
        /// Gets the assemblies that contain controllers.
        /// </summary>
        /// <returns>Array of assemblies.</returns>
        protected abstract Assembly[] GetControllerAssemblies();

        /// <summary>
        /// Gets the assemblies that contain mediatr command handlers.
        /// </summary>
        /// <returns>Array of assemblies.</returns>
        protected abstract Assembly[] GetMediatrAssemblies();

        private void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            ILoggerFactory loggerFactory)
        {
            var diagnosticListener = app.ApplicationServices.GetService<DiagnosticListener>();
            var logDiagnosticListenerLogger = loggerFactory.CreateLogger<LogDiagnosticListener>();
            diagnosticListener.SubscribeWithAdapter(new LogDiagnosticListener(logDiagnosticListenerLogger));

            var logger = loggerFactory.CreateLogger<BaseStartup>();
            logger.LogInformation("Starting configuration");

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseBlockingDetection();
            app.UseProblemDetails();

            var version = new Version(0, 1, 1, 9999);

            /*
            ApmApplicationStartHelper.Configure(Configuration, app, version);
            */

            var secureHeadersMiddlewareConfiguration = SecureHeadersMiddlewareExtensions.BuildDefaultConfiguration();
            app.UseSecureHeadersMiddleware(secureHeadersMiddlewareConfiguration);

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

            _miniProfilerApplicationStartHelper.ConfigureApplication(app);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "get",
                    template: "api/{controller}/{id?}",
                    defaults: new { action = "GetAsync" },
                    constraints: new RouteValueDictionary(new { httpMethod = new HttpMethodRouteConstraint("GET") }));
                routes.MapRoute(
                    name: "post",
                    template: "api/{controller}",
                    defaults: new { action = "PostAsync" },
                    constraints: new RouteValueDictionary(new { httpMethod = new HttpMethodRouteConstraint("POST") }));
                routes.MapRoute(
                    name: "post",
                    template: "api/{controller}/{id}",
                    defaults: new { action = "PutAsync" },
                    constraints: new RouteValueDictionary(new { httpMethod = new HttpMethodRouteConstraint("PUT") }));
                routes.MapRoute(
                    name: "delete",
                    template: "api/{controller}/{id}",
                    defaults: new { action = "DeleteAsync" },
                    constraints: new RouteValueDictionary(new { httpMethod = new HttpMethodRouteConstraint("DELETE") }));
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            OnConfigure(app, env, loggerFactory);
        }
    }
}
