// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Security.Claims;
using Whipstaff.Core;
using Whipstaff.Core.Mediatr;

namespace Whipstaff.AspNetCore
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuditableCommand{TCommandDto, TResponse}"/> class.
    /// </summary>
    /// <typeparam name="TCommandDto">The type of the request object.</typeparam>
    /// <typeparam name="TResponse">The type of the response object.</typeparam>
    /// <param name="CommandDto">The command dto for the call.</param>
    /// <param name="ClaimsPrincipal">The claims principal attached to the request.</param>
    public record AuditableCommand<TCommandDto, TResponse>(TCommandDto CommandDto, ClaimsPrincipal ClaimsPrincipal) : IAuditableCommand<TCommandDto, TResponse?>
    {
    }
}
