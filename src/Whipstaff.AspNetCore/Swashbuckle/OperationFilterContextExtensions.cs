// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Whipstaff.AspNetCore.Swashbuckle
{
    /// <summary>
    /// Extension methods for <see cref="OperationFilterContext"/>.
    /// </summary>
    public static class OperationFilterContextExtensions
    {
        /// <summary>
        /// Ensures a type is registered in the Schema Repository.
        /// </summary>
        /// <typeparam name="T">Type to check is registered.</typeparam>
        /// <param name="context">Operation Filter Context.</param>
        /// <returns>Schema representation of the desired type.</returns>
        public static Microsoft.OpenApi.IOpenApiSchema EnsureTypeRegistered<T>(this OperationFilterContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            var type = typeof(T);
            if (!context.SchemaRepository.TryLookupByType(type, out var problemDetailsReferenceSchema))
            {
                var generatedSchema = context.SchemaGenerator.GenerateSchema(type, context.SchemaRepository);
                var schemaId = type.ToString();
                problemDetailsReferenceSchema =
                    context.SchemaRepository.AddDefinition(schemaId, (Microsoft.OpenApi.OpenApiSchema)generatedSchema);
                context.SchemaRepository.RegisterType(type, schemaId);
            }

            return problemDetailsReferenceSchema;
        }
    }
}
