// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Mediator;

namespace Whipstaff.Mediator
{
    /// <summary>
    /// Represents a Mediator Query. These extend the <see cref="IRequest"/> to allow code by contract constraints downstream.
    /// </summary>
    /// <typeparam name="TQueryResponse">Response Type of the query.</typeparam>
#pragma warning disable CA1040 // Avoid empty interfaces
    public interface IQuery<out TQueryResponse> : IRequest<TQueryResponse>
#pragma warning restore CA1040 // Avoid empty interfaces
    {
    }
}
