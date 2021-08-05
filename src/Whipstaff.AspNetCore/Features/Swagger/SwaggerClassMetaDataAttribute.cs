// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Whipstaff.AspNetCore.Features.Swagger
{
    /// <summary>
    /// Links a web controller class to a class that contains swagger metadata. Allows for compile time generation and linking.
    /// This is currently experimental and may change over time.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SwaggerClassMetaDataAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerClassMetaDataAttribute"/> class.
        /// </summary>
        /// <param name="metadataClass">The type for the metadata class.</param>
        public SwaggerClassMetaDataAttribute(Type metadataClass)
        {
            // todo: use a roslyn analyzer to ensure the type passed in is correct.
            MetadataClass = metadataClass ?? throw new ArgumentNullException(nameof(metadataClass));
        }

        /// <summary>
        /// Gets the metadata class.
        /// </summary>
        public Type MetadataClass { get; }
    }
}
