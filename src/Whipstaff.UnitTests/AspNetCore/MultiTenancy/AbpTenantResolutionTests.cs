// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.AspNetCore.Security.Claims;
using Volo.Abp.MultiTenancy;
using Volo.Abp.MultiTenancy.ConfigurationStore;
using Volo.Abp.MultiTenancy.Localization;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;
using Whipstaff.Abp.AspNetCore.MultiTenancy;
using Whipstaff.Testing.EntityFramework;
using Xunit;

namespace Whipstaff.UnitTests.Features.MultiTenancy
{
    /// <summary>Exercises the ABP tenant resolver and host lookup database.</summary>
    public sealed class AbpTenantResolutionTests
    {
        private static readonly Guid TenantId = Guid.Parse("e43f729c-dc7b-4e84-9fd8-682b3d01d93e");
        private static readonly Guid UserId = Guid.Parse("a63d9860-dc0d-46b4-bba3-8671446a5bd7");

        /// <summary>Resolves each supported source to an ABP tenant ID.</summary>
        /// <param name="source">The lookup source.</param>
        /// <param name="claimType">The identity claim type.</param>
        /// <param name="storedValue">The stored lookup value.</param>
        /// <param name="requestValue">The incoming lookup value.</param>
        /// <returns>The test task.</returns>
        [Theory]
        [InlineData(TenantLookupSource.EntraTenantId, "tid", "e8d07018-b993-4415-a986-293492239457", "E8D07018-B993-4415-A986-293492239457")]
        [InlineData(TenantLookupSource.EntraTenantId, "http://schemas.microsoft.com/identity/claims/tenantid", "e8d07018-b993-4415-a986-293492239457", "e8d07018-b993-4415-a986-293492239457")]
        [InlineData(TenantLookupSource.Email, "email", "user@example.com", "User@Example.Com")]
        [InlineData(TenantLookupSource.Email, ClaimTypes.Email, "user@example.com", "user@example.com")]
        [InlineData(TenantLookupSource.HostPrefix, "", "customer", "CUSTOMER.example.com")]
        public async Task ResolvesConfiguredLookupAsync(TenantLookupSource source, string claimType, string storedValue, string requestValue)
        {
            await using var environment = new TestEnvironment(source);
            await environment.SeedAsync(source, storedValue);
            if (source == TenantLookupSource.HostPrefix)
            {
                environment.HttpContext.Request.Host = new HostString(requestValue, 443);
            }
            else
            {
                environment.HttpContext.User = Principal(new Claim(claimType, requestValue));
            }

            var result = await environment.Resolver.ResolveTenantIdOrNameAsync();
            Assert.Equal(TenantId.ToString("D"), result.TenantIdOrName);
            Assert.Equal("WhipstaffLookup", Assert.Single(result.AppliedResolvers));
        }

        /// <summary>ABP validates that the resolved tenant exists and is active in its tenant store.</summary>
        /// <param name="exists">Whether the ABP tenant exists.</param>
        /// <param name="active">Whether the ABP tenant is active.</param>
        /// <param name="expectedError">The expected ABP error code, if any.</param>
        /// <returns>The test task.</returns>
        [Theory]
        [InlineData(true, true, null)]
        [InlineData(false, false, "Volo.AbpIo.MultiTenancy:010001")]
        [InlineData(true, false, "Volo.AbpIo.MultiTenancy:010002")]
        public async Task AbpStoreValidatesResolvedTenantAsync(bool exists, bool active, string? expectedError)
        {
            await using var environment = new TestEnvironment(TenantLookupSource.Email);
            await environment.SeedAsync(TenantLookupSource.Email, "user@example.com");
            environment.HttpContext.User = Principal(new Claim("email", "user@example.com"));
            var services = new ServiceCollection();
            _ = services.AddLogging();
            _ = services.AddLocalization();
            _ = services.Configure<AbpDefaultTenantStoreOptions>(options =>
            {
                options.Tenants = exists
                    ? new[] { new TenantConfiguration(TenantId, "tenant") { IsActive = active } }
                    : Array.Empty<TenantConfiguration>();
            });
            await using var provider = services.BuildServiceProvider();
            var configurationProvider = new TenantConfigurationProvider(
                environment.Resolver,
                new DefaultTenantStore(provider.GetRequiredService<IOptionsMonitor<AbpDefaultTenantStoreOptions>>()),
                new NullTenantResolveResultAccessor(),
                provider.GetRequiredService<IStringLocalizer<AbpMultiTenancyResource>>(),
                new UpperInvariantTenantNormalizer());
            if (expectedError == null)
            {
                Assert.Equal(TenantId, (await configurationProvider.GetAsync())?.Id);
            }
            else
            {
                var exception = await Assert.ThrowsAsync<BusinessException>(() => configurationProvider.GetAsync());
                Assert.Equal(expectedError, exception.Code);
            }
        }

        /// <summary>Ignores untrusted and conflicting identity claims.</summary>
        /// <param name="authenticated">Whether the identity is authenticated.</param>
        /// <param name="conflicting">Whether the identity contains conflicting claims.</param>
        /// <returns>The test task.</returns>
        [Theory]
        [InlineData(false, false)]
        [InlineData(true, true)]
        public async Task RejectsUntrustedOrConflictingClaimsAsync(bool authenticated, bool conflicting)
        {
            await using var environment = new TestEnvironment(TenantLookupSource.Email);
            await environment.SeedAsync(TenantLookupSource.Email, "user@example.com");
            var claims = conflicting
                ? new[] { new Claim("email", "user@example.com"), new Claim(ClaimTypes.Email, "other@example.com") }
                : new[] { new Claim("email", "user@example.com") };
            environment.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims, authenticated ? "test" : null));
            var result = await environment.Resolver.ResolveTenantIdOrNameAsync();
            Assert.Null(result.TenantIdOrName);
        }

        /// <summary>Existing ABP tenant claims have priority over external lookup keys.</summary>
        /// <returns>The test task.</returns>
        [Fact]
        public async Task ExistingAbpTenantTakesPriorityAsync()
        {
            await using var environment = new TestEnvironment(TenantLookupSource.Email);
            await environment.SeedAsync(TenantLookupSource.Email, "user@example.com");
            var existing = Guid.Parse("9232d548-f503-4635-a6bf-6c185db7e46f");
            environment.HttpContext.User = Principal(
                new Claim(AbpClaimTypes.UserId, UserId.ToString("D")),
                new Claim(AbpClaimTypes.TenantId, existing.ToString("D")),
                new Claim("email", "user@example.com"));
            var result = await environment.Resolver.ResolveTenantIdOrNameAsync();
            Assert.Equal(existing.ToString("D"), result.TenantIdOrName);
        }

        /// <summary>An authenticated ABP user without a tenant can be mapped by the selected identity key.</summary>
        /// <returns>The test task.</returns>
        [Fact]
        public async Task AuthenticatedHostUserCanUseIdentityLookupAsync()
        {
            await using var environment = new TestEnvironment(TenantLookupSource.Email);
            await environment.SeedAsync(TenantLookupSource.Email, "user@example.com");
            environment.HttpContext.User = Principal(
                new Claim(AbpClaimTypes.UserId, UserId.ToString("D")),
                new Claim("email", "user@example.com"));
            var result = await environment.Resolver.ResolveTenantIdOrNameAsync();
            Assert.Equal(TenantId.ToString("D"), result.TenantIdOrName);
        }

        /// <summary>Missing lookup tables propagate database failures rather than resolving the host.</summary>
        /// <returns>The test task.</returns>
        [Fact]
        public async Task DatabaseErrorsPropagateAsync()
        {
            await using var environment = new TestEnvironment(TenantLookupSource.Email);
            environment.HttpContext.User = Principal(new Claim("email", "user@example.com"));
            _ = await Assert.ThrowsAsync<SqliteException>(() => environment.Resolver.ResolveTenantIdOrNameAsync());
        }

        /// <summary>An authenticated ABP host user cannot be switched to a tenant by a host header.</summary>
        /// <returns>The test task.</returns>
        [Fact]
        public async Task AuthenticatedHostUserRemainsHostAsync()
        {
            await using var environment = new TestEnvironment(TenantLookupSource.HostPrefix);
            await environment.SeedAsync(TenantLookupSource.HostPrefix, "customer");
            environment.HttpContext.User = Principal(new Claim(AbpClaimTypes.UserId, UserId.ToString("D")));
            environment.HttpContext.Request.Host = new HostString("customer.example.com");
            var result = await environment.Resolver.ResolveTenantIdOrNameAsync();
            Assert.Null(result.TenantIdOrName);
        }

        /// <summary>Only a trusted proxy's forwarded host is used by the resolver.</summary>
        /// <param name="remoteAddress">The direct peer address.</param>
        /// <param name="isTrusted">Whether the peer is trusted.</param>
        /// <returns>The test task.</returns>
        [Theory]
        [InlineData("192.0.2.10", true)]
        [InlineData("192.0.2.11", false)]
        public async Task ForwardedHostRequiresTrustedProxyAsync(string remoteAddress, bool isTrusted)
        {
            await using var environment = new TestEnvironment(TenantLookupSource.HostPrefix);
            await environment.SeedAsync(TenantLookupSource.HostPrefix, "customer");
            var context = environment.HttpContext;
            context.Connection.RemoteIpAddress = IPAddress.Parse(remoteAddress);
            context.Request.Host = new HostString("internal.invalid");
            context.Request.Headers["X-Forwarded-Host"] = "customer.example.com";
            var options = new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedHost };
            options.KnownProxies.Add(IPAddress.Parse("192.0.2.10"));
            string? resolved = null;
            var middleware = new ForwardedHeadersMiddleware(
                async _ => resolved = (await environment.Resolver.ResolveTenantIdOrNameAsync()).TenantIdOrName,
                NullLoggerFactory.Instance,
                Options.Create(options));
            await middleware.Invoke(context);
            Assert.Equal(isTrusted ? TenantId.ToString("D") : null, resolved);
        }

        /// <summary>Rejects hosts outside the configured domain.</summary>
        /// <param name="host">The incoming host.</param>
        /// <returns>The test task.</returns>
        [Theory]
        [InlineData("customer.evil.example.com")]
        [InlineData("customer.example.com.attacker.test")]
        [InlineData("example.com")]
        public async Task RejectsUntrustedHostsAsync(string host)
        {
            await using var environment = new TestEnvironment(TenantLookupSource.HostPrefix);
            await environment.SeedAsync(TenantLookupSource.HostPrefix, "customer");
            environment.HttpContext.Request.Host = new HostString(host);
            environment.HttpContext.Request.Headers["X-Forwarded-Host"] = "customer.example.com";
            var result = await environment.Resolver.ResolveTenantIdOrNameAsync();
            Assert.Null(result.TenantIdOrName);
        }

        /// <summary>Relational storage rejects duplicate normalized lookup keys.</summary>
        /// <returns>The test task.</returns>
        [Fact]
        public async Task DuplicateLookupKeyFailsAsync()
        {
            await using var environment = new TestEnvironment(TenantLookupSource.Email);
            await environment.SeedAsync(TenantLookupSource.Email, "user@example.com");
            await using var database = environment.Factory.CreateDbContext();
            _ = database.Lookups.Add(new TenantLookup { Source = TenantLookupSource.Email, Value = TenantLookup.Normalize(TenantLookupSource.Email, "USER@example.com"), TenantId = Guid.NewGuid() });
            _ = await Assert.ThrowsAsync<DbUpdateException>(() => database.SaveChangesAsync(TestContext.Current.CancellationToken));
        }

        private static ClaimsPrincipal Principal(params Claim[] claims)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(claims, "test"));
        }

        private sealed class LookupFactory : AbstractDbContextFactory<TenantLookupDbContext>
        {
            public LookupFactory(SqliteConnection connection)
                : base(connection, NullLoggerFactory.Instance)
            {
            }

            protected override TenantLookupDbContext GetDbContext(DbContextOptions<TenantLookupDbContext> dbContextOptions)
            {
                return new TenantLookupDbContext(dbContextOptions);
            }
        }

        private sealed class TestEnvironment : IAsyncDisposable
        {
            private readonly SqliteConnection _connection;
            private readonly ServiceProvider _provider;
            private readonly IHttpContextAccessor _accessor;

            public TestEnvironment(TenantLookupSource source)
            {
                _connection = new SqliteConnection("Data Source=:memory:");
                _connection.Open();
                Factory = new LookupFactory(_connection);
                var services = new ServiceCollection();
                _ = services.AddLogging();
                _ = services.AddAbpLookupTenantResolution(options => options.UseSqlite(_connection), source, "example.com");
                _ = services.Configure<AbpTenantResolveOptions>(options =>
                {
                    options.TenantResolvers.Add(new CurrentUserTenantResolveContributor());
                    options.FallbackTenant = "unsafe-fallback";
                });
                _ = services.AddTransient<ICurrentUser, CurrentUser>();
                _ = services.AddTransient<ICurrentPrincipalAccessor, HttpContextCurrentPrincipalAccessor>();
                _provider = services.BuildServiceProvider();
                _accessor = _provider.GetRequiredService<IHttpContextAccessor>();
                HttpContext = new DefaultHttpContext();
                _accessor.HttpContext = HttpContext;
                Resolver = new TenantResolver(_provider.GetRequiredService<IOptions<AbpTenantResolveOptions>>(), _provider);
            }

            public LookupFactory Factory { get; }

            public DefaultHttpContext HttpContext { get; }

            public TenantResolver Resolver { get; }

            public async Task SeedAsync(TenantLookupSource source, string value)
            {
                await using var database = Factory.CreateDbContext();
                _ = database.Lookups.Add(new TenantLookup { Source = source, Value = TenantLookup.Normalize(source, value), TenantId = TenantId });
                _ = await database.SaveChangesAsync(TestContext.Current.CancellationToken);
            }

            public async ValueTask DisposeAsync()
            {
                _accessor.HttpContext = null;
                await _provider.DisposeAsync();
                await _connection.DisposeAsync();
            }
        }
    }
}
