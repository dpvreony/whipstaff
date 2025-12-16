// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using Audit.Core;
using Audit.Core.Providers;
using Microsoft.AspNetCore.Authentication;
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
using Whipstaff.AspNetCore;
using Whipstaff.AspNetCore.Features.ApplicationStartup;
using Whipstaff.EntityFramework.ModelCreation;
using Whipstaff.EntityFramework.RowVersionSaving;
using Whipstaff.Example.AspireServiceDefaults;
using Whipstaff.Mediator;
using Whipstaff.Testing;
using Whipstaff.Testing.Cqrs;
using Whipstaff.Testing.EntityFramework;
using Whipstaff.Testing.Mediator;

namespace Dhgms.AspNetCoreContrib.Example.WebApiApp
{
    /// <summary>
    /// Startup object for the WebAPI example Application.
    /// </summary>
    public sealed class Startup : BaseStartup
    {
        private readonly DbConnection _dbConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        public Startup()
        {
            _dbConnection = CreateInMemoryDatabase();
        }

        /// <inheritdoc />
        public override void ConfigureAspireServiceDefaults(IHostApplicationBuilder builder)
        {
            _ = builder.AddServiceDefaults();
        }

        /// <inheritdoc />
        public override void ConfigureLogging(
            ILoggingBuilder loggingBuilder,
            ConfigurationManager configuration,
            IWebHostEnvironment environment)
        {
        }

        /// <inheritdoc />
        protected override (string DefaultScheme, Action<AuthenticationBuilder, IConfiguration, IWebHostEnvironment> BuilderAction)? GetConfigureAuthenticationDetails()
        {
            return (
                "bearer",
                static (builder, _, _) => ConfigureAuthenticationScheme(builder));
        }

        /// <inheritdoc />
        protected override void OnConfigureServices(IServiceCollection serviceCollection)
        {
            _ = serviceCollection.AddSingleton<FakeAuditableCommandFactory>();
            _ = serviceCollection.AddSingleton<FakeAuditableQueryFactory>();
            _ = serviceCollection.AddSingleton<FakeCrudControllerLogMessageActions>();

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
        protected override IMediatorRegistration GetMediatorRegistration()
        {
            return new FakeMediatorRegistration();
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
            ArgumentNullException.ThrowIfNull(authorizationOptions);

            authorizationOptions.AddPolicy("ListPolicyName", builder => builder.RequireAssertion(_ => true).Build());
            authorizationOptions.AddPolicy("ViewPolicyName", builder => builder.RequireAssertion(_ => true).Build());
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

        private static void ConfigureAuthenticationScheme(AuthenticationBuilder authenticationBuilder)
        {
            _ = authenticationBuilder.AddBearerToken("bearer");
        }
    }
}
