// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;

namespace Whipstaff.AspNetCore.Features.ApplicationStartup
{
    /// <summary>
    /// Extension methods for <see cref="IEndpointRouteBuilder"/>.
    /// </summary>
    public static class EndpointRouteBuilderExtensions
    {
        /// <summary>
        /// Maps a controller route for the standard CRUD operations.
        /// </summary>
        /// <param name="endpointRouteBuilder">Endpoint route builder instance to modify.</param>
        /// <param name="baseRoutePattern">
        /// The base route pattern. Usually along the lines of:
        ///
        /// "{controller}"
        /// "{controller=Home}"
        /// "api/{controller}"
        ///
        /// Note: don't use "{controller=Home}/{action=Index}/{id?}" the actions are bound by the HttpMethodRouteConstraint to HTTP verbs.
        /// </param>
        /// <param name="namePrefix">
        /// The name prefix to apply to the route names. Can be left as null if you're only mapping a single set of CRUD routes.
        /// Scenarios where you may want more are where you are mapping api's and pages to specific sets of controllers, but in that case you probably
        /// want to constrain the controllers which this method DOES NOT support.</param>
        /// <returns>The modified endpoint builder.</returns>
        public static IEndpointRouteBuilder DoCrudMapControllerRoute(this IEndpointRouteBuilder endpointRouteBuilder, string baseRoutePattern, string? namePrefix)
        {
            if (namePrefix != null)
            {
                namePrefix = namePrefix.Trim();
                if (!namePrefix.EndsWith('-'))
                {
                    namePrefix += '-';
                }
            }
            else
            {
                namePrefix = string.Empty;
            }

            _ = endpointRouteBuilder.MapControllerRoute(
                $"{namePrefix}create",
                baseRoutePattern,
                new { action = "Post" },
                new RouteValueDictionary(new { httpMethod = new HttpMethodRouteConstraint("POST") }));

            var idSuffixRoute = $"{baseRoutePattern}/{{id?}}";

            _ = endpointRouteBuilder.MapControllerRoute(
                $"{namePrefix}read",
                idSuffixRoute,
                new { action = "Get" },
                new RouteValueDictionary(new { httpMethod = new HttpMethodRouteConstraint("GET") }));

            _ = endpointRouteBuilder.MapControllerRoute(
                $"{namePrefix}update",
                idSuffixRoute,
                new { action = "Patch" },
                new RouteValueDictionary(new { httpMethod = new HttpMethodRouteConstraint("PATCH") }));

            _ = endpointRouteBuilder.MapControllerRoute(
                $"{namePrefix}delete",
                idSuffixRoute,
                new { action = "Delete" },
                new RouteValueDictionary(new { httpMethod = new HttpMethodRouteConstraint("DELETE") }));

            return endpointRouteBuilder;
        }
    }
}
