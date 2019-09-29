// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Dhgms.AspNetCoreContrib.Fakes
{
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Claims;
    using Dhgms.AspNetCoreContrib.Controllers;

    [ExcludeFromCodeCoverage]
    public class FakeCrudUpdateCommand : AuditableRequest<int, FakeCrudUpdateResponse>
    {
        public FakeCrudUpdateCommand(int requestDto, ClaimsPrincipal claimsPrincipal)
            : base(requestDto, claimsPrincipal)
        {
        }
    }
}
