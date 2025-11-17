// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Whipstaff.AspNetCore.Swashbuckle
{
    /// <summary>
    /// Operation Filter to apply <see cref="ProblemDetails"/> to Http Status codes 400 or above.
    /// </summary>
    public sealed class ProblemDetailOperationFilter : IOperationFilter
    {
        /// <inheritdoc/>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            ArgumentNullException.ThrowIfNull(operation);
            ArgumentNullException.ThrowIfNull(context);

            if (context.ApiDescription.SupportedResponseTypes.Count < 1)
            {
                throw new ArgumentException(
                    "ApiDescription.SupportedResponseTypes must have at least 1 element",
                    nameof(context));
            }

            var supportedRequestFormats = context.ApiDescription.SupportedResponseTypes[0].ApiResponseFormats;

            var problemDetailsReferenceSchema = context.EnsureTypeRegistered<ProblemDetails>();

            operation.Responses.AssignReferenceSchemaToHttpStatusErrorCodes(
                supportedRequestFormats,
                problemDetailsReferenceSchema);
        }
    }
}
