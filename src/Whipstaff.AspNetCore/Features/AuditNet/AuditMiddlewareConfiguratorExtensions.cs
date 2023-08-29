// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Audit.WebApi.ConfigurationApi;
using Microsoft.AspNetCore.Http;

namespace Whipstaff.AspNetCore.Features.AuditNet
{
    /// <summary>
    /// Extension methods for configuring Audit.Net.
    /// </summary>
    public static class AuditMiddlewareConfiguratorExtensions
    {
        /// <summary>
        /// Configures Audit.Net to do a full audit with a default HTTP request filter.
        /// </summary>
        /// <param name="auditMiddlewareConfigurator">Audit Middleware Configurator to modify.</param>
        /// <returns>Modified Audit Middleware Configurator.</returns>
        public static IAuditMiddlewareConfigurator DoFullAuditMiddlewareConfig(this IAuditMiddlewareConfigurator auditMiddlewareConfigurator)
        {
            return DoFullAuditMiddlewareConfig(
                auditMiddlewareConfigurator,
                rq => IgnoreFavIcon(rq));
        }

        /// <summary>
        /// Configures Audit.Net to do a full audit with a custom HTTP request filter.
        /// </summary>
        /// <param name="auditMiddlewareConfigurator">Audit Middleware Configurator to modify.</param>
        /// <param name="shouldLogRequestFunc">Function to execute to decide whether a request should be audited.</param>
        /// <returns>Modified Audit Middleware Configurator.</returns>
        public static IAuditMiddlewareConfigurator DoFullAuditMiddlewareConfig(
            this IAuditMiddlewareConfigurator auditMiddlewareConfigurator,
            Func<HttpRequest, bool> shouldLogRequestFunc)
        {
            ArgumentNullException.ThrowIfNull(auditMiddlewareConfigurator);

            return auditMiddlewareConfigurator.FilterByRequest(rq => shouldLogRequestFunc(rq))
                .WithEventType("{verb}:{url}")
                .IncludeHeaders()
                .IncludeRequestBody()
                .IncludeResponseHeaders()
                .IncludeResponseBody();
        }

        private static bool IgnoreFavIcon(HttpRequest rq)
        {
            var pathValue = rq.Path.Value;
            return pathValue != null && !pathValue.EndsWith("favicon.ico", StringComparison.OrdinalIgnoreCase);
        }
    }
}
