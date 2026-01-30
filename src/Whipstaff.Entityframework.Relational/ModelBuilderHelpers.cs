// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Whipstaff.EntityFramework.Relational
{
    /// <summary>
    /// Entity Framework Model Builder Helpers for SQL Lite.
    /// </summary>
    public static class ModelBuilderHelpers
    {
        /// <summary>
        /// Converts all DBSet entities that contain a DateTimeOffset column to use a long database type within SQL Lite.
        /// Is used to workaround a limitation in SQL lite where you can't store as a DateTimeOffset and the workaround
        /// is to use a string or DateTime and lose the precision. Instead, so you don't need to adjust your model to cater
        /// for SQL lite, you can retain the ability of databases that do support it, but use SQL lite for testing.
        /// The caveat is that SQL lite loses timezone precision as it converts everything to UTC, but then you should
        /// probably be storing the data as UTC anyway.
        /// </summary>
        /// <param name="modelBuilder">Entity Framework Model Builder being configured.</param>
        public static void ConvertAllDateTimeOffSetPropertiesOnModelBuilderToLong(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);

            foreach (var mutableEntityType in modelBuilder.Model.GetEntityTypes())
            {
                InternalConvertAllDateTimeOffSetPropertiesOnMutableEntityToLong(mutableEntityType);
            }
        }

        /// <summary>
        /// Converts all properties on a DBSet entity that are a DateTimeOffset column to use a long database type within SQL Lite.
        /// Is used to workaround a limitation in SQL lite where you can't store as a DateTimeOffset and the workaround
        /// is to use a string or DateTime and lose the precision. Instead, so you don't need to adjust your model to cater
        /// for SQL lite, you can retain the ability of databases that do support it, but use SQL lite for testing.
        /// The caveat is that SQL lite loses timezone precision as it converts everything to UTC, but then you should
        /// probably be storing the data as UTC anyway.
        /// </summary>
        /// <param name="mutableEntityType">Mutable Entity Type Representing the DBSet to check.</param>
        public static void ConvertAllDateTimeOffSetPropertiesOnMutableEntityToLong(IMutableEntityType mutableEntityType)
        {
            ArgumentNullException.ThrowIfNull(mutableEntityType);

            InternalConvertAllDateTimeOffSetPropertiesOnMutableEntityToLong(mutableEntityType);
        }

        /// <summary>
        /// Converts a property properties on a DBSet entity that are a DateTimeOffset column to use a long database type within SQL Lite.
        /// Is used to workaround a limitation in SQL lite where you can't store as a DateTimeOffset and the workaround
        /// is to use a string or DateTime and lose the precision. Instead, so you don't need to adjust your model to cater
        /// for SQL lite, you can retain the ability of databases that do support it, but use SQL lite for testing.
        /// The caveat is that SQL lite loses timezone precision as it converts everything to UTC, but then you should
        /// probably be storing the data as UTC anyway.
        /// </summary>
        /// <param name="modelBuilder">Entity Framework Model Builder being configured.</param>
        /// <param name="entityClrType">The CLR type of the Entity represented as a DBSet.</param>
        /// <param name="propertyName">The name of the property that's to be converted from DateTimeOffset.</param>
        /// <returns>Property builder for the column..</returns>
        public static PropertyBuilder ConvertDateTimeOffSetPropertyToLong(
            ModelBuilder modelBuilder,
            Type entityClrType,
            string propertyName)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);
            ArgumentNullException.ThrowIfNull(entityClrType);

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            return InternalConvertDateTimeOffSetPropertyToLong(
                modelBuilder,
                entityClrType,
                propertyName);
        }

        /// <summary>
        /// Gets a value convertor for converting a date time offset to unix time milliseconds as a long.
        /// </summary>
        /// <returns>Value convertor.</returns>
        public static ValueConverter<DateTimeOffset, long> GetDateTimeOffSetToUnixTimeMillisecondsLongValueConverter() =>
            new(
                offset => offset.ToUnixTimeMilliseconds(),
                milliseconds => DateTimeOffset.FromUnixTimeMilliseconds(milliseconds));

        private static void InternalConvertAllDateTimeOffSetPropertiesOnMutableEntityToLong(IMutableEntityType mutableEntityType)
        {
            foreach (var p in mutableEntityType.GetProperties())
            {
                if (p.ClrType == typeof(DateTimeOffset))
                {
                    InternalConvertDateTimeOffSetPropertyToLong(p);
                }
            }
        }

        private static PropertyBuilder InternalConvertDateTimeOffSetPropertyToLong(
            ModelBuilder modelBuilder,
            Type entityClrType,
            string propertyName)
        {
            return modelBuilder.Entity(entityClrType)
                .Property(propertyName)
                .HasColumnType("INTEGER")
                .HasConversion(GetDateTimeOffSetToUnixTimeMillisecondsLongValueConverter());
        }

        private static void InternalConvertDateTimeOffSetPropertyToLong(IMutableProperty mutableProperty)
        {
            mutableProperty.SetColumnType("INTEGER");
            mutableProperty.SetValueConverter(GetDateTimeOffSetToUnixTimeMillisecondsLongValueConverter());
        }
    }
}
