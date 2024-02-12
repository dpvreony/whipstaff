// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Whipstaff.EntityFramework.ModelCreation;

namespace Whipstaff.EntityFramework.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> in regards to Entity Framework.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add a DbContext with a model creator.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the Entity Framework DB Context.</typeparam>
        /// <typeparam name="TModelCreator">The type of the model creator to apply to the DB Context.</typeparam>
        /// <param name="serviceCollection">The <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add services to.</param>
        /// <param name="optionsAction">
        ///     <para>
        ///         An optional action to configure the <see cref="Microsoft.EntityFrameworkCore.DbContextOptions" /> for the context. This provides an
        ///         alternative to performing configuration of the context by overriding the
        ///         <see cref="Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)" /> method in your derived context.
        ///     </para>
        ///     <para>
        ///         If an action is supplied here, the <see cref="Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)" /> method will still be run if it has
        ///         been overridden on the derived context. <see cref="Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)" /> configuration will be applied
        ///         in addition to configuration performed here.
        ///     </para>
        ///     <para>
        ///         In order for the options to be passed into your context, you need to expose a constructor on your context that takes
        ///         <see cref="Microsoft.EntityFrameworkCore.DbContextOptions{TContext}" /> and passes it to the base constructor of <see cref="Microsoft.EntityFrameworkCore.DbContext" />.
        ///     </para>
        /// </param>
        /// <param name="contextLifetime">The lifetime with which to register the DbContext service in the container.</param>
        /// <param name="optionsLifetime">The lifetime with which to register the DbContextOptions service in the container.</param>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection AddDbContextWithModelCreator<TDbContext, TModelCreator>(
            this IServiceCollection serviceCollection,
            Action<IServiceProvider, DbContextOptionsBuilder>? optionsAction,
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
            ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
            where TDbContext : DbContext
            where TModelCreator : class, IModelCreator<TDbContext>, new()
        {
            serviceCollection = serviceCollection.AddSingleton<Func<IModelCreator<TDbContext>>>(() => new TModelCreator());
            return serviceCollection.AddDbContext<TDbContext>(
                optionsAction,
                contextLifetime,
                optionsLifetime);
        }
    }
}
