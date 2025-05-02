// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Whipstaff.AspNetCore;

namespace Whipstaff.Testing.Cqrs
{
    /// <summary>
    /// Represents a fake CRUD list query.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class FakeCrudListQuery : AuditableRequest<FakeCrudListRequest, IList<int>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeCrudListQuery"/> class.
        /// </summary>
        /// <param name="requestDto">the request dto.</param>
        /// <param name="claimsPrincipal">the claims principal associated with the request.</param>
        public FakeCrudListQuery(FakeCrudListRequest requestDto, ClaimsPrincipal claimsPrincipal)
            : base(requestDto, claimsPrincipal)
        {
        }
    }
}
