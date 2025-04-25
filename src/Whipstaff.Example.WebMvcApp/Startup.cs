// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Threading.Tasks;
using Audit.Core;
using Audit.Core.Providers;
using Dhgms.AspNetCoreContrib.Example.WebMvcApp.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
#if stuntman
using RimDev.Stuntman.Core;
#endif
using Whipstaff.AspNetCore;
using Whipstaff.AspNetCore.Features.ApplicationStartup;
using Whipstaff.EntityFramework.ModelCreation;
using Whipstaff.EntityFramework.RowVersionSaving;
using Whipstaff.Example.AspireServiceDefaults;
using Whipstaff.MediatR;
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
#if stuntman
        private readonly StuntmanOptions _stuntmanOptions;
#endif
        private readonly DbConnection _dbConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        public Startup()
        {
#if stuntman
            _stuntmanOptions = new StuntmanOptions();
#endif
            _dbConnection = CreateInMemoryDatabase();
        }

        /// <inheritdoc />
        public override void ConfigureAspireServiceDefaults(IHostApplicationBuilder builder)
        {
            _ = builder.AddServiceDefaults();
        }

        /// <inheritdoc />
        public override void ConfigureLogging(ILoggingBuilder loggingBuilder, ConfigurationManager configuration, IWebHostEnvironment environment)
        {
        }

        /// <inheritdoc />
        protected override void OnConfigureServices(IServiceCollection serviceCollection)
        {
            _ = serviceCollection.AddSingleton<FakeAuditableCommandFactory>();
            _ = serviceCollection.AddSingleton<FakeAuditableQueryFactory>();
            _ = serviceCollection.AddSingleton<FakeCrudControllerLogMessageActions>();

#if stuntman
            serviceCollection.AddStuntman(_stuntmanOptions);
#endif
            _ = serviceCollection.AddTransient(_ => new DbContextOptionsBuilder<FakeDbContext>()
                .UseSqlite(_dbConnection)
                .AddInterceptors(new RowVersionSaveChangesInterceptor())
                .Options);
            _ = serviceCollection.AddSingleton<Func<IModelCreator<FakeDbContext>>>(x =>
                () => new SqliteFakeDbContextModelCreator());
        }

        /// <inheritdoc />
        protected override void OnConfigure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            ILoggerFactory loggerFactory)
        {
#if stuntman
            app.UseStuntman(_stuntmanOptions);
#endif

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

                _ = endpoints.MapControllerRoute(
                    "DefaultControllerRoute",
                    "{controller=Home}/{action=Get}");

#if TBC
                _ = endpoints.DoCrudMapControllerRoute(
                    "{controller=Home}",
                    null);
#endif
            };
        }

        /// <inheritdoc/>
        protected override MvcServiceMode GetMvcServiceMode()
        {
            return MvcServiceMode.ControllersWithViews;
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

        private static SqliteConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");

            connection.Open();

            return connection;
        }
    }
}
