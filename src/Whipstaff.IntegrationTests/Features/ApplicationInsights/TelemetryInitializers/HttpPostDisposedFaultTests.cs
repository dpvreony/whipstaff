// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if TBC
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Example.WebApiApp;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Whipstaff.AspNetCore.Features.ApplicationStartup;
using Whipstaff.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Whipstaff.IntegrationTests.Features.ApplicationInsights.TelemetryInitializers
{
    /// <summary>
    /// Integration Tests for tracking the behaviour of Application Insights telemetry initializers
    /// where the HTTP Request body is unavailable due to it already being disposed.
    /// We're keeping this test here as a reminder that we need to keep an eye on this behaviour in case
    /// it is fixed.
    /// </summary>
    public sealed class HttpPostDisposedFaultTests : TestWithLoggingBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpPostDisposedFaultTests"/> class.
        /// </summary>
        /// <param name="output">XUnit Logging output helper.</param>
        public HttpPostDisposedFaultTests(ITestOutputHelper output)
            : base(output)
        {
        }

        /// <summary>
        /// Tests that a Web app using API Controllers throws.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public Task ApiControllerThrows()
        {
            throw new NotImplementedException("This test is not yet implemented");
#if TBC
            using (var testServer = new TestServer(builder))
            {
                await TestAgainstTestServer(testServer).ConfigureAwait(false);
            }
#endif
        }

        /// <summary>
        /// Tests that a Web app using MVC Controllers throws.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public Task MvcControllerThrows()
        {
            throw new NotImplementedException("This test is not yet implemented");
#if TBC
            using (var testServer = new TestServer(builder))
            {
                await TestAgainstTestServer(testServer).ConfigureAwait(false);
            }
#endif
        }

        /// <summary>
        /// Tests that a Web app using Minimal API throws.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public Task MinimalApiThrows()
        {
            throw new NotImplementedException("This test is not yet implemented");
            /*
            var webApplicationBuilder = GetWebApplicationBuilder(LoggerFactory);
            using (var testServer = new TestServer())
            {
                _ = webApplicationBuilder.WebHost.UseServer(testServer);
                var app = webApplicationBuilder.Build();
                _ = app.MapPost("/api/entity", MinimalApiPost);
                await TestAgainstTestServer(testServer).ConfigureAwait(false);
            }
            */
        }

        /// <summary>
        /// Tests that a Web app using branch pipeline middleware throws.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Fact]
        public async Task BranchPipelinesThrow()
        {
            var builder = GetWebHostBuilder(LoggerFactory);

            using (var testServer = new TestServer(builder))
            {
                await TestAgainstTestServer(testServer).ConfigureAwait(false);
            }
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            _ = serviceCollection.AddSingleton(new TelemetryExceptionTracker());
            _ = serviceCollection.AddSingleton<ITelemetryInitializer, HttpPostExceptionTelemetryInitializer>();
            _ = serviceCollection.Configure<TelemetryConfiguration>(config => { });
            _ = serviceCollection.AddApplicationInsightsTelemetry(options => { });
            /*
            _ = serviceCollection.AddApplicationInsightsTelemetry(new ApplicationInsightsServiceOptions
            {
                ConnectionString = "InstrumentationKey=00000000-0000-0000-0000-000000000000"
            });
            */
        }

        private static void ConfigureLogging(ILoggingBuilder loggingBuilder, ILoggerFactory loggerFactory)
        {
            _ = loggingBuilder.AddApplicationInsights();
            _ = loggingBuilder.Services.AddSingleton(loggerFactory);
        }

        private static void ConfigureApp(WebHostBuilderContext webHostBuilderContext, IApplicationBuilder applicationBuilder)
        {
            _ = applicationBuilder.MapWhen(IsMermaidPost, AppConfiguration);
        }

        private static bool IsMermaidPost(HttpContext arg)
        {
            var request = arg.Request;

            return request.Method.Equals("POST", StringComparison.Ordinal) &&
                   request.Path.Equals("/api/entity", StringComparison.OrdinalIgnoreCase);
        }

        private static void AppConfiguration(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Run(Handler);
        }

        private static async Task Handler(HttpContext context)
        {
            var response = context.Response;
            response.StatusCode = 200;
            response.ContentType = "application/json";
            await response.WriteAsJsonAsync("Hello World!").ConfigureAwait(false);
        }

        private static IWebHostBuilder GetWebHostBuilder(ILoggerFactory loggerFactory)
        {
            var builder = new WebHostBuilder();

            _ = builder.ConfigureAppConfiguration(configurationBuilder => DoConfigureAppConfiguration(configurationBuilder))
                .ConfigureLogging(loggingBuilder => ConfigureLogging(
                    loggingBuilder,
                    loggerFactory))
                .ConfigureServices((_, serviceCollection) => ConfigureServices(serviceCollection))
                .Configure((webHostBuilderContext, applicationBuilder) => ConfigureApp(
                    webHostBuilderContext,
                    applicationBuilder));

            return builder;
        }

        private static void DoConfigureAppConfiguration(IConfigurationBuilder configurationBuilder)
        {
            _ = configurationBuilder.AddApplicationInsightsSettings("InstrumentationKey=00000000-0000-0000-0000-000000000000", true);
        }

        private static WebApplicationBuilder GetWebApplicationBuilder(ILoggerFactory loggerFactory)
        {
            var webApplicationBuilder = WebApplication.CreateBuilder();
            ConfigureLogging(webApplicationBuilder.Logging, loggerFactory);
            ConfigureServices(webApplicationBuilder.Services);
            return webApplicationBuilder;
        }

        private static Task<IResult> MinimalApiPost([FromBody] FakeRequestDto fakeRequestDto)
        {
            return Task.FromResult(Results.Ok());
        }

        private static async Task TestAgainstTestServer(TestServer testServer)
        {
            var client = testServer.CreateClient();

            var testObject = new
            {
                Name = "Test",
                Value = "Test",
            };

            var httpResponse = await client.PostAsJsonAsync("/api/entity", testObject).ConfigureAwait(false);

            // even if app insights fails it should still be a HTTP 200.
            Assert.Equal(200, (int)httpResponse.StatusCode);

            var telemetryExceptionTracker = testServer.Services.GetRequiredService<TelemetryExceptionTracker>();

            Assert.Equal(1, telemetryExceptionTracker.InvokeCount);
            _ = Assert.Single(telemetryExceptionTracker.Exceptions);
        }
    }
}
#endif
