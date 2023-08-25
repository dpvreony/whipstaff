// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reflection;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Example.WebMvcApp.Controllers;
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
using Whipstaff.AspNetCore.Features.ApplicationStartup;
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

            serviceCollection.AddStuntman(_stuntmanOptions);
            var databaseName = Guid.NewGuid().ToString();
            _ = serviceCollection.AddTransient(_ => new DbContextOptionsBuilder<FakeDbContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options);

            // TODO: need to allow base class to control this, as it's having part of it called twice at the moment.
            _ = serviceCollection.AddControllersWithViews();
        }

        /// <inheritdoc />
        protected override void OnConfigure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            ILoggerFactory loggerFactory)
        {
            app.UseStuntman(_stuntmanOptions);

            _ = app.UseStaticFiles();
        }

        /// <inheritdoc />
        protected override Assembly[] GetControllerAssemblies()
        {
            return new[]
            {
                typeof(HomeController).Assembly,
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
                _ = endpoints.Map("home", context =>
                {
                    context.Response.StatusCode = 404;
                    return Task.CompletedTask;
                });

                _ = endpoints.DoCrudMapControllerRoute(
                    "{controller=Home}",
                    null);
            };
        }
    }
}
