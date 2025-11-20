// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Whipstaff.AspNetCore.Features.Swagger
{
    /// <inheritdoc />
    public class SwaggerClassMetaDataDocumentFilter : IDocumentFilter
    {
        /// <inheritdoc/>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
        }
    }
}
