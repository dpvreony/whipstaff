// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Reflection;
using Audit.Core.Providers;
using Audit.WebApi;
using Ben.Diagnostics;
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
using Microsoft.OpenApi.Models;
using OwaspHeaders.Core.Extensions;
using RimDev.ApplicationInsights.Filters.Processors;
using Whipstaff.AspNetCore.Features.ApiAuthorization;
using Whipstaff.AspNetCore.Features.Apm.HealthChecks;
using Whipstaff.AspNetCore.Features.DiagnosticListener;
using Whipstaff.AspNetCore.Features.StartUp;
using Whipstaff.AspNetCore.Features.Swagger;
using Whipstaff.AspNetCore.Swashbuckle;
using Whipstaff.Core.Mediatr;

namespace Whipstaff.AspNetCore
{
    /// <summary>
    /// Core Initialization logic.
    /// </summary>
    public abstract class BaseStartup
    {
        private readonly MiniProfilerApplicationStartHelper _miniProfilerApplicationStartHelper;
        private readonly bool _useSwagger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseStartup"/> class.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        /// <param name="useSwagger">Flag indicating whether to enable swagger.</param>
        protected BaseStartup(
            IConfiguration configuration,
            bool useSwagger)
        {
            Configuration = configuration;
            _miniProfilerApplicationStartHelper = new MiniProfilerApplicationStartHelper();
            _useSwagger = useSwagger;
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
            if (env == null)
            {
                throw new InvalidOperationException("Failed to retrieve Environment Registration");
            }

            var logger = app.ApplicationServices.GetService<ILoggerFactory>();
            if (logger == null)
            {
                throw new InvalidOperationException("Failed to retrieve Logger Factory");
            }

            Configure(app, env, logger);
        }

        /// <summary>
        /// Configure the services for the web application.
        /// </summary>
        /// <param name="services">DI services collection instance.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            _ = services.AddFeatureManagement();
            _ = services.AddMiddlewareAnalysis();

            ConfigureControllerService(services);

            ConfigureMediatrService(services);

            _ = services.AddProblemDetails();

            _ = services.AddAuthorization(ConfigureAuthorization);

            new HealthChecksApplicationStartHelper().ConfigureService(services, Configuration);

            if (_useSwagger)
            {
                _ = services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                    c.OperationFilter<SwaggerClassMetaDataOperationFilter>();
                    c.OperationFilter<ProblemDetailOperationFilter>();
                    c.UseAllOfToExtendReferenceSchemas();
                    c.CustomSchemaIds(x => x.FullName);
                });
            }

            /*
            _miniProfilerApplicationStartHelper.ConfigureService(services, Configuration);
            */

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

            _ = services.AddApplicationInsightsTelemetry();
            _ = services.AddApplicationInsightsTelemetryProcessor<IgnoreHangfireTelemetry>();
            _ = services.AddApplicationInsightsTelemetryProcessor<IgnorePathsTelemetry>();
            _ = services.AddApplicationInsightsTelemetryProcessor<RemoveHttpUrlPasswordsTelemetry>();

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
        /// Gets a mediatr registration object. This is used to avoid reflection.
        /// </summary>
        /// <returns>Array of assemblies.</returns>
        protected abstract IMediatrRegistration GetMediatrRegistration();

        private static void ConfigureAuthorization(AuthorizationOptions authorizationOptions)
        {
            authorizationOptions.AddPolicy("ListPolicyName", builder => builder.RequireAssertion(_ => true).Build());
            authorizationOptions.AddPolicy("ViewSpreadSheet", builder => builder.RequireAssertion(_ => true).Build());
            authorizationOptions.AddPolicy("ViewPolicyName", builder => builder.RequireAssertion(_ => true).Build());
            authorizationOptions.AddPolicy("ControllerAuthenticatedUser", builder => builder.RequireAuthenticatedUser().Build());
        }

        private void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            ILoggerFactory loggerFactory)
        {
            var diagnosticListener = app.ApplicationServices.GetService<DiagnosticListener>();
            var logDiagnosticListenerLogger = loggerFactory.CreateLogger<LogDiagnosticListener>();
            _ = diagnosticListener.SubscribeWithAdapter(new LogDiagnosticListener(logDiagnosticListenerLogger));

            var logger = loggerFactory.CreateLogger<BaseStartup>();
#pragma warning disable CA1848 // Use the LoggerMessage delegates
            logger.LogInformation("Starting configuration");
#pragma warning restore CA1848 // Use the LoggerMessage delegates

            if (env.IsDevelopment())
            {
                _ = app.UseBrowserLink();
                _ = app.UseDeveloperExceptionPage();
            }
            else
            {
                _ = app.UseExceptionHandler("/Home/Error");
            }

            _ = app.UseBlockingDetection();

            var version = new Version(0, 1, 1, 9999);

            /*
            ApmApplicationStartHelper.Configure(Configuration, app, version);
            */

            var secureHeadersMiddlewareConfiguration = SecureHeadersMiddlewareExtensions.BuildDefaultConfiguration();
            _ = app.UseSecureHeadersMiddleware(secureHeadersMiddlewareConfiguration);

            _ = app.UseStaticFiles();

            _ = app.UseAuditMiddleware(_ => _
                .FilterByRequest(rq =>
                {
                    var pathValue = rq.Path.Value;
                    return pathValue != null && !pathValue.EndsWith("favicon.ico", StringComparison.OrdinalIgnoreCase);
                })
                .WithEventType("{verb}:{url}")
                .IncludeHeaders()
                .IncludeRequestBody()
                .IncludeResponseHeaders()
                .IncludeResponseBody());

            if (Audit.Core.Configuration.DataProvider is FileDataProvider fileDataProvider)
            {
                // this was done so files don't get added to git
                // as the visual studio .gitignore ignores log folders.
                fileDataProvider.DirectoryPath = "log";
            }

            /*
            _miniProfilerApplicationStartHelper.ConfigureApplication(app);
            */

            if (_useSwagger)
            {
                _ = app.UseSwagger();
                _ = app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }

            _ = app.UseRouting();

            _ = app.UseEndpoints(endpoints =>
            {
                _ = endpoints.MapControllerRoute(
                    "get",
                    "api/{controller}/{id?}",
                    new { action = "Get" },
                    new RouteValueDictionary(new { httpMethod = new HttpMethodRouteConstraint("GET") }));
            });

            OnConfigure(app, env, loggerFactory);
        }

        private void ConfigureControllerService(IServiceCollection services)
        {
            var controllerAssemblies = GetControllerAssemblies();
            foreach (var controllerAssembly in controllerAssemblies)
            {
                _ = services.AddControllers(options =>
                    {
                        options.Conventions.Add(new AddAuthorizePolicyControllerConvention());

                        // if you have a load balancer in front, you can have an issue if there is no cache-control specified
                        // where it assumes it can cache it because it doesn't say "Don't cache it" (BIG-IP, etc.)
                        options.CacheProfiles.Add("nostore", new CacheProfile { NoStore = true });
                    })
                    .AddApplicationPart(controllerAssembly)
                    .AddControllersAsServices();
            }
        }

        private void ConfigureMediatrService(IServiceCollection services)
        {
            var mediatrRegistration = GetMediatrRegistration();
            MediatrHelpers.RegisterMediatrWithExplicitTypes(
                services,
                null,
                new MediatRServiceConfiguration(),
                mediatrRegistration);
        }
    }
}
