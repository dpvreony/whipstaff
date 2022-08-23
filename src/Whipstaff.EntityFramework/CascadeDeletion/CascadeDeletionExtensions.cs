// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Whipstaff.EntityFramework.CascadeDeletion
{
    /// <summary>
    /// Extension methods for modifying cascade delete behaviors.
    /// </summary>
    public static class CascadeDeletionExtensions
    {
        /// <summary>
        /// Walks through the entities and removes the foreign key cascade <see cref="DeleteBehavior"/>.
        /// </summary>
        /// <param name="modelBuilder">Model to check and modify.</param>
        public static void RemoveCascadeDeletionBehaviors(this ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);

            InternalRemoveCascadeDeletionBehaviors(modelBuilder.Model.GetEntityTypes());
        }

        /// <summary>
        /// Walks through the entities and removes the foreign key cascade <see cref="DeleteBehavior"/>.
        /// </summary>
        /// <param name="entityTypes">Entity Types to check and modify.</param>
        public static void RemoveCascadeDeletionBehaviors(this IEnumerable<IMutableEntityType> entityTypes)
        {
            ArgumentNullException.ThrowIfNull(entityTypes);

            InternalRemoveCascadeDeletionBehaviors(entityTypes);
        }

        private static void InternalRemoveCascadeDeletionBehaviors(IEnumerable<IMutableEntityType> entityTypes)
        {
            var foreignKeysWithCascadeDeleteBehaviour = entityTypes.SelectMany(t => t.GetForeignKeys())
                .Where(foreignKey => !foreignKey.IsOwnership && foreignKey.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var foreignKey in foreignKeysWithCascadeDeleteBehaviour)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
