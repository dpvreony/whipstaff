// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Protocol.Core.Types;

namespace Whipstaff.Nuget
{
    /// <summary>
    /// Extension methods for <see cref="SourceRepository" />.
    /// </summary>
    public static class SourceRepositoryExtensions
    {
        /// <summary>
        /// Gets a list of packages for the given author name in the specific source repository.
        /// </summary>
        /// <param name="sourceRepository">Source repository to check.</param>
        /// <param name="authorName">Username of the author.</param>
        /// <param name="logger">Nuget Logging Framework instance.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>List of packages for the author.</returns>
        public static async Task<IList<IPackageSearchMetadata>> GetPackagesForAuthor(
            this SourceRepository sourceRepository,
            AuthorUsernameAsStringModel authorName,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(sourceRepository);
            var packageSearchResource = await sourceRepository.GetResourceAsync<PackageSearchResource>(cancellationToken).ConfigureAwait(false);

            return await packageSearchResource.GetPackagesForAuthor(
                    authorName,
                    logger,
                    cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a list of packages for the given author name in the specific source repository.
        /// </summary>
        /// <typeparam name="TResult">Return type for the selected information.</typeparam>
        /// <param name="sourceRepository">Source repository to check.</param>
        /// <param name="authorName">Username of the author.</param>
        /// <param name="selector">selection predicate of the data to return.</param>
        /// <param name="logger">Nuget Logging Framework instance.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>List of selected output based on packages for the author.</returns>
        public static async Task<IList<TResult>> GetPackagesForAuthor<TResult>(
            this SourceRepository sourceRepository,
            AuthorUsernameAsStringModel authorName,
            Func<IPackageSearchMetadata, TResult> selector,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(sourceRepository);
            var packageSearchResource = await sourceRepository.GetResourceAsync<PackageSearchResource>(cancellationToken).ConfigureAwait(false);

            return await packageSearchResource.GetPackagesForAuthor(
                    authorName,
                    selector,
                    logger,
                    cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
