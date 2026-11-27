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
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Whipstaff.MultiTenancy.AspNetCore;
using Whipstaff.Testing.EntityFramework;
using Xunit;

namespace Whipstaff.UnitTests.Features.MultiTenancy
{
    /// <summary>Exercises standalone tenant resolution in an ASP.NET Core pipeline.</summary>
    public sealed class StandaloneTenantResolutionTests
    {
        private static readonly Guid TenantId = Guid.Parse("e43f729c-dc7b-4e84-9fd8-682b3d01d93e");

        /// <summary>Resolves all supported lookup sources and exposes the current tenant downstream.</summary>
        /// <param name="source">The lookup source.</param>
        /// <param name="claimType">The identity claim type.</param>
        /// <param name="storedValue">The stored lookup key.</param>
        /// <param name="requestValue">The incoming lookup value.</param>
        /// <returns>The test task.</returns>
        [Theory]
        [InlineData(TenantLookupSource.EntraTenantId, "tid", "e8d07018-b993-4415-a986-293492239457", "E8D07018-B993-4415-A986-293492239457")]
        [InlineData(TenantLookupSource.EntraTenantId, "http://schemas.microsoft.com/identity/claims/tenantid", "e8d07018-b993-4415-a986-293492239457", "e8d07018-b993-4415-a986-293492239457")]
        [InlineData(TenantLookupSource.Email, "email", "user@example.com", "User@Example.Com")]
        [InlineData(TenantLookupSource.Email, ClaimTypes.Email, "user@example.com", "user@example.com")]
        [InlineData(TenantLookupSource.HostPrefix, "", "customer", "CUSTOMER.example.com")]
        public async Task ResolvesTenantAsync(TenantLookupSource source, string claimType, string storedValue, string requestValue)
        {
            await using var environment = new TestEnvironment(source);
            await environment.SeedAsync(source, storedValue);
            var context = environment.CreateContext();
            if (source == TenantLookupSource.HostPrefix)
            {
                context.Request.Host = new HostString(requestValue, 443);
            }
            else
            {
                context.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(claimType, requestValue) }, "test"));
            }

            Guid? seenId = null;
            string? seenName = null;
            await environment.RunAsync(context, httpContext =>
            {
                var tenant = httpContext.RequestServices.GetRequiredService<CurrentTenant>();
                seenId = tenant.Id;
                seenName = tenant.Name;
                return Task.CompletedTask;
            });
            Assert.Equal(TenantId, seenId);
            Assert.Equal("Customer", seenName);
            Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
            Assert.Null(environment.CurrentTenant.Id);
            Assert.Null(environment.CurrentTenant.Name);
        }

        /// <summary>Untrusted or conflicting claims cannot select a tenant.</summary>
        /// <param name="authenticated">Whether the identity is authenticated.</param>
        /// <param name="conflicting">Whether the identity contains conflicting claims.</param>
        /// <returns>The test task.</returns>
        [Theory]
        [InlineData(false, false)]
        [InlineData(true, true)]
        public async Task RejectsClaimsAsync(bool authenticated, bool conflicting)
        {
            await using var environment = new TestEnvironment(TenantLookupSource.Email);
            await environment.SeedAsync(TenantLookupSource.Email, "user@example.com");
            var context = environment.CreateContext();
            var claims = conflicting
                ? new[] { new Claim("email", "user@example.com"), new Claim(ClaimTypes.Email, "other@example.com") }
                : new[] { new Claim("email", "user@example.com") };
            context.User = new ClaimsPrincipal(new ClaimsIdentity(claims, authenticated ? "test" : null));
            var invoked = false;
            await environment.RunAsync(context, _ =>
            {
                invoked = true;
                return Task.CompletedTask;
            });
            Assert.False(invoked);
            Assert.Equal(StatusCodes.Status404NotFound, context.Response.StatusCode);
            Assert.Null(environment.CurrentTenant.Id);
        }

        /// <summary>Unknown and inactive tenants do not reach downstream middleware.</summary>
        /// <param name="isActive">Whether the existing tenant is active.</param>
        /// <param name="host">The requested host.</param>
        /// <returns>The test task.</returns>
        [Theory]
        [InlineData(false, "customer.example.com")]
        [InlineData(true, "unknown.example.com")]
        [InlineData(true, "customer.example.com.attacker.test")]
        [InlineData(true, "customer.evil.example.com")]
        [InlineData(true, "example.com")]
        public async Task RejectsUnknownOrInactiveTenantAsync(bool isActive, string host)
        {
            await using var environment = new TestEnvironment(TenantLookupSource.HostPrefix);
            await environment.SeedAsync(TenantLookupSource.HostPrefix, "customer", isActive);
            var context = environment.CreateContext();
            context.Request.Host = new HostString(host);
            context.Request.Headers["X-Forwarded-Host"] = "customer.example.com";
            var invoked = false;
            await environment.RunAsync(context, _ =>
            {
                invoked = true;
                return Task.CompletedTask;
            });
            Assert.False(invoked);
            Assert.Equal(StatusCodes.Status404NotFound, context.Response.StatusCode);
            Assert.Null(environment.CurrentTenant.Id);
        }

        /// <summary>Only trusted forwarded hosts are applied before tenant resolution.</summary>
        /// <param name="remoteAddress">The direct peer address.</param>
        /// <param name="isTrusted">Whether the direct peer is trusted.</param>
        /// <returns>The test task.</returns>
        [Theory]
        [InlineData("192.0.2.10", true)]
        [InlineData("192.0.2.11", false)]
        public async Task HonorsTrustedProxiesAsync(string remoteAddress, bool isTrusted)
        {
            await using var environment = new TestEnvironment(TenantLookupSource.HostPrefix);
            await environment.SeedAsync(TenantLookupSource.HostPrefix, "customer");
            var context = environment.CreateContext();
            context.Connection.RemoteIpAddress = IPAddress.Parse(remoteAddress);
            context.Request.Host = new HostString("internal.invalid");
            context.Request.Headers["X-Forwarded-Host"] = "customer.example.com";
            var options = new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedHost };
            options.KnownProxies.Add(IPAddress.Parse("192.0.2.10"));
            Guid? resolved = null;
            var pipeline = environment.BuildPipeline(httpContext =>
            {
                resolved = httpContext.RequestServices.GetRequiredService<CurrentTenant>().Id;
                return Task.CompletedTask;
            });
            var forwarded = new ForwardedHeadersMiddleware(pipeline, NullLoggerFactory.Instance, Options.Create(options));
            await forwarded.Invoke(context);
            Assert.Equal(isTrusted ? TenantId : null, resolved);
            Assert.Equal(isTrusted ? StatusCodes.Status200OK : StatusCodes.Status404NotFound, context.Response.StatusCode);
        }

        /// <summary>Tenant state is cleared even when downstream middleware fails.</summary>
        /// <returns>The test task.</returns>
        [Fact]
        public async Task ClearsTenantWhenDownstreamThrowsAsync()
        {
            await using var environment = new TestEnvironment(TenantLookupSource.HostPrefix);
            await environment.SeedAsync(TenantLookupSource.HostPrefix, "customer");
            var context = environment.CreateContext();
            context.Request.Host = new HostString("customer.example.com");
            _ = await Assert.ThrowsAsync<InvalidOperationException>(() => environment.RunAsync(context, _ => throw new InvalidOperationException()));
            Assert.Null(environment.CurrentTenant.Id);
            Assert.Null(environment.CurrentTenant.Name);
        }

        /// <summary>Concurrent request scopes keep tenant state independent.</summary>
        /// <returns>The test task.</returns>
        [Fact]
        public async Task RequestScopesAreIsolatedAsync()
        {
            await using var environment = new TestEnvironment(TenantLookupSource.HostPrefix);
            await environment.SeedAsync(TenantLookupSource.HostPrefix, "customer");
            await using var secondScope = environment.CreateScope();
            var first = environment.CreateContext();
            first.Request.Host = new HostString("customer.example.com");
            var second = new DefaultHttpContext { RequestServices = secondScope.ServiceProvider };
            second.Request.Host = new HostString("missing.example.com");
            var entered = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            var firstRequest = environment.RunAsync(first, async _ =>
            {
                entered.SetResult();
#pragma warning disable VSTHRD003 // The test coordinates two independent request scopes.
                await release.Task.WaitAsync(TestContext.Current.CancellationToken);
#pragma warning restore VSTHRD003
                Assert.Equal(TenantId, environment.CurrentTenant.Id);
            });
            await entered.Task.WaitAsync(TestContext.Current.CancellationToken);
            try
            {
                await environment.RunAsync(second, _ => Task.CompletedTask);
                Assert.Null(secondScope.ServiceProvider.GetRequiredService<CurrentTenant>().Id);
                Assert.Equal(StatusCodes.Status404NotFound, second.Response.StatusCode);
                Assert.Equal(TenantId, environment.CurrentTenant.Id);
            }
            finally
            {
                release.SetResult();
                await firstRequest;
            }

            Assert.Null(environment.CurrentTenant.Id);
        }

        /// <summary>Database errors propagate instead of being treated as an unresolved tenant.</summary>
        /// <returns>The test task.</returns>
        [Fact]
        public async Task DatabaseErrorsPropagateAsync()
        {
            await using var environment = new TestEnvironment(TenantLookupSource.Email);
            var context = environment.CreateContext();
            context.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("email", "user@example.com") }, "test"));
            _ = await Assert.ThrowsAsync<SqliteException>(() => environment.RunAsync(context, _ => Task.CompletedTask));
        }

        /// <summary>Invalid Entra tenant identifiers are rejected before database access.</summary>
        /// <returns>The test task.</returns>
        [Fact]
        public async Task InvalidEntraIdDoesNotResolveAsync()
        {
            await using var environment = new TestEnvironment(TenantLookupSource.EntraTenantId);
            var context = environment.CreateContext();
            context.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("tid", "not-a-guid") }, "test"));
            await environment.RunAsync(context, _ => Task.CompletedTask);
            Assert.Equal(StatusCodes.Status404NotFound, context.Response.StatusCode);
            Assert.Null(environment.CurrentTenant.Id);
        }

        /// <summary>The lookup key is unique after normalization.</summary>
        /// <returns>The test task.</returns>
        [Fact]
        public async Task DuplicateLookupKeyFailsAsync()
        {
            await using var environment = new TestEnvironment(TenantLookupSource.Email);
            await environment.SeedAsync(TenantLookupSource.Email, "user@example.com");
            await using var database = environment.Factory.CreateDbContext();
            _ = database.Lookups.Add(new TenantLookup { Source = TenantLookupSource.Email, Value = TenantLookup.Normalize(TenantLookupSource.Email, "USER@example.com"), TenantId = TenantId });
            _ = await Assert.ThrowsAsync<DbUpdateException>(() => database.SaveChangesAsync(TestContext.Current.CancellationToken));
        }

        /// <summary>A lookup cannot refer to a nonexistent tenant.</summary>
        /// <returns>The test task.</returns>
        [Fact]
        public async Task MissingTenantForeignKeyFailsAsync()
        {
            await using var environment = new TestEnvironment(TenantLookupSource.Email);
            await using var database = environment.Factory.CreateDbContext();
            _ = database.Lookups.Add(new TenantLookup { Source = TenantLookupSource.Email, Value = TenantLookup.Normalize(TenantLookupSource.Email, "user@example.com"), TenantId = TenantId });
            _ = await Assert.ThrowsAsync<DbUpdateException>(() => database.SaveChangesAsync(TestContext.Current.CancellationToken));
        }

        /// <summary>Rejects invalid source and host configuration.</summary>
        /// <param name="source">The lookup source.</param>
        /// <param name="hostDomain">The configured parent domain.</param>
        [Theory]
        [InlineData(TenantLookupSource.None, null)]
        [InlineData(TenantLookupSource.HostPrefix, null)]
        [InlineData(TenantLookupSource.HostPrefix, "bad host")]
        public void RejectsInvalidConfiguration(TenantLookupSource source, string? hostDomain)
        {
            var services = new ServiceCollection();
            _ = Assert.ThrowsAny<ArgumentException>(() => services.AddStandaloneTenantResolution(_ => { }, source, hostDomain));
        }

        /// <summary>Middleware rejects invalid options even when they are overridden after registration.</summary>
        [Fact]
        public void MiddlewareRejectsInvalidOptions()
        {
            var options = Options.Create(new TenantResolutionOptions { Source = TenantLookupSource.HostPrefix });
            _ = Assert.Throws<ArgumentException>(() => new TenantResolutionMiddleware(_ => Task.CompletedTask, options, NullLogger<TenantResolutionMiddleware>.Instance));
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
            private readonly AsyncServiceScope _scope;

            public TestEnvironment(TenantLookupSource source)
            {
                _connection = new SqliteConnection("Data Source=:memory:");
                _connection.Open();
                Factory = new LookupFactory(_connection);
                var services = new ServiceCollection();
                _ = services.AddLogging();
                _ = services.AddStandaloneTenantResolution(options => options.UseSqlite(_connection), source, "example.com");
                _provider = services.BuildServiceProvider();
                _scope = _provider.CreateAsyncScope();
                CurrentTenant = _scope.ServiceProvider.GetRequiredService<CurrentTenant>();
            }

            public LookupFactory Factory { get; }

            public CurrentTenant CurrentTenant { get; }

            public DefaultHttpContext CreateContext()
            {
                return new DefaultHttpContext { RequestServices = _scope.ServiceProvider };
            }

            public AsyncServiceScope CreateScope()
            {
                return _provider.CreateAsyncScope();
            }

            public RequestDelegate BuildPipeline(RequestDelegate next)
            {
                var app = new ApplicationBuilder(_provider);
                _ = app.UseStandaloneTenantResolution();
                app.Run(next);
                return app.Build();
            }

            public Task RunAsync(HttpContext context, RequestDelegate next)
            {
                return BuildPipeline(next)(context);
            }

            public async Task SeedAsync(TenantLookupSource source, string value, bool isActive = true)
            {
                await using var database = Factory.CreateDbContext();
                _ = database.Tenants.Add(new Tenant { Id = TenantId, Name = "Customer", IsActive = isActive });
                _ = database.Lookups.Add(new TenantLookup { Source = source, Value = TenantLookup.Normalize(source, value), TenantId = TenantId });
                _ = await database.SaveChangesAsync(TestContext.Current.CancellationToken);
            }

            public async ValueTask DisposeAsync()
            {
                await _scope.DisposeAsync();
                await _provider.DisposeAsync();
                await _connection.DisposeAsync();
            }
        }
    }
}
