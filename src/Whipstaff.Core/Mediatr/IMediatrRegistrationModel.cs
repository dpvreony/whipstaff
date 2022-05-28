﻿// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Whipstaff.Core.Mediatr
{
    /// <summary>
    /// Represents a MediatR registration.
    /// </summary>
    public interface IMediatrRegistrationModel
    {
        /// <summary>
        /// Gets the registration type.
        /// </summary>
        Type ServiceType { get; }

        /// <summary>
        /// Gets the implementation type.
        /// </summary>
        Type ImplementationType { get; }
    }
}
