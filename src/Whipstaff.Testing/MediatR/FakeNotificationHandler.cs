// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Whipstaff.Testing.Mediator;

namespace Whipstaff.Testing.MediatR
{
    /// <summary>
    /// Fake Notification Handler for Mediator.
    /// </summary>
    public sealed class FakeNotificationHandler : INotificationHandler<FakeNotification>
    {
        /// <inheritdoc />
        public Task Handle(FakeNotification notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
