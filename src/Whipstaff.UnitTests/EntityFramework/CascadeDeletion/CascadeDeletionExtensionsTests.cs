// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Whipstaff.EntityFramework.CascadeDeletion;
using Xunit;

namespace Whipstaff.UnitTests.EntityFramework.CascadeDeletion
{
    /// <summary>
    /// Unit Tests for <see cref="CascadeDeletionExtensions"/>.
    /// </summary>
    public static class CascadeDeletionExtensionsTests
    {
        /// <summary>
        /// Unit Tests for <see cref="CascadeDeletionExtensions.RemoveCascadeDeletionBehaviors(ModelBuilder)"/>.
        /// </summary>
        public sealed class RemoveCascadeDeletionBehaviorsModelBuilderMethod
        {
            /// <summary>
            /// Test to ensure cascade deletes are removed.
            /// </summary>
            [Fact]
            public void RemovesCascadeDeletes()
            {
                var modelBuilder = SqliteConventionSetBuilder.CreateModelBuilder();
                _ = modelBuilder.Entity<IdentityUser>(b =>
                {
                    _ = b.HasMany<IdentityUserRole<string>>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                });

                _ = modelBuilder.Entity<IdentityRole>(b =>
                {
                    _ = b.HasKey(r => r.Id);
                    _ = b.HasIndex(r => r.NormalizedName).HasDatabaseName("RoleNameIndex").IsUnique();
                    _ = b.ToTable("AspNetRoles");
                    _ = b.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

                    _ = b.Property(u => u.Name).HasMaxLength(256);
                    _ = b.Property(u => u.NormalizedName).HasMaxLength(256);

                    _ = b.HasMany<IdentityUserRole<string>>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                    _ = b.HasMany<IdentityRoleClaim<string>>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                });
                _ = modelBuilder.Entity<IdentityUserRole<string>>();

                Assert.NotEmpty(modelBuilder.GetForeignKeysWithCascadeDelete());

                modelBuilder.RemoveCascadeDeletionBehaviors();

                Assert.Empty(modelBuilder.GetForeignKeysWithCascadeDelete());
            }

            /// <summary>
            /// Test to the method doesn't fail when there are no cascade delete behaviors in the modell.
            /// </summary>
            [Fact]
            public void DoesNotThrowWhenNoCascadeDeleteBehaviors()
            {
                var modelBuilder = SqliteConventionSetBuilder.CreateModelBuilder();
                _ = modelBuilder.Entity<IdentityUser>();

                Assert.Empty(modelBuilder.GetForeignKeysWithCascadeDelete());

                modelBuilder.RemoveCascadeDeletionBehaviors();

                Assert.Empty(modelBuilder.GetForeignKeysWithCascadeDelete());
            }
        }
    }
}
