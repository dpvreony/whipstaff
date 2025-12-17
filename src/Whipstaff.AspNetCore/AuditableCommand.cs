// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Security.Claims;
using Whipstaff.Mediator;

namespace Whipstaff.AspNetCore
{
    /// <inheritdoc />
    public class AuditableCommand<TRequestDto, TResponse> : IAuditableCommand<TRequestDto, TResponse>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditableCommand{TRequestDto,TResponse}"/> class.
        /// </summary>
        /// <param name="requestDto">The request dto for the call.</param>
        /// <param name="claimsPrincipal">The claims principal attached to the request.</param>
        public AuditableCommand(
            TRequestDto requestDto,
            ClaimsPrincipal claimsPrincipal)
        {
            RequestDto = requestDto;
            ClaimsPrincipal = claimsPrincipal;
        }

        /// <inheritdoc />
        public TRequestDto RequestDto { get; }

        /// <inheritdoc/>
        public ClaimsPrincipal ClaimsPrincipal { get; }
    }
}
