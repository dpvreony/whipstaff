// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

namespace Whipstaff.Aspire.Hosting
{
    /// <summary>
    /// Extensions for <see cref="IResourceBuilder{T}"/>.
    /// </summary>
    public static class ResourceBuilderExtensions
    {
        /// <summary>
        /// Add a reference to another resource and indicate that this resource should wait for the reference to complete.
        /// This is a wrapper around WaitForCompletion and WithReference to inline the operation.
        /// </summary>
        /// <typeparam name="TDestination">The type of the resource.</typeparam>
        /// <param name="builder">The resource builder for the resource that will be waiting.</param>
        /// <param name="source">The source dependency.</param>
        /// <returns>The resource builder.</returns>
        public static IResourceBuilder<TDestination> WaitForCompletionWithReference<TDestination>(
            this IResourceBuilder<TDestination> builder,
            IResourceBuilder<IResourceWithServiceDiscovery> source)
            where TDestination : IResourceWithEnvironment, IResourceWithWaitSupport
        {
            return builder.WithReference(source)
                .WaitForCompletion(source);
        }

        /// <summary>
        /// Add a reference to another resource and indicate that this resource should wait for the reference to complete.
        /// This is a wrapper around WaitForCompletion and WithReference to inline the operation.
        /// </summary>
        /// <typeparam name="TDestination">The type of the resource.</typeparam>
        /// <param name="builder">The resource builder for the resource that will be waiting.</param>
        /// <param name="endpointReference">The endpoint reference to associate.</param>
        /// <param name="source">The source dependency to wait upon.</param>
        /// <returns>The resource builder.</returns>
        public static IResourceBuilder<TDestination> WaitForCompletionWithReference<TDestination>(
            this IResourceBuilder<TDestination> builder,
            EndpointReference endpointReference,
            IResourceBuilder<IResource> source)
            where TDestination : IResourceWithEnvironment, IResourceWithWaitSupport
        {
            return builder.WithReference(endpointReference)
                .WaitForCompletion(source);
        }

        /// <summary>
        /// Add a reference to another resource and indicate that this resource should wait for the reference to complete.
        /// This is a wrapper around WaitFor and WithReference to inline the operation.
        /// </summary>
        /// <typeparam name="TDestination">The type of the resource.</typeparam>
        /// <param name="builder">The resource where connection string will be injected.</param>
        /// <param name="source">The resource from which to extract the connection string.</param>
        /// <param name="connectionName">An override of the source resource's name for the connection string. The resulting connection string will be "ConnectionStrings__connectionName" if this is not null.</param>
        /// <param name="optional"><see langword="true" /> to allow a missing connection string; <see langword="false" /> to throw an exception if the connection string is not found.</param>
        /// <returns>The resource builder.</returns>
        public static IResourceBuilder<TDestination> WaitForCompletionWithReference<TDestination>(
            this IResourceBuilder<TDestination> builder,
            IResourceBuilder<IResourceWithConnectionString> source,
            string? connectionName = null,
            bool optional = false)
            where TDestination : IResourceWithEnvironment, IResourceWithWaitSupport
        {
            return builder.WithReference(
                    source,
                    connectionName,
                    optional)
                .WaitFor(source);
        }

        /// <summary>
        /// Add a reference to another resource and indicate that this resource should wait for the reference to be ready.
        /// This is a wrapper around WaitFor and WithReference to inline the operation.
        /// </summary>
        /// <typeparam name="TDestination">The type of the resource.</typeparam>
        /// <param name="builder">The resource builder for the resource that will be waiting.</param>
        /// <param name="source">The source dependency.</param>
        /// <returns>The resource builder.</returns>
        public static IResourceBuilder<TDestination> WaitForWithReference<TDestination>(
            this IResourceBuilder<TDestination> builder,
            IResourceBuilder<IResourceWithServiceDiscovery> source)
            where TDestination : IResourceWithEnvironment, IResourceWithWaitSupport
        {
            return builder.WithReference(source)
                .WaitFor(source);
        }

        /// <summary>
        /// Add a reference to another resource and indicate that this resource should wait for the reference to be ready.
        /// This is a wrapper around WaitFor and WithReference to inline the operation.
        /// </summary>
        /// <typeparam name="TDestination">The type of the resource.</typeparam>
        /// <param name="builder">The resource builder for the resource that will be waiting.</param>
        /// <param name="endpointReference">The endpoint reference to associate.</param>
        /// <param name="source">The source dependency to wait upon.</param>
        /// <returns>The resource builder.</returns>
        public static IResourceBuilder<TDestination> WaitForWithReference<TDestination>(
            this IResourceBuilder<TDestination> builder,
            EndpointReference endpointReference,
            IResourceBuilder<IResource> source)
            where TDestination : IResourceWithEnvironment, IResourceWithWaitSupport
        {
            return builder.WithReference(endpointReference)
                .WaitFor(source);
        }

        /// <summary>
        /// Add a reference to another resource and indicate that this resource should wait for the reference to be ready.
        /// This is a wrapper around WaitFor and WithReference to inline the operation.
        /// </summary>
        /// <typeparam name="TDestination">The type of the resource.</typeparam>
        /// <param name="builder">The resource where connection string will be injected.</param>
        /// <param name="source">The resource from which to extract the connection string.</param>
        /// <param name="connectionName">An override of the source resource's name for the connection string. The resulting connection string will be "ConnectionStrings__connectionName" if this is not null.</param>
        /// <param name="optional"><see langword="true" /> to allow a missing connection string; <see langword="false" /> to throw an exception if the connection string is not found.</param>
        /// <returns>The resource builder.</returns>
        public static IResourceBuilder<TDestination> WaitForWithReference<TDestination>(
            this IResourceBuilder<TDestination> builder,
            IResourceBuilder<IResourceWithConnectionString> source,
            string? connectionName = null,
            bool optional = false)
            where TDestination : IResourceWithEnvironment, IResourceWithWaitSupport
        {
            return builder.WithReference(
                    source,
                    connectionName,
                    optional)
                .WaitFor(source);
        }
    }
}
