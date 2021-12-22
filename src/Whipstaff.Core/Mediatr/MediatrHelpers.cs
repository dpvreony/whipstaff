// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Whipstaff.Core.Logging;

namespace Whipstaff.Core.Mediatr
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
            ILogger? logger,
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

        private static void Register<T>(
            IServiceCollection services,
            ILogger? logger,
            ICollection<Func<T>> registrations)
            where T : IMediatrRegistrationModel
        {
            if (registrations == null || registrations.Count < 1)
            {
                if (logger != null)
                {
                    var loggerMessage = LoggerMessageFactory.GetNoMediatRHandlersRegisteredForTypeLoggerMessageAction();
                    loggerMessage(logger, typeof(T), null);
                }

                return;
            }

            foreach (var registration in registrations)
            {
                var registrationInstance = registration();
                var serviceType = registrationInstance.ServiceType;
                var implementationType = registrationInstance.ImplementationType;

                _ = services.AddTransient(
                    serviceType,
                    implementationType);
            }

            if (logger != null)
            {
                var loggerMessage = LoggerMessageFactory.GetCountOfMediatRHandlersRegisteredLoggerMessageAction();
                loggerMessage(logger, typeof(T), registrations.Count, null);
            }
        }
    }
}
