// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace Whipstaff.Nuget
{
    /// <summary>
    /// Helpers for working with the NuGet client API.
    /// </summary>
    public static class NugetClientHelpers
    {
        /// <summary>
        /// Gets a list of packages for the given author name.
        /// </summary>
        /// <param name="authorName">Username of the author.</param>
        /// <param name="nugetForwardingToNetCoreLogger">Forwarding logger instance.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>List of packages for the author.</returns>
        public static async Task<IList<IPackageSearchMetadata>> GetPackagesForAuthor(
            AuthorUsernameAsStringModel authorName,
            NugetForwardingToNetCoreLogger nugetForwardingToNetCoreLogger,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(authorName);
            ArgumentNullException.ThrowIfNull(nugetForwardingToNetCoreLogger);

            var packageSourceProvider = new PackageSourceProvider(new Settings(Environment.CurrentDirectory));
            var sources = packageSourceProvider.LoadPackageSources();
            var packages = new List<IPackageSearchMetadata>();

            foreach (var packageSource in sources)
            {
                var packagesForAuthor = await packageSource.GetPackagesForAuthor(
                        authorName,
                        nugetForwardingToNetCoreLogger,
                        cancellationToken)
                    .ConfigureAwait(false);

                packages.AddRange(packagesForAuthor);
            }

            return packages;
        }

        /// <summary>
        /// Gets a list of selected information per package for the given author name.
        /// </summary>
        /// <typeparam name="TResult">Return type for the selected information.</typeparam>
        /// <param name="authorName">Username of the author.</param>
        /// <param name="selector">selection predicate of the data to return.</param>
        /// <param name="nugetForwardingToNetCoreLogger">Forwarding logger instance.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>List of selected output based on packages for the author.</returns>
        public static async Task<IList<TResult>> GetPackagesForAuthor<TResult>(
            AuthorUsernameAsStringModel authorName,
            Func<IPackageSearchMetadata, TResult> selector,
            NugetForwardingToNetCoreLogger nugetForwardingToNetCoreLogger,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(selector);

            var packageSourceProvider = new PackageSourceProvider(new Settings(Environment.CurrentDirectory));
            var sources = packageSourceProvider.LoadPackageSources();
            var packages = new List<TResult>();

            foreach (var packageSource in sources)
            {
                var packagesForAuthor = await packageSource.GetPackagesForAuthor(
                        authorName,
                        selector,
                        nugetForwardingToNetCoreLogger,
                        cancellationToken)
                    .ConfigureAwait(false);

                packages.AddRange(packagesForAuthor);
            }

            return packages;
        }
    }
}
