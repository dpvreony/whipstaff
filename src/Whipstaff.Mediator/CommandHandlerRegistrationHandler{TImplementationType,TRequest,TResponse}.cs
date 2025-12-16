// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Mediator;

namespace Whipstaff.Mediator
{
    /// <summary>
    /// Registers a concrete type for handling Mediator commands.
    /// </summary>
    /// <typeparam name="TImplementationType">The type for the request handler.</typeparam>
    /// <typeparam name="TRequest">The type for the request object that goes into the request handler.</typeparam>
    /// <typeparam name="TResponse">The type for the response object that comes out of the request handler.</typeparam>
    public sealed class CommandHandlerRegistrationHandler<TImplementationType, TRequest, TResponse>
        : ICommandHandlerRegistrationHandler
        where TImplementationType : class, ICommandHandler<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        /// <inheritdoc/>
        public Type ServiceType => typeof(ICommandHandler<TRequest, TResponse>);

        /// <inheritdoc/>
        public Type ImplementationType => typeof(TImplementationType);
    }
}
