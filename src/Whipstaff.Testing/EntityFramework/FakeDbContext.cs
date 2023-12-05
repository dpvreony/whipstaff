// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.EntityFrameworkCore;
using Whipstaff.EntityFramework.ModelCreation;

namespace Whipstaff.Testing.EntityFramework
{
    /// <summary>
    /// Fake Entity Framework DBContext.
    /// </summary>
    public sealed class FakeDbContext : DbContext
    {
        private readonly Func<IModelCreator<FakeDbContext>> _modelCreatorFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeDbContext"/> class.
        /// </summary>
        /// <param name="options">Entity Framework DB Context options for initializing instance.</param>
        /// <param name="modelCreatorFunc">Function used to build the database model. Allows for extra control for versions, features, and provider specific customization to be injected.</param>
        public FakeDbContext(
            DbContextOptions<FakeDbContext> options,
            Func<IModelCreator<FakeDbContext>> modelCreatorFunc)
            : base(options)
        {
            ArgumentNullException.ThrowIfNull(modelCreatorFunc);
            _modelCreatorFunc = modelCreatorFunc;
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

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            _modelCreatorFunc().CreateModel(modelBuilder);
        }
    }
}
