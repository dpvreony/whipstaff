// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Mediator;

namespace Whipstaff.Mediator
{
    /// <summary>
    /// Registers a concrete type for Mediator pre processors.
    /// </summary>
    /// <typeparam name="TImplementationType">The type for the request handler.</typeparam>
    /// <typeparam name="TRequest">The type for the mediator request.</typeparam>
    public sealed class RequestPreProcessorRegistrationHandler<TImplementationType, TRequest>
        : IRequestPreProcessorRegistrationHandler
        where TImplementationType : class, IPipelineBehavior<TRequest, Unit>
        where TRequest : notnull
    {
        /// <inheritdoc/>
        public Type ServiceType => typeof(IPipelineBehavior<TRequest, Unit>);

        /// <inheritdoc/>
        public Type ImplementationType => typeof(TImplementationType);
    }
}
