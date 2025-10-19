// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Whipstaff.Core.Logging;

namespace Whipstaff.Mediator
{
    /// <summary>
    /// Helper methods for registration mediator without using reflection.
    /// </summary>
    public static class MediatorHelpers
    {
        /// <summary>
        /// Registers mediator to net core DI without relying on reflection.
        /// </summary>
        /// <param name="services">.Net Core services collection to register mediator to.</param>
        /// <param name="logger">Instance of logging framework.</param>
        /// <param name="mediatorRegistration">
        /// The mediator explicit registration details.
        /// This is the part specific to this functionality to avoid reflection.
        /// The object contains all the relevant details compiled up front.
        /// </param>
        public static void RegisterMediatorWithExplicitTypes(
            IServiceCollection services,
            ILogger? logger,
            IMediatorRegistration mediatorRegistration)
        {
            if (mediatorRegistration == null)
            {
                throw new ArgumentNullException(nameof(mediatorRegistration));
            }

            Register(
                services,
                logger,
                mediatorRegistration.RequestHandlers);

            Register(
                services,
                logger,
                mediatorRegistration.NotificationHandlers);

            Register(
                services,
                logger,
                mediatorRegistration.RequestPreProcessors);

            Register(
                services,
                logger,
                mediatorRegistration.RequestPostProcessors);
        }

        private static void Register<T>(
            IServiceCollection services,
            ILogger? logger,
            ICollection<Func<T>> registrations)
            where T : IMediatorRegistrationModel
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
