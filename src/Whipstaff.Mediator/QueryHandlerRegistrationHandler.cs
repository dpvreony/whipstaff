// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Mediator;

namespace Whipstaff.Mediator
{
    /// <summary>
    /// Registers a concrete type for handling Mediator queries.
    /// </summary>
    /// <typeparam name="TImplementationType">The type for the request handler.</typeparam>
    /// <typeparam name="TQuery">The type for the request object that goes into the query handler.</typeparam>
    /// <typeparam name="TResponse">The type for the response object that comes out of the request handler.</typeparam>
    public sealed class QueryHandlerRegistrationHandler<TImplementationType, TQuery, TResponse>
        : IQueryHandlerRegistrationHandler
        where TImplementationType : class, IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        /// <inheritdoc/>
        public Type ServiceType => typeof(IQueryHandler<TQuery, TResponse>);

        /// <inheritdoc/>
        public Type ImplementationType => typeof(TImplementationType);
    }
}
