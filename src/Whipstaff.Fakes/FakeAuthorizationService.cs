// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Whipstaff.Testing
{
    /// <summary>
    /// Fake Authorization Service.
    /// </summary>
    public sealed class FakeAuthorizationService : IAuthorizationService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeAuthorizationService"/> class.
        /// </summary>
        public FakeAuthorizationService()
        {
        }

        /// <summary>
        /// Gets the Func for <see cref="AuthorizeAsync(ClaimsPrincipal, object?, IEnumerable{IAuthorizationRequirement})"/>.
        /// </summary>
        public Func<ClaimsPrincipal, object?, IEnumerable<IAuthorizationRequirement>, Task<AuthorizationResult>>? AuthorizeAsyncUserResourceRequirementsFunc
        {
            get;
            init;
        }

        /// <summary>
        /// Gets the Func for <see cref="AuthorizeAsync(ClaimsPrincipal, object?, string)"/>.
        /// </summary>
        public Func<ClaimsPrincipal, object?, string, Task<AuthorizationResult>>? AuthorizeAsyncUserResourcePolicyNameFunc
        {
            get;
            init;
        }

        /// <inheritdoc/>
        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            if (AuthorizeAsyncUserResourceRequirementsFunc != null)
            {
                return AuthorizeAsyncUserResourceRequirementsFunc(user, resource, requirements);
            }

            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, string policyName)
        {
            if (AuthorizeAsyncUserResourcePolicyNameFunc != null)
            {
                return AuthorizeAsyncUserResourcePolicyNameFunc(user, resource, policyName);
            }

            throw new NotImplementedException();
        }
    }
}
