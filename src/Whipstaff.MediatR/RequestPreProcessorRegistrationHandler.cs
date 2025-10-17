// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using MediatR.Pipeline;

namespace Whipstaff.MediatR
{
    /// <summary>
    /// Registers a concrete type for MediatR pre processors.
    /// </summary>
    /// <typeparam name="TImplementationType">The type for the request handler.</typeparam>
    /// <typeparam name="TRequest">The type for the mediatr request.</typeparam>
    public sealed class RequestPreProcessorRegistrationHandler<TImplementationType, TRequest>
        : IRequestPreProcessorRegistrationHandler
        where TImplementationType : class, IRequestPreProcessor<TRequest>
        where TRequest : notnull
    {
        /// <inheritdoc/>
        public Type ServiceType => typeof(IRequestPreProcessor<TRequest>);

        /// <inheritdoc/>
        public Type ImplementationType => typeof(TImplementationType);
    }
}
