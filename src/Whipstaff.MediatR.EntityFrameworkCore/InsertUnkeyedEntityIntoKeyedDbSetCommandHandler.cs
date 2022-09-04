// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;
using Whipstaff.Core.Mediatr;

namespace Whipstaff.MediatR.EntityFrameworkCore
{
    public abstract class InsertUnkeyedEntityIntoKeyedDbSetCommandHandler<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
    {
        /// <inheritdoc/>
        public Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
