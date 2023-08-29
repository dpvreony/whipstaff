// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;
using Whipstaff.AspNetCore.Features.ApiAuthorization;

namespace Whipstaff.AspNetCore.Features.Mvc
{
    /// <summary>
    /// Helpers for <see cref="IApplicationModelConvention"/>.
    /// </summary>
    public static class ApplicationModelConventionHelpers
    {
        /// <summary>
        /// Ensures the "Add Authorization" policy controller convention is present.
        /// </summary>
        /// <param name="options">MVC options to check.</param>
        public static void EnsureAuthorizationPolicyControllerConvention(MvcOptions options)
        {
            ArgumentNullException.ThrowIfNull(options);

            if (options.Conventions.All(t => t.GetType() != typeof(AddAuthorizePolicyControllerConvention)))
            {
                options.Conventions.Add(new AddAuthorizePolicyControllerConvention());
            }
        }
    }
}
