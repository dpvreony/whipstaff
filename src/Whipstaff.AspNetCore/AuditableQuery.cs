// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Security.Claims;
using Whipstaff.Core.Mediatr;

namespace Whipstaff.AspNetCore
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuditableQuery{TCommandDto, TResponse}"/> class.
    /// </summary>
    /// <typeparam name="TQueryDto"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="QueryDto">The request dto for the call.</param>
    /// <param name="ClaimsPrincipal">The claims principal attached to the request.</param>
    public record AuditableQuery<TQueryDto, TResponse>(TQueryDto QueryDto, ClaimsPrincipal ClaimsPrincipal)
        : IAuditableQuery<TQueryDto, TResponse?>
    {
    }
}
