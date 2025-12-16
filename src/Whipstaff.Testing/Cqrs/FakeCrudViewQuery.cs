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
    public class FakeCrudViewQuery : AuditableQuery<long, FakeCrudViewResponse?>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeCrudViewQuery"/> class.
        /// </summary>
        /// <param name="requestDto">the unique id for the request.</param>
        /// <param name="claimsPrincipal">the claims principal associated with the request.</param>
        public FakeCrudViewQuery(long requestDto, ClaimsPrincipal claimsPrincipal)
            : base(requestDto, claimsPrincipal)
        {
        }
    }
}
