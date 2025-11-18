// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if TBC
using System;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using Whipstaff.AspNetCore.Features.Generics;

namespace Whipstaff.AspNetCore.Features.Swagger
{
    /// <summary>
    /// Class to apply Swagger Metadata described at the class level to allow
    /// for generic classes with re-usuable api functionality
    /// the still expose useful api documentation.
    /// </summary>
    public class SwaggerClassMetaDataOperationFilter : IOperationFilter
    {
        /// <inheritdoc/>
        public void Apply(
            OpenApiOperation operation,
            OperationFilterContext context)
        {
            ArgumentNullException.ThrowIfNull(operation);
            ArgumentNullException.ThrowIfNull(context);

            /*
            var successResponseType = GetSuccessResponseType(context);
            if (successResponseType != null)
            {
                operation.Responses["200"].Schema = context.SchemaRegistry.GetOrRegister(successResponseType);
            }

            operation.Responses["404"] = new Response { Description = "Not Found" };
            */

            /*
            var apiDesc = context.ApiDescription;


            if (customAttributes.All(ca => ca.AttributeType != typeof(SwaggerRequestExampleAttribute)))
            {
                customAttributes.
             }

            var attributes = GetActionAttributes(apiDesc);

            if (!attributes.Any())
                return;

            if (operation.Responses == null)
            {
                operation.Responses = new Dictionary<string, Response>();
            }

            foreach (var attribute in attributes)
            {
                ApplyAttribute(operation, context, attribute);
            }
            */
        }

        /*
        private static void ApplyAttribute(Operation operation, OperationFilterContext context, SwaggerClassMetaDataAttribute attribute)
        {
            var key = attribute.StatusCode.ToString();
            Response response;
            if (!operation.Responses.TryGetValue(key, out response))
            {
                response = new Response();
            }

            if (attribute.Description != null)
                response.Description = attribute.Description;

            if (attribute.Type != null && attribute.Type != typeof(void))
                response.Schema = context.SchemaRegistry.GetOrRegister(attribute.Type);

            operation.Responses[key] = response;
        }

        private static SwaggerClassMetaDataAttribute[] GetActionAttributes(ApiDescription apiDesc)
        {
            return apiDesc.ControllerAttributes().OfType<SwaggerClassMetaDataAttribute>().ToArray();
        }

        private static Type GetSuccessResponseType(OperationFilterContext context)
        {
            _ = context.ApiDescription.TryGetMethodInfo(out var methodInfo);
            var declaringType = methodInfo.DeclaringType;
            var baseType = declaringType.BaseType;
            var comparison = typeof(QueryOnlyController<,,,,,>);
            if (GenericHelpers.IsSubclassOfRawGeneric(comparison, baseType))
            {
                var genericArgs = baseType.GenericTypeArguments;
                return genericArgs[3];
            }

            return null;
        }
        */
    }
}
#endif
