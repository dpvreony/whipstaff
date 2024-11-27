// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Whipstaff.Example.AspireServiceDefaults
{
    /// <summary>
    /// Extension methods for configuring services.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Add service defaults to the host builder.
        /// </summary>
        /// <param name="builder">Host builder to modify.</param>
        /// <returns>The <see cref="IHostApplicationBuilder"/> passed in, so it can be used in a fluent style.</returns>
        public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            _ = builder.ConfigureOpenTelemetry();

            _ = builder.AddDefaultHealthChecks();

            var services = builder.Services;

            _ = services.AddServiceDiscovery();

            _ = services.ConfigureHttpClientDefaults(http =>
            {
                // Turn on resilience by default
                _ = http.AddStandardResilienceHandler();

                // Turn on service discovery by default
                _ = http.AddServiceDiscovery();
            });

            return builder;
        }

        /// <summary>
        /// Configure OpenTelemetry for the application.
        /// </summary>
        /// <param name="builder">Host builder to modify.</param>
        /// <returns>The <see cref="IHostApplicationBuilder"/> passed in, so it can be used in a fluent style.</returns>
        public static IHostApplicationBuilder ConfigureOpenTelemetry(this IHostApplicationBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            _ = builder.Logging.AddOpenTelemetry(logging =>
            {
                logging.IncludeFormattedMessage = true;
                logging.IncludeScopes = true;
            });

            _ = builder.Services.AddOpenTelemetry()
                .WithMetrics(metrics =>
                {
                    _ = metrics.AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddRuntimeInstrumentation();
                })
                .WithTracing(tracing =>
                {
                    _ = tracing.AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation();
                });

            _ = builder.AddOpenTelemetryExporters();

            return builder;
        }

        /// <summary>
        /// Add default health checks to the application.
        /// </summary>
        /// <param name="builder">Host builder to modify.</param>
        /// <returns>The <see cref="IHostApplicationBuilder"/> passed in, so it can be used in a fluent style.</returns>
        public static IHostApplicationBuilder AddDefaultHealthChecks(this IHostApplicationBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            // Add a default liveness check to ensure app is responsive
            _ = builder.Services.AddHealthChecks()
                .AddCheck(
                    "self",
                    () => HealthCheckResult.Healthy(),
                    ["live"]);

            return builder;
        }

        /// <summary>
        /// Map default endpoints for the application.
        /// </summary>
        /// <param name="app">Web application instance to configure.</param>
        /// <returns>The <see cref="WebApplication"/> that was passed in, so it can be used in a fluent style.</returns>
        public static WebApplication MapDefaultEndpoints(this WebApplication app)
        {
            ArgumentNullException.ThrowIfNull(app);

            // Adding health checks endpoints to applications in non-development environments has security implications.
            // See https://aka.ms/dotnet/aspire/healthchecks for details before enabling these endpoints in non-development environments.
            if (app.Environment.IsDevelopment())
            {
                // All health checks must pass for app to be considered ready to accept traffic after starting
                _ = app.MapHealthChecks("/health");

                // Only health checks tagged with the "live" tag must pass for app to be considered alive
                _ = app.MapHealthChecks("/alive", new HealthCheckOptions
                {
                    Predicate = r => r.Tags.Contains("live")
                });
            }

            return app;
        }

        private static IHostApplicationBuilder AddOpenTelemetryExporters(this IHostApplicationBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

            if (useOtlpExporter)
            {
                _ = builder.Services.AddOpenTelemetry().UseOtlpExporter();
            }

            // Uncomment the following lines to enable the Azure Monitor exporter (requires the Azure.Monitor.OpenTelemetry.AspNetCore package)
            if (!string.IsNullOrEmpty(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
            {
                _ = builder.Services.AddOpenTelemetry()
                    .UseAzureMonitor();
            }

            return builder;
        }
    }
}
