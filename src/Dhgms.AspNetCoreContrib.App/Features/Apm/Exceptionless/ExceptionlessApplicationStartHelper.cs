// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Exceptionless;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Dhgms.AspNetCoreContrib.App.Features.Apm.Exceptionless
{
    public static class ExceptionlessApplicationStartHelper
    {
        public static void Configure(
            IConfiguration configuration,
            IApplicationBuilder app,
            IWebHostEnvironment env,
            Version version)
        {
            var exceptionlessApiKey = configuration.GetValue<string>("Exceptionless:ApiKey", null);

            if (string.IsNullOrWhiteSpace(exceptionlessApiKey))
            {
                return;
            }

            var exceptionlessClient = ExceptionlessClient.Default;
            DoExceptionLessConfiguration(
                env,
                exceptionlessClient,
                version);
            app.UseExceptionless(exceptionlessClient);
        }

        private static void DoExceptionLessConfiguration(
            IWebHostEnvironment env,
            ExceptionlessClient exceptionlessClient,
            Version version)
        {
            var exceptionlessConfiguration = exceptionlessClient.Configuration;
            exceptionlessConfiguration.SetVersion(version);
            exceptionlessConfiguration.Enabled = !env.IsDevelopment();
        }
    }
}
