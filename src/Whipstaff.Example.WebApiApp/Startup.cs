// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Whipstaff.AspNetCore;
using Whipstaff.Core.Mediatr;
using Whipstaff.Testing;
using Whipstaff.Testing.Cqrs;
using Whipstaff.Testing.MediatR;

namespace Dhgms.AspNetCoreContrib.Example.WebApiApp
{
    /// <summary>
    /// Startup object for the WebAPI example Application.
    /// </summary>
    public class Startup : BaseStartup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration object for the application instance.</param>
        public Startup(IConfiguration configuration)
            : base(configuration, true)
        {
        }

        /// <inheritdoc />
        protected override void OnConfigureServices(IServiceCollection serviceCollection)
        {
            _ = serviceCollection.AddSingleton<FakeAuditableCommandFactory>();
            _ = serviceCollection.AddSingleton<FakeAuditableQueryFactory>();
            _ = serviceCollection.AddSingleton<FakeCrudControllerLogMessageActions>();
        }

        /// <inheritdoc />
        protected override void OnConfigure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            ILoggerFactory loggerFactory)
        {
        }

        /// <inheritdoc />
        protected override Assembly[] GetControllerAssemblies()
        {
            return new[]
            {
                typeof(FakeCrudController).Assembly,
            };
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
                _ = endpoints.MapControllerRoute(
                    "create",
                    "api/{controller}",
                    new { action = "Post" },
                    new RouteValueDictionary(new { httpMethod = new HttpMethodRouteConstraint("POST") }));
                _ = endpoints.MapControllerRoute(
                    "read",
                    "api/{controller}/{id?}",
                    new { action = "Get" },
                    new RouteValueDictionary(new { httpMethod = new HttpMethodRouteConstraint("GET") }));
                _ = endpoints.MapControllerRoute(
                    "update",
                    "api/{controller}/{id?}",
                    new { action = "Patch" },
                    new RouteValueDictionary(new { httpMethod = new HttpMethodRouteConstraint("PATCH") }));
                _ = endpoints.MapControllerRoute(
                    "delete",
                    "api/{controller}/{id?}",
                    new { action = "Delete" },
                    new RouteValueDictionary(new { httpMethod = new HttpMethodRouteConstraint("DELETE") }));
            };
        }
    }
}
