// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Security.Claims;

namespace Whipstaff.Core.Mediatr
{
    /// <summary>
    /// Represents a request that requires auditing.
    /// </summary>
    /// <typeparam name="TQueryDto">The type of the request object.</typeparam>
    /// <typeparam name="TResponse">The type of the response object.</typeparam>
    public interface IAuditableQuery<out TQueryDto, out TResponse> : IQuery<TResponse?>
    {
        /// <summary>
        /// Gets the query DTO for the actual query.
        /// </summary>
        TQueryDto QueryDto { get; }

        /// <summary>
        /// Gets the claims principal attached to the request.
        /// </summary>
        ClaimsPrincipal ClaimsPrincipal { get; }
    }
}
