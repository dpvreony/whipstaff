// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Dhgms.AspNetCoreContrib.App.Features.Mediatr
{
    /// <summary>
    /// Registers a concrete type for handling Mediatr requests.
    /// </summary>
    /// <typeparam name="TImplementationType">The type for the request handler.</typeparam>
    /// <typeparam name="TRequest">The type for the request object that goes into the request handler.</typeparam>
    /// <typeparam name="TResponse">The type for the response object that comes out of the request handler.</typeparam>
    public sealed class RequestHandlerRegistrationHandler<TImplementationType, TRequest, TResponse>
        : IRequestHandlerRegistrationHandler
        where TImplementationType : class, IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        /// <inheritdoc />
        public void AddRequestHandler(IServiceCollection services)
        {
            services.AddTransient(
                typeof(IRequestHandler<TRequest, TResponse>),
                typeof(TImplementationType));
        }
    }
}
