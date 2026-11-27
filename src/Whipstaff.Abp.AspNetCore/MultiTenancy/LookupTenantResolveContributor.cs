// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;
using Whipstaff.MultiTenancy.Internal;

namespace Whipstaff.Abp.AspNetCore.MultiTenancy
{
    /// <summary>Resolves an ABP tenant ID from an external key in the host lookup database.</summary>
    public sealed class LookupTenantResolveContributor : TenantResolveContributorBase
    {
        private readonly TenantLookupSource _source;
        private readonly string? _hostDomain;

        /// <summary>Initializes a new instance of the <see cref="LookupTenantResolveContributor"/> class.</summary>
        /// <param name="source">The resolution source.</param>
        /// <param name="hostDomain">The allowed host parent domain.</param>
        public LookupTenantResolveContributor(TenantLookupSource source, string? hostDomain = null)
        {
            if (!Enum.IsDefined(source) || source == TenantLookupSource.None)
            {
                throw new ArgumentOutOfRangeException(nameof(source));
            }

            if (source == TenantLookupSource.HostPrefix &&
                (string.IsNullOrWhiteSpace(hostDomain) || Uri.CheckHostName(hostDomain) != UriHostNameType.Dns))
            {
                throw new ArgumentException("A valid parent DNS domain is required for host-prefix resolution.", nameof(hostDomain));
            }

            _source = source;
            _hostDomain = hostDomain;
        }

        /// <inheritdoc />
        public override string Name => "WhipstaffLookup";

        /// <inheritdoc />
        public override async Task ResolveAsync(ITenantResolveContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();
            var httpContext = context.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var authenticated = currentUser.IsAuthenticated &&
                (httpContext == null || httpContext.User.Identities.Any(identity => identity.IsAuthenticated));
            if (authenticated && currentUser.TenantId.HasValue)
            {
                context.TenantIdOrName = currentUser.TenantId.Value.ToString("D");
                context.Handled = true;
                return;
            }

            if (authenticated && _source == TenantLookupSource.HostPrefix)
            {
                context.Handled = true;
                return;
            }

            if (httpContext != null)
            {
                var value = TenantKeyReader.Read(httpContext, _source == TenantLookupSource.HostPrefix, _source == TenantLookupSource.EntraTenantId, _hostDomain);
                if (value != null)
                {
                    var normalized = TenantLookup.Normalize(_source, value);
                    var database = context.ServiceProvider.GetRequiredService<TenantLookupDbContext>();
                    var tenantId = await database.Lookups.AsNoTracking()
                        .Where(lookup => lookup.Source == _source && lookup.Value == normalized)
                        .Select(lookup => (Guid?)lookup.TenantId)
                        .SingleOrDefaultAsync(httpContext.RequestAborted)
                        .ConfigureAwait(false);
                    context.TenantIdOrName = tenantId?.ToString("D");
                }
            }

            context.Handled = true;
        }
    }
}
