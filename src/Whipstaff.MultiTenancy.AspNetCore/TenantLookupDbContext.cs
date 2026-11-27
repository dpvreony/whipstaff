// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.EntityFrameworkCore;

namespace Whipstaff.MultiTenancy.AspNetCore
{
    /// <summary>A host-level database containing tenants and their lookup keys.</summary>
    public sealed class TenantLookupDbContext : DbContext
    {
        /// <summary>Initializes a new instance of the <see cref="TenantLookupDbContext"/> class.</summary>
        /// <param name="options">Database options.</param>
        public TenantLookupDbContext(DbContextOptions<TenantLookupDbContext> options)
            : base(options)
        {
        }

        /// <summary>Gets the tenants.</summary>
        public DbSet<Tenant> Tenants => Set<Tenant>();

        /// <summary>Gets the tenant resolution keys.</summary>
        public DbSet<TenantLookup> Lookups => Set<TenantLookup>();

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);
            base.OnModelCreating(modelBuilder);
            _ = modelBuilder.Entity<Tenant>().HasKey(x => x.Id);
            _ = modelBuilder.Entity<Tenant>().Property(x => x.Name).HasMaxLength(256).IsRequired();
            _ = modelBuilder.Entity<TenantLookup>().HasKey(x => new { x.Source, x.Value });
            _ = modelBuilder.Entity<TenantLookup>().Property(x => x.Value).HasMaxLength(320);
            _ = modelBuilder.Entity<TenantLookup>().HasOne(x => x.Tenant).WithMany().HasForeignKey(x => x.TenantId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
