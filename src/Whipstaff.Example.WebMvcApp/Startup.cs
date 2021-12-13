// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RimDev.Stuntman.Core;
using Whipstaff.AspNetCore;
using Whipstaff.Core.Mediatr;
using Whipstaff.Testing;
using Whipstaff.Testing.EntityFramework;
using Whipstaff.Testing.MediatR;

namespace Dhgms.AspNetCoreContrib.Examples.WebMvcApp
{
    /// <summary>
    /// Start up logic for the sample Web MVC app.
    /// </summary>
    public class Startup : BaseStartup
    {
        private readonly StuntmanOptions _stuntmanOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        public Startup(IConfiguration configuration)
            : base(configuration, false)
        {
            _stuntmanOptions = new StuntmanOptions();
        }

        /// <inheritdoc />
        protected override void OnConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddStuntman(_stuntmanOptions);
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
            app.UseStuntman(_stuntmanOptions);
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
    }
}
