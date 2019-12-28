using System;
using System.Collections.Generic;
using System.Text;
using Dhgms.AspNetCoreContrib.App.Features.Mediatr;
using MediatR;

namespace Dhgms.AspNetCoreContrib.Fakes
{
    /// <summary>
    /// Represents a Mediatr code based registration.
    /// </summary>
    public sealed class FakeMediatrRegistration : IMediatrRegistration
    {
        /// <inheritdoc />
        public IList<Func<IRequestHandlerRegistrationHandler>> RequestHandlers => new
            List<Func<IRequestHandlerRegistrationHandler>>
        {
            () => new RequestHandlerRegistrationHandler<FakeCrudAddCommandHandler, FakeCrudAddCommand, int>()
        };

        /// <inheritdoc />
        public IList<Func<INotificationHandlerRegistrationHandler>> NotificationHandlers { get; }

        /// <inheritdoc />
        public IList<Func<IRequestPreProcessorRegistrationHandler>> RequestPreProcessors { get; }

        /// <inheritdoc />
        public IList<Func<IRequestPostProcessorRegistrationHandler>> RequestPostProcessors { get; }
    }
}
