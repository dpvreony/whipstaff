// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Whipstaff.AspNetCore;

namespace Whipstaff.Testing.Cqrs
{
    /// <summary>
    /// Represents a fake CRUD view query.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed record FakeCrudViewQuery(long QueryDto, ClaimsPrincipal ClaimsPrincipal) : AuditableQuery<long, FakeCrudViewResponse?>(QueryDto, ClaimsPrincipal)
    {
    }
}
