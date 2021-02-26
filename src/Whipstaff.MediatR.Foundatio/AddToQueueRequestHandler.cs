// Copyright (c) 2020 DHGMS Solutions and Contributors. All rights reserved.
// DHGMS Solutions and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Foundatio.Queues;
using MediatR;

namespace Whipstaff.MediatR.Foundatio
{
    /// <summary>
    /// A simple MediatR Request Handler to push a RequestDto straight into a Queue.
    /// </summary>
    /// <typeparam name="TRequest">The type of the Request to enqueue.</typeparam>
    public class AddToQueueRequestHandler<TRequest> : IRequestHandler<TRequest, string>
        where TRequest : class, IRequest<string>
    {
        private readonly IQueue<TRequest> _queue;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddToQueueRequestHandler{TRequest}"/> class.
        /// </summary>
        /// <param name="queue">The queue to add the requests to.</param>
        public AddToQueueRequestHandler(global::Foundatio.Queues.IQueue<TRequest> queue)
        {
            _queue = queue ?? throw new ArgumentNullException(nameof(queue));
        }

        /// <inheritdoc/>
        public Task<string> Handle(TRequest request, CancellationToken cancellationToken)
        {
            return _queue.EnqueueAsync(request);
        }
    }
}
