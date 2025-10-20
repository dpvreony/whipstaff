// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.EntityFrameworkCore;
using Whipstaff.Core.Entities;

namespace Whipstaff.Mediator.EntityFrameworkCore
{
    /// <summary>
    /// Abstract class for inserting an unkeyed entity into a <see cref="DbSet{TEntity}"/> that uses an <see cref="int"/> as the primary key.
    /// </summary>
    /// <typeparam name="TCommand">The type for the CQRS command.</typeparam>
    /// <typeparam name="TResponse">The type for the CQRS response.</typeparam>
    /// <typeparam name="TDbContext">The type for the <see cref="DbContext"/>.</typeparam>
    /// <typeparam name="TKeyedEntity">The type for the entity in the DbSet we will save to.</typeparam>
    public abstract class InsertUnkeyedEntityIntoInt32IdDbSetCommandHandler<TCommand, TResponse, TDbContext, TKeyedEntity>
        : InsertUnkeyedEntityIntoKeyedDbSetCommandHandler<TCommand, TResponse, TDbContext, TKeyedEntity>
        where TCommand : ICommand<TResponse>
        where TDbContext : DbContext
        where TKeyedEntity : class, IIntId
    {
    }
}
