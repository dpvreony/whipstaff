﻿// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using MediatR;
using Whipstaff.Core.Entities;
using Whipstaff.Core.Mediatr;

namespace Whipstaff.Testing.Cqrs
{
    /// <summary>
    /// Represents a Test Request by an int 32.
    /// </summary>
    public sealed class RequestById : IQuery<int>, IIntId
    {
        /// <inheritdoc />
        public int Id { get; init; }
    }
}
