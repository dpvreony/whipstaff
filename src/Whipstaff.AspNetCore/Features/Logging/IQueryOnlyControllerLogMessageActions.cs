using System;
using Microsoft.Extensions.Logging;

namespace Whipstaff.AspNetCore.Features.Logging
{
    /// <summary>
    /// Interface for defining log message actions for a Query only controller.
    /// This is to allow the performance recommendations for log message action definitions
    /// by moving the instantiation of them outside of the controllers.
    /// The interface mandates a constraint that the properties are initialized at object
    /// creation to ensure the message action is done in a way that the define call is only
    /// done once.
    /// </summary>
    public interface IQueryOnlyControllerLogMessageActions
    {
        /// <summary>
        /// Gets the log message action for the list API event.
        /// </summary>
        public Action<ILogger, string, Exception?> ListEventLogMessageAction { get; init; }

        /// <summary>
        /// Gets the log message action for the view API event.
        /// </summary>
        public Action<ILogger, string, Exception?> ViewEventLogMessageAction { get; init; }
    }
}
