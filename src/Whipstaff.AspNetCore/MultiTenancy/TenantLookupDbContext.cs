// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Finbuckle.MultiTenant.Abstractions;
using Finbuckle.MultiTenant.EntityFrameworkCore.Stores;
using Microsoft.EntityFrameworkCore;

namespace Whipstaff.AspNetCore.MultiTenancy
{
    /// <summary>A database containing tenants and their resolution keys, independent of tenant databases.</summary>
    public sealed class TenantLookupDbContext : EFCoreStoreDbContext<TenantInfo>
    {
        /// <summary>Initializes a new instance of the <see cref="TenantLookupDbContext"/> class.</summary>
        /// <param name="options">Database options.</param>
        public TenantLookupDbContext(DbContextOptions<TenantLookupDbContext> options)
            : base(options)
        {
        }

        /// <summary>Gets the tenant resolution keys.</summary>
        public DbSet<TenantLookup> Lookups => Set<TenantLookup>();

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);
            base.OnModelCreating(modelBuilder);
            _ = modelBuilder.Entity<TenantInfo>().Property(x => x.Id).HasMaxLength(128);
            _ = modelBuilder.Entity<TenantInfo>().Property(x => x.Identifier).HasMaxLength(128);
            _ = modelBuilder.Entity<TenantLookup>().HasKey(x => new { x.Source, x.Value });
            _ = modelBuilder.Entity<TenantLookup>().Property(x => x.Value).HasMaxLength(320);
            _ = modelBuilder.Entity<TenantLookup>().Property(x => x.TenantId).HasMaxLength(128);
            _ = modelBuilder.Entity<TenantLookup>()
                .HasOne(x => x.Tenant)
                .WithMany()
                .HasForeignKey(x => x.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
