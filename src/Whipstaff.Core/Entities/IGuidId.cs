// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Whipstaff.Core.Entities
{
    /// <summary>
    /// Represents an object that has a Guid as the Identifier.
    /// </summary>
    public interface IGuidId : IId<Guid>
    {
    }
}
