// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using Audit.Core;
using Audit.Core.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Whipstaff.AspNetCore;
using Whipstaff.AspNetCore.Features.ApplicationStartup;
using Whipstaff.Core.Mediatr;
using Whipstaff.Testing;
using Whipstaff.Testing.Cqrs;
using Whipstaff.Testing.EntityFramework;
using Whipstaff.Testing.MediatR;

namespace Dhgms.AspNetCoreContrib.Example.WebApiApp
{
    /// <summary>
    /// Startup object for the WebAPI example Application.
    /// </summary>
    public sealed class Startup : BaseStartup
    {
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
                _ = endpoints.DoCrudMapControllerRoute(
                    "api/{controller}",
                    null);
            };
        }

        /// <inheritdoc/>
        protected override MvcServiceMode GetMvcServiceMode()
        {
            return MvcServiceMode.Basic;
        }

        /// <inheritdoc />
        protected override void ConfigureAuthorization(AuthorizationOptions authorizationOptions)
        {
        }

        /// <inheritdoc />
        protected override AuditDataProvider? GetAuditDataProvider()
        {
            return new InMemoryDataProvider();
        }

        /// <inheritdoc />
        protected override IEnumerable<(string Url, string Name)> GetSwaggerEndpoints()
        {
            return Array.Empty<(string Url, string Name)>();
        }
    }
}
