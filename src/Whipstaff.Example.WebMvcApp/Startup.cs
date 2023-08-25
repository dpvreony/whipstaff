// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RimDev.Stuntman.Core;
using Whipstaff.AspNetCore;
using Whipstaff.Core.Mediatr;
using Whipstaff.Testing;
using Whipstaff.Testing.Cqrs;
using Whipstaff.Testing.EntityFramework;
using Whipstaff.Testing.MediatR;

namespace Dhgms.AspNetCoreContrib.Example.WebMvcApp
{
    /// <summary>
    /// Start up logic for the sample Web MVC app.
    /// </summary>
    public sealed class Startup : BaseStartup
    {
        private readonly StuntmanOptions _stuntmanOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        public Startup()
        {
            _stuntmanOptions = new StuntmanOptions();
        }

        /// <inheritdoc />
        public override void ConfigureLogging(WebHostBuilderContext hostBuilderContext, ILoggingBuilder loggingBuilder)
        {
        }

        /// <inheritdoc />
        protected override void OnConfigureServices(IServiceCollection serviceCollection)
        {
            _ = serviceCollection.AddSingleton<FakeAuditableCommandFactory>();
            _ = serviceCollection.AddSingleton<FakeAuditableQueryFactory>();
            _ = serviceCollection.AddSingleton<FakeCrudControllerLogMessageActions>();
            _ = serviceCollection.AddControllersWithViews();

            /*
            serviceCollection.AddStuntman(_stuntmanOptions);
            */
            var databaseName = Guid.NewGuid().ToString();
            _ = serviceCollection.AddTransient(_ => new DbContextOptionsBuilder<FakeDbContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options);
        }

        /// <inheritdoc />
        protected override void OnConfigure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            ILoggerFactory loggerFactory)
        {
            /*
            app.UseStuntman(_stuntmanOptions);
            */
        }

        /// <inheritdoc />
        protected override Assembly[] GetControllerAssemblies()
        {
            return Array.Empty<Assembly>();
        }

        /// <inheritdoc />
        protected override IMediatrRegistration GetMediatrRegistration()
        {
            return new FakeMediatrRegistration();
        }

        /// <inheritdoc/>
        protected override Action<IEndpointRouteBuilder>? GetOnUseEndpointsAction()
        {
            return endpoints =>
            {
                /*
                _ = endpoints.Map("home", context =>
                {
                    context.Response.StatusCode = 404;
                    return Task.CompletedTask;
                });
                */

                _ = endpoints.MapControllerRoute(
                    "create",
                    "{controller=Home}",
                    new { action = "Post" },
                    new RouteValueDictionary(new { httpMethod = new HttpMethodRouteConstraint("POST") }));

                _ = endpoints.MapControllerRoute(
                    "read",
                    "{controller=Home}/{id?}",
                    new { action = "Get" },
                    new RouteValueDictionary(new { httpMethod = new HttpMethodRouteConstraint("GET") }));

                _ = endpoints.MapControllerRoute(
                    "update",
                    "{controller=Home}/{id?}",
                    new { action = "Patch" },
                    new RouteValueDictionary(new { httpMethod = new HttpMethodRouteConstraint("PATCH") }));

                _ = endpoints.MapControllerRoute(
                    "delete",
                    "{controller=Home}/{id?}",
                    new { action = "Delete" },
                    new RouteValueDictionary(new { httpMethod = new HttpMethodRouteConstraint("DELETE") }));
            };
        }
    }
}
