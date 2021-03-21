// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Security.Claims;
using MediatR;

namespace Whipstaff.Core
{
    /// <summary>
    /// Represents a request that requires auditing.
    /// </summary>
    /// <typeparam name="TRequestDto">The type of the request object.</typeparam>
    /// <typeparam name="TResponse">The type of the response object.</typeparam>
    public interface IAuditableRequest<out TRequestDto, out TResponse> : IRequest<TResponse>
    {
        /// <summary>
        /// Gets the request DTO for.
        /// </summary>
        TRequestDto RequestDto { get; }

        /// <summary>
        /// Gets the claims principal attached to the request.
        /// </summary>
        ClaimsPrincipal ClaimsPrincipal { get; }
    }
}
