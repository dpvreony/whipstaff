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
        public IList<Type> NotificationHandlers { get; }

        /// <inheritdoc />
        public IList<Type> RequestPreProcessors { get; }

        /// <inheritdoc />
        public IList<Type> RequestPostProcessors { get; }
    }
}
