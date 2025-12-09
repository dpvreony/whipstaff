// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Whipstaff.Mediator;
using Whipstaff.Testing.Cqrs;

namespace Whipstaff.Testing.MediatR
{
    /// <summary>
    /// Represents a Mediator code based registration.
    /// </summary>
    public sealed class FakeMediatrRegistration : IMediatorRegistration
    {
        /// <inheritdoc />
        public IList<Func<IRequestHandlerRegistrationHandler>> RequestHandlers => new
            List<Func<IRequestHandlerRegistrationHandler>>
        {
            () => new RequestHandlerRegistrationHandler<FakeCrudAddCommandHandler, FakeCrudAddCommand, int?>(),
            () => new RequestHandlerRegistrationHandler<FakeCrudListQueryHandler, FakeCrudListQuery, IList<int>?>(),
            () => new RequestHandlerRegistrationHandler<FakeCrudViewQueryHandler, FakeCrudViewQuery, FakeCrudViewResponse?>(),
        };

        /// <inheritdoc />
        public IList<Func<INotificationHandlerRegistrationHandler>> NotificationHandlers => new
            List<Func<INotificationHandlerRegistrationHandler>>
        {
            () => new NotificationHandlerRegistrationHandler<FakeNotificationHandler, FakeNotification>()
        };

        /// <inheritdoc />
        public IList<Func<IRequestPreProcessorRegistrationHandler>> RequestPreProcessors => new List<Func<IRequestPreProcessorRegistrationHandler>>
        {
            () => new RequestPreProcessorRegistrationHandler<FakePreProcessorCommandHandler, FakeCrudAddCommand>()
        };

        /// <inheritdoc />
        public IList<Func<IRequestPostProcessorRegistrationHandler>> RequestPostProcessors => new List<Func<IRequestPostProcessorRegistrationHandler>>
        {
            () => new RequestPostProcessorRegistrationHandler<FakePostProcessorCommandHandler, FakeCrudAddCommand, int?>()
        };
    }
}
