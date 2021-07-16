// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using MediatR;
using Whipstaff.Core.Entities;

namespace Whipstaff.Testing.Cqrs
{
    /// <summary>
    /// Represents a Test Request by an int 32.
    /// </summary>
    public sealed class RequestById : IRequest<int>, IIntId
    {
        /// <inheritdoc />
        public int Id { get; init; }
    }
}
