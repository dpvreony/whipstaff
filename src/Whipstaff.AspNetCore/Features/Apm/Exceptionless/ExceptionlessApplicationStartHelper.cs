// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.AspNetCore.Features.Apm.Exceptionless
{
    /// <summary>
    /// Initialization logic for Exceptionless.
    /// </summary>
    public static class ExceptionlessApplicationStartHelper
    {
        /*
        public static void Configure(
            IConfiguration configuration,
            IApplicationBuilder app,
            Version version)
        {
            var exceptionlessApiKey = configuration.GetValue<string>("Exceptionless:ApiKey", null);

            if (string.IsNullOrWhiteSpace(exceptionlessApiKey))
            {
                return;
            }

            var exceptionlessClient = ExceptionlessClient.Default;
            DoExceptionLessConfiguration(
                exceptionlessClient,
                version);
            app.UseExceptionless(exceptionlessClient);
        }

        private static void DoExceptionLessConfiguration(
            ExceptionlessClient exceptionlessClient,
            Version version)
        {
            var exceptionlessConfiguration = exceptionlessClient.Configuration;
            exceptionlessConfiguration.SetVersion(version);
        }
        */
    }
}
