// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Finbuckle.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Whipstaff.AspNetCore.MultiTenancy;
using Xunit;

namespace Whipstaff.UnitTests.Features.MultiTenancy
{
    /// <summary>Exercises the Finbuckle lookup database integration.</summary>
    public sealed class TenantResolutionTests
    {
        /// <summary>Resolves each supported lookup source to the canonical tenant.</summary>
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
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync(TestContext.Current.CancellationToken);
            var services = new ServiceCollection();
            _ = services.AddLogging();
            _ = services.AddLookupTenantResolution(options => options.UseSqlite(connection), source, "example.com");
            await using var provider = services.BuildServiceProvider();
            await using var scope = provider.CreateAsyncScope();
            var database = scope.ServiceProvider.GetRequiredService<TenantLookupDbContext>();
            _ = await database.Database.EnsureCreatedAsync(TestContext.Current.CancellationToken);
            _ = database.TenantInfo.Add(new TenantInfo { Id = "tenant-1", Identifier = "canonical", Name = "Tenant" });
            _ = database.Lookups.Add(new TenantLookup { Source = source, Value = TenantLookup.Normalize(source, storedValue), TenantId = "tenant-1" });
            _ = await database.SaveChangesAsync(TestContext.Current.CancellationToken);

            var context = new DefaultHttpContext { RequestServices = scope.ServiceProvider };
            if (source == TenantLookupSource.HostPrefix)
            {
                context.Request.Host = new HostString(requestValue, 443);
            }
            else
            {
                context.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(claimType, requestValue) }, "test"));
            }

            var resolver = scope.ServiceProvider.GetRequiredService<ITenantResolver<TenantInfo>>();
            var result = await resolver.ResolveAsync(context);
            Assert.Equal("tenant-1", result.TenantInfo?.Id);
            Assert.Equal("canonical", result.TenantInfo?.Identifier);
        }

        /// <summary>Rejects claims from unauthenticated identities.</summary>
        /// <param name="source">The lookup source.</param>
        /// <returns>The test task.</returns>
        [Theory]
        [InlineData(TenantLookupSource.Email)]
        [InlineData(TenantLookupSource.EntraTenantId)]
        public async Task UnauthenticatedClaimsDoNotResolveAsync(TenantLookupSource source)
        {
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync(TestContext.Current.CancellationToken);
            var services = new ServiceCollection();
            _ = services.AddLogging();
            _ = services.AddLookupTenantResolution(options => options.UseSqlite(connection), source);
            await using var provider = services.BuildServiceProvider();
            await using var scope = provider.CreateAsyncScope();
            var context = new DefaultHttpContext { RequestServices = scope.ServiceProvider };
            var claim = source == TenantLookupSource.Email
                ? new Claim("email", "user@example.com")
                : new Claim("tid", "e8d07018-b993-4415-a986-293492239457");
            await SeedAsync(scope.ServiceProvider, source, claim.Value);
            context.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { claim }));
            var resolver = scope.ServiceProvider.GetRequiredService<ITenantResolver<TenantInfo>>();
            var result = await resolver.ResolveAsync(context);
            Assert.Null(result.TenantInfo);
        }

        /// <summary>Rejects hosts outside the configured domain without trusting forwarded headers.</summary>
        /// <param name="host">The incoming host.</param>
        /// <returns>The test task.</returns>
        [Theory]
        [InlineData("customer.evil.example.com")]
        [InlineData("customer.example.com.attacker.test")]
        [InlineData("example.com")]
        public async Task UntrustedHostsAndForwardedHeadersDoNotResolveAsync(string host)
        {
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync(TestContext.Current.CancellationToken);
            var services = new ServiceCollection();
            _ = services.AddLogging();
            _ = services.AddLookupTenantResolution(options => options.UseSqlite(connection), TenantLookupSource.HostPrefix, "example.com");
            await using var provider = services.BuildServiceProvider();
            await using var scope = provider.CreateAsyncScope();
            await SeedAsync(scope.ServiceProvider, TenantLookupSource.HostPrefix, "customer");
            var context = new DefaultHttpContext { RequestServices = scope.ServiceProvider };
            context.Request.Host = new HostString(host);
            context.Request.Headers["X-Forwarded-Host"] = "customer.example.com";
            var resolver = scope.ServiceProvider.GetRequiredService<ITenantResolver<TenantInfo>>();
            var result = await resolver.ResolveAsync(context);
            Assert.Null(result.TenantInfo);
        }

        /// <summary>Accepts a forwarded host only after trusted proxy processing.</summary>
        /// <param name="remoteAddress">The direct peer address.</param>
        /// <param name="expectedId">The expected tenant ID, if any.</param>
        /// <returns>The test task.</returns>
        [Theory]
        [InlineData("192.0.2.10", "tenant-1")]
        [InlineData("192.0.2.11", null)]
        public async Task ForwardedHostRequiresTrustedProxyAsync(string remoteAddress, string? expectedId)
        {
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync(TestContext.Current.CancellationToken);
            var services = new ServiceCollection();
            _ = services.AddLogging();
            _ = services.AddLookupTenantResolution(options => options.UseSqlite(connection), TenantLookupSource.HostPrefix, "example.com");
            await using var provider = services.BuildServiceProvider();
            await using var scope = provider.CreateAsyncScope();
            await SeedAsync(scope.ServiceProvider, TenantLookupSource.HostPrefix, "customer");
            var context = new DefaultHttpContext { RequestServices = scope.ServiceProvider };
            context.Connection.RemoteIpAddress = IPAddress.Parse(remoteAddress);
            context.Request.Host = new HostString("internal.invalid");
            context.Request.Headers["X-Forwarded-Host"] = "customer.example.com";
            var options = new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedHost };
            options.KnownProxies.Add(IPAddress.Parse("192.0.2.10"));
            string? resolvedId = null;
            var middleware = new ForwardedHeadersMiddleware(
                async httpContext =>
                {
                    var resolver = httpContext.RequestServices.GetRequiredService<ITenantResolver<TenantInfo>>();
                    resolvedId = (await resolver.ResolveAsync(httpContext)).TenantInfo?.Id;
                },
                scope.ServiceProvider.GetRequiredService<ILoggerFactory>(),
                Options.Create(options));
            await middleware.Invoke(context);
            Assert.Equal(expectedId, resolvedId);
        }

        /// <summary>Rejects conflicting authenticated email claims instead of choosing one.</summary>
        /// <returns>The test task.</returns>
        [Fact]
        public async Task ConflictingClaimsDoNotResolveAsync()
        {
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync(TestContext.Current.CancellationToken);
            var services = new ServiceCollection();
            _ = services.AddLogging();
            _ = services.AddLookupTenantResolution(options => options.UseSqlite(connection), TenantLookupSource.Email);
            await using var provider = services.BuildServiceProvider();
            await using var scope = provider.CreateAsyncScope();
            await SeedAsync(scope.ServiceProvider, TenantLookupSource.Email, "user@example.com");
            var context = new DefaultHttpContext { RequestServices = scope.ServiceProvider };
            var claims = new[]
            {
                new Claim("email", "user@example.com"),
                new Claim(ClaimTypes.Email, "other@example.com"),
            };
            context.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "test"));
            var resolver = scope.ServiceProvider.GetRequiredService<ITenantResolver<TenantInfo>>();
            var result = await resolver.ResolveAsync(context);
            Assert.Null(result.TenantInfo);
        }

        /// <summary>Enforces a unique lookup key in the relational database.</summary>
        /// <returns>The test task.</returns>
        [Fact]
        public async Task DuplicateLookupKeyFailsAsync()
        {
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync(TestContext.Current.CancellationToken);
            var services = new ServiceCollection();
            _ = services.AddLogging();
            _ = services.AddLookupTenantResolution(options => options.UseSqlite(connection), TenantLookupSource.Email);
            await using var provider = services.BuildServiceProvider();
            await using var scope = provider.CreateAsyncScope();
            await SeedAsync(scope.ServiceProvider, TenantLookupSource.Email, "user@example.com");
            var database = scope.ServiceProvider.GetRequiredService<TenantLookupDbContext>();
            database.ChangeTracker.Clear();
            _ = database.Lookups.Add(new TenantLookup
            {
                Source = TenantLookupSource.Email,
                Value = TenantLookup.Normalize(TenantLookupSource.Email, "USER@example.com"),
                TenantId = "tenant-1",
            });
            _ = await Assert.ThrowsAsync<DbUpdateException>(() => database.SaveChangesAsync(TestContext.Current.CancellationToken));
        }

        private static async Task SeedAsync(IServiceProvider services, TenantLookupSource source, string value)
        {
            var database = services.GetRequiredService<TenantLookupDbContext>();
            _ = await database.Database.EnsureCreatedAsync(TestContext.Current.CancellationToken);
            _ = database.TenantInfo.Add(new TenantInfo { Id = "tenant-1", Identifier = "canonical" });
            _ = database.Lookups.Add(new TenantLookup { Source = source, Value = TenantLookup.Normalize(source, value), TenantId = "tenant-1" });
            _ = await database.SaveChangesAsync(TestContext.Current.CancellationToken);
        }
    }
}
