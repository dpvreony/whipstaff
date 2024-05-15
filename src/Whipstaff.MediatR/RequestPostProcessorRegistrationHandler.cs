// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using MediatR;
using MediatR.Pipeline;

namespace Whipstaff.MediatR
{
    /// <summary>
    /// Registers a concrete type for MediatR post processors.
    /// </summary>
    /// <typeparam name="TImplementationType">The type for the request handler.</typeparam>
    /// <typeparam name="TRequest">The type for the mediatr request.</typeparam>
    /// <typeparam name="TResponse">The type for the mediatr response.</typeparam>
    public sealed class RequestPostProcessorRegistrationHandler<TImplementationType, TRequest, TResponse>
        : IRequestPostProcessorRegistrationHandler
        where TImplementationType : class, IRequestPostProcessor<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        /// <inheritdoc/>
        public Type ServiceType => typeof(IRequestPostProcessor<TRequest, TResponse>);

        /// <inheritdoc/>
        public Type ImplementationType => typeof(TImplementationType);
    }
}
