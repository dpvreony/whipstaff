﻿// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Whipstaff.AspNetCore;

namespace Whipstaff.Testing.Cqrs
{
    /// <summary>
    /// Represents a fake CRUD CQRS Command.
    /// </summary>
    /// <param name="CommandDto">The command dto.</param>
    /// <param name="ClaimsPrincipal">Claims principal associated with the request.</param>
    [ExcludeFromCodeCoverage]
    public sealed record FakeCrudAddCommand(int CommandDto, ClaimsPrincipal ClaimsPrincipal) : AuditableCommand<int, int?>(CommandDto, ClaimsPrincipal)
    {
    }
}
