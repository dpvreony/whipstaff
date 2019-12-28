// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Dhgms.AspNetCoreContrib.App.Features.Mediatr
{
    /// <summary>
    /// Helper methods for registration mediatr without using reflection.
    /// </summary>
    public static class MediatrHelpers
    {
        /// <summary>
        /// Registers mediatr to net core DI without relying on reflection.
        /// </summary>
        /// <param name="services">.Net Core services collection to register mediatr to.</param>
        /// <param name="logger">Instance of logging framework.</param>
        /// <param name="serviceConfiguration">The mediatr service configuration.</param>
        /// <param name="mediatrRegistration">
        /// The mediatr explicit registration details.
        /// This is the part specific to this functionality to avoid reflection.
        /// The object contains all the relevant details compiled up front.
        /// </param>
        public static void RegisterMediatrWithExplicitTypes(
            IServiceCollection services,
            ILogger logger,
            MediatRServiceConfiguration serviceConfiguration,
            IMediatrRegistration mediatrRegistration)
        {
            if (mediatrRegistration == null)
            {
                throw new ArgumentNullException(nameof(mediatrRegistration));
            }

            Register(
                services,
                logger,
                mediatrRegistration.RequestHandlers);

            Register(
                services,
                logger,
                mediatrRegistration.NotificationHandlers);

            Register(
                services,
                logger,
                mediatrRegistration.RequestPreProcessors);

            Register(
                services,
                logger,
                mediatrRegistration.RequestPostProcessors);

            MediatR.Registration.ServiceRegistrar.AddRequiredServices(services, serviceConfiguration);
        }

        private static void Register(
            IServiceCollection services,
            ILogger logger,
            IList<Func<IRequestPreProcessorRegistrationHandler>> mediatrRegistrationNotificationHandlers)
        {
            if (mediatrRegistrationNotificationHandlers == null || mediatrRegistrationNotificationHandlers.Count < 1)
            {
                logger?.LogInformation($"No mediatr handlers registered.");
                return;
            }

            logger?.LogInformation($"{mediatrRegistrationNotificationHandlers.Count} mediatr handlers registered.");

            foreach (var registration in mediatrRegistrationNotificationHandlers)
            {
                registration().AddRequestPreProcessor(services);
            }
        }

        private static void Register(
            IServiceCollection services,
            ILogger logger,
            IList<Func<IRequestPostProcessorRegistrationHandler>> mediatrRegistrationNotificationHandlers)
        {
            if (mediatrRegistrationNotificationHandlers == null || mediatrRegistrationNotificationHandlers.Count < 1)
            {
                logger?.LogInformation($"No mediatr handlers registered.");
                return;
            }

            logger?.LogInformation($"{mediatrRegistrationNotificationHandlers.Count} mediatr handlers registered.");

            foreach (var registration in mediatrRegistrationNotificationHandlers)
            {
                registration().AddRequestPostProcessor(services);
            }
        }

        private static void Register(
            IServiceCollection services,
            ILogger logger,
            IList<Func<INotificationHandlerRegistrationHandler>> mediatrRegistrationNotificationHandlers)
        {
            if (mediatrRegistrationNotificationHandlers == null || mediatrRegistrationNotificationHandlers.Count < 1)
            {
                logger?.LogInformation($"No mediatr handlers registered.");
                return;
            }

            logger?.LogInformation($"{mediatrRegistrationNotificationHandlers.Count} mediatr handlers registered.");

            foreach (var registration in mediatrRegistrationNotificationHandlers)
            {
                registration().AddNotificationHandler(services);
            }
        }

        private static void Register(
            IServiceCollection services,
            ILogger logger,
            IList<Func<IRequestHandlerRegistrationHandler>> mediatrRegistrationRequestHandlers)
        {
            if (mediatrRegistrationRequestHandlers == null || mediatrRegistrationRequestHandlers.Count < 1)
            {
                logger?.LogInformation($"No mediatr handlers registered.");
                return;
            }

            logger?.LogInformation($"{mediatrRegistrationRequestHandlers.Count} mediatr handlers registered.");

            foreach (var registration in mediatrRegistrationRequestHandlers)
            {
                registration().AddRequestHandler(services);
            }
        }
    }
}
