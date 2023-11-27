﻿// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Audit.Core;
using Audit.Core.Providers;
using Audit.WebApi;
using Ben.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Microsoft.OpenApi.Models;
using RimDev.ApplicationInsights.Filters.Processors;
using Swashbuckle.AspNetCore.SwaggerUI;
using Whipstaff.AspNetCore.Features.ApplicationStartup;
using Whipstaff.AspNetCore.Features.AuditNet;
using Whipstaff.AspNetCore.Features.DiagnosticListener;
using Whipstaff.AspNetCore.Features.Swagger;
using Whipstaff.AspNetCore.Swashbuckle;
using Whipstaff.Core.Mediatr;

namespace Whipstaff.AspNetCore
{
    /// <summary>
    /// Core Initialization logic.
    /// </summary>
    public abstract class BaseStartup : IWhipstaffWebAppStartup
    {
        /// <inheritdoc/>
        public abstract void ConfigureLogging(WebHostBuilderContext hostBuilderContext, ILoggingBuilder loggingBuilder);

        /// <inheritdoc/>
        public void ConfigureWebApplication(WebHostBuilderContext webHostBuilderContext, IApplicationBuilder applicationBuilder)
        {
            ArgumentNullException.ThrowIfNull(webHostBuilderContext);
            ArgumentNullException.ThrowIfNull(applicationBuilder);

            var env = applicationBuilder.ApplicationServices.GetService<IWebHostEnvironment>();
            if (env == null)
            {
                throw new InvalidOperationException("Failed to retrieve Environment Registration");
            }

            var logger = applicationBuilder.ApplicationServices.GetService<ILoggerFactory>();
            if (logger == null)
            {
                throw new InvalidOperationException("Failed to retrieve Logger Factory");
            }

            Configure(applicationBuilder, env, logger);
        }

        /// <inheritdoc/>
        public void ConfigureServices(WebHostBuilderContext hostBuilderContext, IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(hostBuilderContext);
            ArgumentNullException.ThrowIfNull(services);

            var configuration = hostBuilderContext.Configuration;

            _ = services.AddFeatureManagement();
            _ = services.AddMiddlewareAnalysis();

            ConfigureControllerService(services);

            ConfigureMediatrService(services);

            _ = services.AddProblemDetails();

            _ = services.AddAuthorization(ConfigureAuthorization);

            // new HealthChecksApplicationStartHelper().ConfigureService(services, configuration);
            var useSwagger = configuration.GetValue("useSwagger", false);

            if (useSwagger)
            {
                // taken from https://mderriey.com/2020/12/14/how-to-lock-down-csp-using-swachbuckle/
                _ = services.AddHttpContextAccessor();
                _ = services
                    .AddOptions<SwaggerUIOptions>()
                    .Configure<IHttpContextAccessor>((swaggerUiOptions, httpContextAccessor) => swaggerUiOptions.ApplyNonceToSwaggerIndexStream(httpContextAccessor));
                _ = services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                    c.OperationFilter<SwaggerClassMetaDataOperationFilter>();
                    c.OperationFilter<ProblemDetailOperationFilter>();
                    c.UseAllOfToExtendReferenceSchemas();
                    c.CustomSchemaIds(x => x.FullName);
                });
            }

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

        /// <summary>
        /// Gets the action to use when configuring the controllers.
        /// </summary>
        /// <returns>Action to execute, or null if no endpoints to be registered.</returns>
        protected abstract Action<IEndpointRouteBuilder>? GetOnUseEndpointsAction();

        /// <summary>
        /// Gets the mode to configure MVC services with.
        /// </summary>
        /// <returns>MVC Service Mode to use.</returns>
        protected abstract MvcServiceMode GetMvcServiceMode();

        /// <summary>
        /// Configures Authorization policies.
        /// </summary>
        /// <param name="authorizationOptions">Authorization options instance to modify.</param>
        protected abstract void ConfigureAuthorization(AuthorizationOptions authorizationOptions);

        /// <summary>
        /// Gets the data provider to use for audit logging.
        /// </summary>
        /// <returns>Audit Data Provider to use, if any.</returns>
        protected abstract AuditDataProvider? GetAuditDataProvider();

        /// <summary>
        /// Gets the swagger endpoints to register on the UI.
        /// </summary>
        /// <returns>Collection of Swagger endpoints.</returns>
        protected abstract IEnumerable<(string Url, string Name)>? GetSwaggerEndpoints();

        private static Func<IServiceCollection, Action<MvcOptions>, IMvcBuilder>? GetControllerFunc(MvcServiceMode mvcServiceMode)
        {
            return mvcServiceMode switch
            {
                MvcServiceMode.Basic => MvcServiceCollectionExtensions.AddControllers,
                MvcServiceMode.ControllersWithViews => MvcServiceCollectionExtensions.AddControllersWithViews,
                _ => null
            };
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

#if TBC
            if (env.IsDevelopment())
            {
                _ = app.UseBrowserLink();
                _ = app.UseDeveloperExceptionPage();
            }
            else
            {
                _ = app.UseExceptionHandler("/Home/Error");
            }
#endif

            _ = app.UseBlockingDetection();

            var version = new Version(0, 1, 1, 9999);

            /*
            ApmApplicationStartHelper.Configure(Configuration, app, version);
            */

#if TBC
            // taken this out whilst reviewing nonce logic for swashbuckle.
            var secureHeadersMiddlewareConfiguration = SecureHeadersMiddlewareExtensions.BuildDefaultConfiguration();
            _ = app.UseSecureHeadersMiddleware(secureHeadersMiddlewareConfiguration);
#else
            _ = app.UseSecurityHeaders(policyCollection =>
            {
                policyCollection.AddContentSecurityPolicy(csp =>
                {
                    // Only allow loading resources from this app by default
                    csp.AddDefaultSrc().Self();

                    // Allow nonce-enabled <style> tags
                    csp.AddStyleSrc()
                        .Self()
                        .WithNonce();

                    // Allow nonce-enabled <script> tags
                    csp.AddScriptSrc()
                        .Self()
                        .WithNonce();
                });
            });
#endif

            _ = app.UseStaticFiles();

            DoAuditNetConfiguration(app);

            var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
            var useSwagger = configuration.GetValue("useSwagger", false);
            if (useSwagger)
            {
                _ = app.UseSwagger();

                var swaggerEndpoints = GetSwaggerEndpoints();
                if (swaggerEndpoints != null)
                {
                    _ = app.UseSwaggerUI(c =>
                    {
                        foreach (var (url, name) in swaggerEndpoints)
                        {
                            c.SwaggerEndpoint(url, name);
                        }
                    });
                }
            }

            _ = app.UseRouting();

            var useEndpointsAction = GetOnUseEndpointsAction();
            if (useEndpointsAction != null)
            {
                _ = app.UseEndpoints(endpointRouteBuilder => useEndpointsAction(endpointRouteBuilder));
            }

            OnConfigure(app, env, loggerFactory);
        }

        private void DoAuditNetConfiguration(IApplicationBuilder applicationBuilder)
        {
            var provider = GetAuditDataProvider();

            if (provider == null)
            {
                return;
            }

            Audit.Core.Configuration.DataProvider = provider;

            _ = applicationBuilder.UseAuditMiddleware(configurator => configurator.DoFullAuditMiddlewareConfig());

            if (provider is FileDataProvider fileDataProvider)
            {
                // this was done so files don't get added to git
                // as the visual studio .gitignore ignores log folders.
                fileDataProvider.DirectoryPath = "log";
            }
        }

        private void ConfigureControllerService(IServiceCollection services)
        {
            var controllerAssemblies = GetControllerAssemblies();

            var mvcServiceMode = GetMvcServiceMode();
            var controllerFunc = GetControllerFunc(mvcServiceMode);

            if (controllerFunc == null)
            {
                return;
            }

            var mvcBuilder = controllerFunc(
                services,
                options =>
            {
                // if you have a load balancer in front, you can have an issue if there is no cache-control specified
                // where it assumes it can cache it because it doesn't say "Don't cache it" (BIG-IP, etc.)
                _ = options.CacheProfiles.TryAdd("nostore", new CacheProfile { NoStore = true });
            });

            foreach (var controllerAssembly in controllerAssemblies)
            {
                mvcBuilder = mvcBuilder.AddApplicationPart(controllerAssembly);
            }

            _ = mvcBuilder.AddControllersAsServices();
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
