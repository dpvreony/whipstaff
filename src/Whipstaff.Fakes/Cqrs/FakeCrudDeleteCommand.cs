// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Whipstaff.AspNetCore;

namespace Whipstaff.Testing.Cqrs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FakeCrudDeleteCommand"/> class.
    /// </summary>
    /// <param name="CommandDto">The request dto.</param>
    /// <param name="ClaimsPrincipal">Claims principal associated with the request.</param>
    [ExcludeFromCodeCoverage]
    public sealed record FakeCrudDeleteCommand(long CommandDto, ClaimsPrincipal ClaimsPrincipal) : AuditableCommand<long, long?>(CommandDto, ClaimsPrincipal)
    {
    }
}
