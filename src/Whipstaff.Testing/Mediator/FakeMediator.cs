// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Whipstaff.Testing.Cqrs;

namespace Whipstaff.Testing.Mediator
{
    /// <summary>
    /// Fake Mediator for testing purposes.
    /// </summary>
    public sealed class FakeMediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeMediator"/> class.
        /// </summary>
        /// <param name="serviceProvider">DI Service Provider with relevant CQRS handlers.</param>
        public FakeMediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamQuery<TResponse> query, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamCommand<TResponse> command, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<object?> CreateStream(object message, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async ValueTask Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
            where TNotification : INotification
        {
            await _serviceProvider.GetRequiredService<INotificationHandler<TNotification>>().Handle(
                notification,
                cancellationToken);
        }

        /// <inheritdoc/>
        public ValueTask Publish(object notification, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ValueTask<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async ValueTask<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
        {
            switch (command)
            {
                case FakeCrudAddCommand fakeCrudAddCommand:
#pragma warning disable CA1508 // Avoid dead conditional code
                    if (await _serviceProvider.GetRequiredService<ICommandHandler<FakeCrudAddCommand, int?>>().Handle(
                            fakeCrudAddCommand,
                            cancellationToken) is TResponse response)
                    {
                        return response;
                    }
#pragma warning restore CA1508 // Avoid dead conditional code

                    throw new InvalidOperationException("Problem casting result from command handler.");
                default:
                    throw new ArgumentException("Unregistered command type");
            }
        }

        /// <inheritdoc/>
        public async ValueTask<TResponse> Send<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
        {
            switch (query)
            {
                case FakeCrudListQuery fakeCrudListQuery:
                    return (TResponse)await _serviceProvider.GetRequiredService<IQueryHandler<FakeCrudListQuery, IList<int>>>().Handle(
                        fakeCrudListQuery,
                        cancellationToken);
                case FakeCrudViewQuery fakeCrudViewQuery:
                    if (await _serviceProvider.GetRequiredService<IQueryHandler<FakeCrudViewQuery, FakeCrudViewResponse?>>().Handle(
                            fakeCrudViewQuery,
                            cancellationToken) is TResponse response)
                    {
                        return response;
                    }

                    throw new InvalidOperationException("Problem casting result from query handler.");
                default:
                    throw new ArgumentException("Unregistered command type");
            }
        }

        /// <inheritdoc/>
        public ValueTask<object?> Send(object message, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
