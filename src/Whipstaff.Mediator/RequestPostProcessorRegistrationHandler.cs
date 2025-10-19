// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Mediator;

namespace Whipstaff.Mediator
{
    /// <summary>
    /// Registers a concrete type for Mediator post processors.
    /// </summary>
    /// <typeparam name="TImplementationType">The type for the request handler.</typeparam>
    /// <typeparam name="TMessage">The type for the mediator message.</typeparam>
    /// <typeparam name="TResponse">The type for the mediator response.</typeparam>
    public sealed class RequestPostProcessorRegistrationHandler<TImplementationType, TMessage, TResponse>
        : IRequestPostProcessorRegistrationHandler
        where TImplementationType : class, IPipelineBehavior<TMessage, TResponse>
        where TMessage : IMessage
    {
        /// <inheritdoc/>
        public Type ServiceType => typeof(IPipelineBehavior<TMessage, TResponse>);

        /// <inheritdoc/>
        public Type ImplementationType => typeof(TImplementationType);
    }
}
