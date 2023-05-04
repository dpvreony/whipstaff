// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Protocol.Core.Types;

namespace Whipstaff.Nuget
{
    /// <summary>
    /// Extension methods for <see cref="PackageSearchResource" />.
    /// </summary>
    public static class PackageSearchResourceExtensions
    {
        /// <summary>
        /// Gets a list of packages for the given author name in the specific package search resource.
        /// </summary>
        /// <param name="packageSearchResource">Package search resource to check.</param>
        /// <param name="authorName">Username of the author.</param>
        /// <param name="logger">NuGet logging framework instance.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>List of packages for the author.</returns>
        public static async Task<IList<IPackageSearchMetadata>> GetPackagesForAuthor(
            this PackageSearchResource packageSearchResource,
            AuthorUsernameAsStringModel authorName,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(packageSearchResource);
            ArgumentNullException.ThrowIfNull(authorName);

            var result = new List<IPackageSearchMetadata>();
            var searchTerm = $"author:{authorName.Value}";
            var searchFilter = new SearchFilter(false);

            var packageCount = 0;
            var skip = 0;

            do
            {
                var searchResult = await packageSearchResource.SearchAsync(
                    searchTerm,
                    searchFilter,
                    skip: skip,
                    take: 1000,
                    logger,
                    cancellationToken).ConfigureAwait(false);

                var packages = searchResult.ToList();
                packageCount = packages.Count;

                result.AddRange(packages);
                skip += 1000;
            }
            while (packageCount == 1000);

            return result;
        }

        /// <summary>
        /// Gets a list of selected information per package for the given author name in the specific package search resource.
        /// </summary>
        /// <typeparam name="TResult">Return type for the selected information.</typeparam>
        /// <param name="packageSearchResource">Package search resource to check.</param>
        /// <param name="authorName">Username of the author.</param>
        /// <param name="selector">selection predicate of the data to return.</param>
        /// <param name="logger">NuGet logging framework instance.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>List of packages for the author.</returns>
        public static async Task<IList<TResult>> GetPackagesForAuthor<TResult>(
            this PackageSearchResource packageSearchResource,
            AuthorUsernameAsStringModel authorName,
            Func<IPackageSearchMetadata, TResult> selector,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(packageSearchResource);
            ArgumentNullException.ThrowIfNull(authorName);
            ArgumentNullException.ThrowIfNull(selector);
            ArgumentNullException.ThrowIfNull(logger);

            var result = new List<TResult>();
            var searchTerm = $"author:{authorName.Value}";
            var searchFilter = new SearchFilter(false);

            var packageCount = 0;

            var skip = 0;

            do
            {
                var searchResult = await packageSearchResource.SearchAsync(
                    searchTerm,
                    searchFilter,
                    skip: 0,
                    take: 1000,
                    logger,
                    cancellationToken).ConfigureAwait(false);

                var packages = searchResult.Select(selector)
                    .ToList();

                packageCount = packages.Count;
                result.AddRange(packages);

                skip += 1000;
            }
            while (packageCount == 1000);

            return result;
        }
    }
}
