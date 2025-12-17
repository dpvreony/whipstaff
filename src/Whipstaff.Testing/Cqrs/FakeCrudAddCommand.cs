// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
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
    [ExcludeFromCodeCoverage]
    public class FakeCrudAddCommand : AuditableCommand<int, int?>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeCrudAddCommand"/> class.
        /// </summary>
        /// <param name="requestDto">The request dto.</param>
        /// <param name="claimsPrincipal">Claims principal associated with the request.</param>
        public FakeCrudAddCommand(int requestDto, ClaimsPrincipal claimsPrincipal)
            : base(requestDto, claimsPrincipal)
        {
        }
    }
}
