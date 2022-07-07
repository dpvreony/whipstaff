// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;

namespace Whipstaff.AspNetCore.Features.Logging
{
    /// <summary>
    /// Interface for defining log message actions for a CRUD controller.
    /// This is to allow the performance recommendations for log message action definitions
    /// by moving the instantiation of them outside of the controllers.
    /// The interface mandates a constraint that the properties are initialized at object
    /// creation to ensure the message action is done in a way that the define call is only
    /// done once.
    /// </summary>
    public interface ICrudControllerLogMessageActions : IQueryOnlyControllerLogMessageActions
    {
        /// <summary>
        /// Gets the log message action for the add API event.
        /// </summary>
        public Action<ILogger, string, Exception?> AddEventLogMessageAction { get; init; }

        /// <summary>
        /// Gets the log message action for the delete API event.
        /// </summary>
        public Action<ILogger, string, Exception?> DeleteEventLogMessageAction { get; init; }

        /// <summary>
        /// Gets the log message action for the update API event.
        /// </summary>
        public Action<ILogger, string, Exception?> UpdateEventLogMessageAction { get; init; }
    }
}
