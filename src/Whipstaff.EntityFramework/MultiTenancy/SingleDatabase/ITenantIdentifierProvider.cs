// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Whipstaff.Core.Entities;

namespace Whipstaff.EntityFramework.MultiTenancy.SingleDatabase
{
    /// <summary>
    /// Represents the tenant id that has been set on a request or process.
    /// </summary>
    /// <typeparam name="T">The type of the tenant id.</typeparam>
    public interface ITenantIdentifierProvider<out T>
        : ITenantId<T>
        where T : struct;
}
