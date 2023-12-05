﻿// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Whipstaff.EntityFramework.ModelCreation;
using Whipstaff.Entityframework.Relational;

namespace Whipstaff.Testing.EntityFramework
{
    /// <summary>
    /// Sqlite Fake DB Context Model Creator.
    /// </summary>
    public sealed class SqliteFakeDbContextModelCreator : IModelCreator<FakeDbContext>
    {
        /// <inheritdoc/>
        public void CreateModel(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);

            ApplyDefaultToRowVersion(modelBuilder);
            ModelBuilderHelpers.ConvertAllDateTimeOffSetPropertiesOnModelBuilderToLong(modelBuilder);
        }

        private static void ApplyDefaultToRowVersion(ModelBuilder modelBuilder)
        {
            var properties = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties());

            foreach (var property in properties)
            {
                if (!property.Name.Equals("RowVersion", StringComparison.Ordinal))
                {
                    continue;
                }

                property.SetDefaultValue(0);
                property.SetProviderClrType(typeof(long));
            }
        }
    }
}
