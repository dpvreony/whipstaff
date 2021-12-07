// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace Whipstaff.AspNetCore.Features.ApiAuthorization
{
    /// <summary>
    /// Controller Model Convention to apply an authorization policy to all controllers.
    /// </summary>
    /// <remarks>
    /// Based upon https://joonasw.net/view/apply-authz-by-default (accessed: 2020-01-19).
    /// </remarks>
    public sealed class AddAuthorizePolicyControllerConvention : IControllerModelConvention
    {
        /// <inheritdoc/>
        public void Apply(ControllerModel controller)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            controller.Filters.Add(new AuthorizeFilter("ControllerAuthenticatedUser"));
        }
    }
}
