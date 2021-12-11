using Microsoft.Extensions.Logging;

namespace Whipstaff.Core.Logging
{
    /// <summary>
    /// Factory for Event Ids.
    /// </summary>
    public static class EventIdFactory
    {
        /// <summary>
        /// Gets the Event Id for the Db Context Save Result logging event.
        /// </summary>
        /// <returns></returns>
        public static EventId DbContextSaveResultEventId() => new (1, "DbContext Save Result");

        /// <summary>
        /// Gets the Event Id for the No MediatR Handlers Registered for type logging event.
        /// </summary>
        /// <returns></returns>
        public static EventId NoMediatRHandlersRegisteredForTypeEventId() => new (1, "No MediatR Handlers Registered for type");

        /// <summary>
        /// Gets the Event Id for the Number of MediatR Handlers Registered for type logging event.
        /// </summary>
        /// <returns></returns>
        public static EventId CountOfMediatRHandlersRegisteredEventId() => new (1, "Number of MediatR Handlers Registered for type");
    }
}
