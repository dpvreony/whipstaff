// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Whipstaff.Runtime.Exceptions;

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
                throw new NumberTooLowInteger32Exception()
            }

            var supportedRequestFormats = context.ApiDescription.SupportedResponseTypes[0].ApiResponseFormats;

            var problemDetailsReferenceSchema = context.EnsureTypeRegistered<ProblemDetails>();

            operation.Responses.AssignReferenceSchemaToHttpStatusErrorCodes(
                supportedRequestFormats,
                problemDetailsReferenceSchema);
        }
    }
}
