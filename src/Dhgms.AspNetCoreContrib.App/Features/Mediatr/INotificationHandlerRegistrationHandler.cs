// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Dhgms.AspNetCoreContrib.App.Features.Mediatr
{
    /// <summary>
    /// Registers a concrete type for handling Mediatr notifications.
    /// </summary>
    public interface INotificationHandlerRegistrationHandler
    {
        /// <summary>
        /// Registers the request handler to the DI service collection.
        /// </summary>
        /// <param name="services">DI service collection to register the handler to.</param>
        void AddNotificationHandler(IServiceCollection services);
    }
}
