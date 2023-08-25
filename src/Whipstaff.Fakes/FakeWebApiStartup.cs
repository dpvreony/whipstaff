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
using Whipstaff.AspNetCore.Features.ApplicationStartup;
using Whipstaff.Core.Mediatr;
using Whipstaff.Testing.Cqrs;
using Whipstaff.Testing.MediatR;

namespace Whipstaff.Testing
{
    /// <summary>
    /// Startup object for the WebAPI example Application.
    /// </summary>
    public sealed class FakeWebApiStartup : BaseStartup
    {
        /// <inheritdoc />
        public override void ConfigureLogging(WebHostBuilderContext hostBuilderContext, ILoggingBuilder loggingBuilder)
        {
        }

        /// <inheritdoc />
        protected override void OnConfigureServices(IServiceCollection serviceCollection)
        {
            _ = serviceCollection.AddSingleton<FakeAuditableCommandFactory>();
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
    }
}
