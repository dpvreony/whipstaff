// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace Whipstaff.Nuget
{
    /// <summary>
    /// Extension Methods for <see cref="PackageSource" />.
    /// </summary>
    public static class PackageSourceExtensions
    {
        /// <summary>
        /// Gets a list of packages for the given author name.
        /// </summary>
        /// <param name="packageSource">Package source to check.</param>
        /// <param name="authorName">Username of the author.</param>
        /// <param name="nugetForwardingToNetCoreLogger">Forwarding logger instance.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>List of packages for the author.</returns>
        public static async Task<IList<IPackageSearchMetadata>> GetPackagesForAuthor(
            this PackageSource packageSource,
            AuthorUsernameAsStringModel authorName,
            NugetForwardingToNetCoreLogger nugetForwardingToNetCoreLogger,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(packageSource);

            var packageSearchResource = await packageSource.GetPackageSearchResource()
                .ConfigureAwait(false);

            return await packageSearchResource.GetPackagesForAuthor(
                    authorName,
                    nugetForwardingToNetCoreLogger,
                    cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a list of selected information per package for the given author name.
        /// </summary>
        /// <typeparam name="TResult">Return type for the selected information.</typeparam>
        /// <param name="packageSource">Package source to check.</param>
        /// <param name="authorName">Username of the author.</param>
        /// <param name="selector">selection predicate of the data to return.</param>
        /// <param name="nugetForwardingToNetCoreLogger">Forwarding logger instance.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>List of selected output based on packages for the author.</returns>
        public static async Task<IList<TResult>> GetPackagesForAuthor<TResult>(
            this PackageSource packageSource,
            AuthorUsernameAsStringModel authorName,
            Func<IPackageSearchMetadata, TResult> selector,
            NugetForwardingToNetCoreLogger nugetForwardingToNetCoreLogger,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(packageSource);

            var packageSearchResource = await packageSource.GetPackageSearchResource()
                .ConfigureAwait(false);

            return await packageSearchResource.GetPackagesForAuthor(
                    authorName,
                    selector,
                    nugetForwardingToNetCoreLogger,
                    cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the source repository for the given package source.
        /// </summary>
        /// <param name="packageSource">Package source to check.</param>
        /// <returns>Source repository for the given package source.</returns>
        public static SourceRepository GetRepository(this PackageSource packageSource)
        {
            ArgumentNullException.ThrowIfNull(packageSource);

            return packageSource.ProtocolVersion switch
            {
                2 => Repository.Factory.GetCoreV2(packageSource),
                3 => Repository.Factory.GetCoreV3(packageSource),
                _ => throw new ArgumentException(
                    $"Protocol version {packageSource.ProtocolVersion} is not supported",
                    nameof(packageSource)),
            };
        }

        /// <summary>
        /// Gets the package search resource for the given package source.
        /// </summary>
        /// <param name="packageSource">Package source to check.</param>
        /// <returns>Package search resource for the given package source.</returns>
        public static Task<PackageSearchResource> GetPackageSearchResource(this PackageSource packageSource)
        {
            ArgumentNullException.ThrowIfNull(packageSource);

            var repository = packageSource.GetRepository();
            return repository.GetResourceAsync<PackageSearchResource>();
        }
    }
}
