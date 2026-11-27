// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Whipstaff.MultiTenancy.Internal;

namespace Whipstaff.MultiTenancy.AspNetCore
{
    /// <summary>Resolves an active tenant for the request or rejects the request.</summary>
    public sealed class TenantResolutionMiddleware
    {
        private static readonly Action<ILogger, TenantLookupSource, Exception?> MissingKey = LoggerMessage.Define<TenantLookupSource>(
            LogLevel.Information, new EventId(1, nameof(MissingKey)), "Tenant resolution failed: no valid {Source} key was supplied.");

        private static readonly Action<ILogger, TenantLookupSource, Exception?> MissingTenant = LoggerMessage.Define<TenantLookupSource>(
            LogLevel.Information, new EventId(2, nameof(MissingTenant)), "Tenant resolution failed: no active tenant matched the {Source} key.");

        private readonly RequestDelegate _next;
        private readonly TenantResolutionOptions _options;
        private readonly ILogger<TenantResolutionMiddleware> _logger;

        /// <summary>Initializes a new instance of the <see cref="TenantResolutionMiddleware"/> class.</summary>
        /// <param name="next">The next middleware.</param>
        /// <param name="options">The configured lookup source.</param>
        /// <param name="logger">The logger.</param>
        public TenantResolutionMiddleware(RequestDelegate next, IOptions<TenantResolutionOptions> options, ILogger<TenantResolutionMiddleware> logger)
        {
            ArgumentNullException.ThrowIfNull(next);
            ArgumentNullException.ThrowIfNull(options);
            ArgumentNullException.ThrowIfNull(logger);
            TenantResolutionOptions.Validate(options.Value.Source, options.Value.HostDomain);
            _next = next;
            _options = new TenantResolutionOptions { Source = options.Value.Source, HostDomain = options.Value.HostDomain };
            _logger = logger;
        }

        /// <summary>Resolves the tenant in the request scope.</summary>
        /// <param name="context">The current HTTP context.</param>
        /// <param name="database">The host lookup database.</param>
        /// <param name="currentTenant">The scoped tenant accessor.</param>
        /// <returns>The middleware task.</returns>
        public async Task InvokeAsync(HttpContext context, TenantLookupDbContext database, CurrentTenant currentTenant)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(database);
            ArgumentNullException.ThrowIfNull(currentTenant);
            var value = TenantKeyReader.Read(context, _options.Source == TenantLookupSource.HostPrefix, _options.Source == TenantLookupSource.EntraTenantId, _options.HostDomain);
            if (value == null)
            {
                MissingKey(_logger, _options.Source, null);
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            var normalized = TenantLookup.Normalize(_options.Source, value);
            var tenant = await database.Lookups.AsNoTracking()
                .Where(x => x.Source == _options.Source && x.Value == normalized && x.Tenant.IsActive)
                .Select(x => new { x.Tenant.Id, x.Tenant.Name })
                .SingleOrDefaultAsync(context.RequestAborted)
                .ConfigureAwait(false);
            if (tenant == null)
            {
                MissingTenant(_logger, _options.Source, null);
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            var previousId = currentTenant.Id;
            var previousName = currentTenant.Name;
            currentTenant.Id = tenant.Id;
            currentTenant.Name = tenant.Name;
            try
            {
                await _next(context).ConfigureAwait(false);
            }
            finally
            {
                currentTenant.Id = previousId;
                currentTenant.Name = previousName;
            }
        }
    }
}
