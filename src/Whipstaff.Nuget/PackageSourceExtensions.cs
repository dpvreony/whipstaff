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

        public static SourceRepository GetRepository(this PackageSource packageSource)
        {
            ArgumentNullException.ThrowIfNull(packageSource);

            switch (packageSource.ProtocolVersion)
            {
                case 2:
                    return Repository.Factory.GetCoreV2(packageSource);
                case 3:
                    return Repository.Factory.GetCoreV3(packageSource);
                default:
                    throw new ArgumentException(
                        $"Protocol version {packageSource.ProtocolVersion} is not supported",
                        nameof(packageSource));
            }
        }

        public static Task<PackageSearchResource> GetPackageSearchResource(this PackageSource packageSource)
        {
            ArgumentNullException.ThrowIfNull(packageSource);

            var repository = packageSource.GetRepository();
            return repository.GetResourceAsync<PackageSearchResource>();
        }
    }
}
