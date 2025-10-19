// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Whipstaff.Mediator
{
    /// <summary>
    /// Details the registration information to be used to set up mediator.
    /// </summary>
    public interface IMediatorRegistration
    {
        /// <summary>
        /// Gets a list of request handlers to configure.
        /// </summary>
        IList<Func<IRequestHandlerRegistrationHandler>> RequestHandlers { get; }

        /// <summary>
        /// Gets a list of notification handlers to configure.
        /// </summary>
        IList<Func<INotificationHandlerRegistrationHandler>> NotificationHandlers { get; }

        /// <summary>
        /// Gets a list of request pre processors to configure.
        /// </summary>
        IList<Func<IRequestPreProcessorRegistrationHandler>> RequestPreProcessors { get; }

        /// <summary>
        /// Gets a list of request post processors to configure.
        /// </summary>
        IList<Func<IRequestPostProcessorRegistrationHandler>> RequestPostProcessors { get; }
    }
}
