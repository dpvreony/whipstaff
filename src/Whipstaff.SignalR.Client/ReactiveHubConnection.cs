// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.AspNetCore.SignalR.Client;
using ReactiveMarbles.ObservableEvents;

namespace Whipstaff.SignalR.Client
{
    /// <summary>
    /// Reactive Events for a SignalR Hub Connection.
    /// </summary>
    public sealed class ReactiveHubConnection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveHubConnection"/> class.
        /// </summary>
        /// <param name="hubConnection">Hub Connection Instance.</param>
        public ReactiveHubConnection(HubConnection hubConnection)
        {
            ArgumentNullException.ThrowIfNull(hubConnection);
            HubConnection = hubConnection;
        }

        /// <summary>
        /// Gets the underlying Hub Connection instance.
        /// </summary>
        public HubConnection HubConnection { get; }

        /// <summary>
        /// Gets an observable for monitoring the Hub Connection Closed Event.
        /// </summary>
        public IObservable<Exception> HubConnectionClosed => HubConnection.Events().Closed;

        /// <summary>
        /// Gets an observable for monitoring the Hub Connection Reconnected Event.
        /// </summary>
        public IObservable<string> HubConnectionReconnected => HubConnection.Events().Reconnected;

        /// <summary>
        /// Gets an observable for monitoring the Hub Connection Reconnecting Event.
        /// </summary>
        public IObservable<Exception> HubConnectionReconnecting => HubConnection.Events().Reconnecting;
    }
}
