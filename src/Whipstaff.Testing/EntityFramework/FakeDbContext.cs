// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.EntityFrameworkCore;

namespace Whipstaff.Testing.EntityFramework
{
    /// <summary>
    /// Fake Entity Framework DBContext.
    /// </summary>
    public sealed class FakeDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeDbContext"/> class.
        /// </summary>
        /// <param name="options">Entity Framework DB Context options for initializing instance.</param>
        public FakeDbContext(DbContextOptions options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets the Fake Add Audit Db Set.
        /// </summary>
        public DbSet<DbSets.FakeAddAuditDbSet> FakeAddAudit => Set<DbSets.FakeAddAuditDbSet>();

        /// <summary>
        /// Gets the Fake Add Audit Pre Process Db Set.
        /// </summary>
        public DbSet<DbSets.FakeAddPreProcessAuditDbSet> FakeAddPreProcessAudit => Set<DbSets.FakeAddPreProcessAuditDbSet>();

        /// <summary>
        /// Gets the Fake Add Audit Post Process Db Set.
        /// </summary>
        public DbSet<DbSets.FakeAddPostProcessAuditDbSet> FakeAddPostProcessAudit => Set<DbSets.FakeAddPostProcessAuditDbSet>();

        /// <summary>
        /// Gets the Fake Long Id table Db Set.
        /// </summary>
        public DbSet<DbSets.FakeLongIdTableDbSet> FakeLongIdTable => Set<DbSets.FakeLongIdTableDbSet>();
    }
}
