// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using MediatR;

namespace Whipstaff.Core.Mediatr
{
    /// <summary>
    /// Represents a MediatR Query. These extend the <see cref="MediatR.IRequest"/> to allow code by contract constraints downstream.
    /// </summary>
    /// <typeparam name="TResponse">Response Type of the query.</typeparam>
#pragma warning disable CA1040 // Avoid empty interfaces
    public interface IQuery<out TResponse> : IRequest<TResponse>
#pragma warning restore CA1040 // Avoid empty interfaces
    {
    }
}
