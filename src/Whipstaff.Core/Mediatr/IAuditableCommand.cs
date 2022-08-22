// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Security.Claims;

namespace Whipstaff.Core.Mediatr
{
    /// <summary>
    /// Represents a command that requires auditing.
    /// </summary>
    /// <typeparam name="TCommandDto">The type of the request object.</typeparam>
    /// <typeparam name="TResponse">The type of the response object.</typeparam>
    public interface IAuditableCommand<out TCommandDto, out TResponse> : IQuery<TResponse?>
    {
        /// <summary>
        /// Gets the command DTO for the actual command.
        /// </summary>
        TCommandDto CommandDto { get; }

        /// <summary>
        /// Gets the claims principal attached to the request.
        /// </summary>
        ClaimsPrincipal ClaimsPrincipal { get; }
    }
}
