// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Dhgms.AspNetCoreContrib.App.Features.Mediatr
{
    /// <summary>
    /// Registers a concrete type for MediatR notifications.
    /// </summary>
    /// <typeparam name="TImplementationType">The type for the request handler.</typeparam>
    /// <typeparam name="TNotification">The type for the notification.</typeparam>
    public sealed class NotificationHandlerRegistrationHandler<TImplementationType, TNotification>
        : INotificationHandlerRegistrationHandler
        where TImplementationType : class, INotificationHandler<TNotification>
        where TNotification : INotification
    {
        /// <inheritdoc/>
        public void AddNotificationHandler(IServiceCollection services)
        {
            services.AddTransient(
                typeof(INotificationHandler<TNotification>),
                typeof(TImplementationType));
        }
    }
}
