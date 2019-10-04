// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Fakes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dhgms.AspNetCoreContrib.Examples.WebMvcApp
{
    /// <summary>
    /// Start up logic for the sample Web MVC app.
    /// </summary>
    public class Startup : Dhgms.AspNetCoreContrib.App.BaseStartup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        public Startup(IConfiguration configuration)
            : base(configuration)
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
        protected override Assembly[] GetMediatrAssemblies()
        {
            return new[]
            {
                typeof(FakeCrudController).Assembly,
                typeof(Startup).Assembly,
            };
        }
    }
}
