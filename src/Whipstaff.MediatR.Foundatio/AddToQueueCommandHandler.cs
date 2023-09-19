// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Foundatio.Queues;
using MediatR;
using Microsoft.Extensions.Logging;
using Whipstaff.Core.Mediatr;

namespace Whipstaff.MediatR.Foundatio
{
    /// <summary>
    /// A simple MediatR Request Handler to push a RequestDto straight into a Queue.
    /// </summary>
    /// <typeparam name="TCommand">The type of the Command to enqueue.</typeparam>
    public class AddToQueueCommandHandler<TCommand> : ICommandHandler<TCommand, string>
        where TCommand : class, ICommand<string>
    {
        private readonly IQueue<TCommand> _queue;
        private readonly ILogger<AddToQueueCommandHandler<TCommand>> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddToQueueCommandHandler{TRequest}"/> class.
        /// </summary>
        /// <param name="queue">The queue to add the requests to.</param>
        /// <param name="logger"></param>
        public AddToQueueCommandHandler(global::Foundatio.Queues.IQueue<TCommand> queue, ILogger<AddToQueueCommandHandler<TCommand>> logger)
        {
            _queue = queue ?? throw new ArgumentNullException(nameof(queue));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public Task<string> Handle(TCommand request, CancellationToken cancellationToken)
        {
            return _queue.EnqueueAsync(request);
        }
    }
}
