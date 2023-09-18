// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.EntityFrameworkCore;

namespace Whipstaff.EntityFramework.ModelCreation
{
    /// <summary>
    /// Interface for creating a model for a database context.
    /// </summary>
    /// <typeparam name="TDbContext">The database context for the model.</typeparam>
    public interface IModelCreator<TDbContext>
        where TDbContext : DbContext
    {
        /// <summary>
        /// Creates the model for the database context.
        /// </summary>
        /// <param name="modelBuilder">Model builder to apply the configuration to.</param>
        public void CreateModel(ModelBuilder modelBuilder);
    }
}
