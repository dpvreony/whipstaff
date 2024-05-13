// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using MediatR;

namespace Whipstaff.MediatR
{
    /// <summary>
    /// Represents a MediatR Command. These extend the <see cref="IRequest"/> to allow code by contract constraints downstream.
    /// </summary>
    /// <typeparam name="TCommand">Type for the command.</typeparam>
    /// <typeparam name="TResponse">Type for the response of the query.</typeparam>
    public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
    }
}
