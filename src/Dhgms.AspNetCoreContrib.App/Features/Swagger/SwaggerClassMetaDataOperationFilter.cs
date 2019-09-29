// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Abstractions;
using Dhgms.AspNetCoreContrib.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Dhgms.AspNetCoreContrib.App.Features.Swagger
{
    /// <summary>
    /// Class to apply Swagger Metadata described at the class level to allow
    /// for generic classes with re-usuable api functionality
    /// the still expose useful api documentation.
    /// </summary>
    public class SwaggerClassMetaDataOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            // var successResponseType = GetSuccessResponseType(operation, context);
            // if (successResponseType != null)
            // {
                // operation.Responses["200"].Schema = context.SchemaRegistry.GetOrRegister(successResponseType);
            // }

            operation.Responses["404"] = new Response { Description = "Not Found" };

            // var apiDesc = context.ApiDescription;

            //
            // if (customAttributes.All(ca => ca.AttributeType != typeof(SwaggerRequestExampleAttribute)))
            // {
                // customAttributes.
            // }

            // var attributes = GetActionAttributes(apiDesc);

            // if (!attributes.Any())
                // return;

            // if (operation.Responses == null)
            // {
                // operation.Responses = new Dictionary<string, Response>();
            // }

            // foreach (var attribute in attributes)
            // {
                // ApplyAttribute(operation, context, attribute);
            // }
        }

        private Type GetSuccessResponseType(Operation operation, OperationFilterContext context)
        {
            context.ApiDescription.TryGetMethodInfo(out var methodInfo);
            var declaringType = methodInfo.DeclaringType;
            var baseType = declaringType.BaseType;
            var comparison = typeof(QueryOnlyController<,,,,,>);
            if (IsSubclassOfRawGeneric(comparison, baseType))
            {
                var genericArgs = baseType.GenericTypeArguments;
                return genericArgs[3];
            }

            return null;
        }

        /// <summary>
        /// taken from https://stackoverflow.com/questions/457676/check-if-a-class-is-derived-from-a-generic-class
        /// </summary>
        /// <param name="generic"></param>
        /// <param name="toCheck"></param>
        /// <returns></returns>
        static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            // while (toCheck != null && toCheck != typeof(object)) {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur) {
                    return true;
                }
                // toCheck = toCheck.BaseType;
            // }
            return false;
        }

        private static void ApplyAttribute(Operation operation, OperationFilterContext context, SwaggerClassMetaDataAttribute attribute)
        {
            /*
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
            */
        }

        private static SwaggerClassMetaDataAttribute[] GetActionAttributes(ApiDescription apiDesc)
        {
            return apiDesc.ControllerAttributes().OfType<SwaggerClassMetaDataAttribute>().ToArray();
        }
    }
}
