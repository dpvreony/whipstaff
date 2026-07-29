// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Whipstaff.Testing.Cqrs;

namespace Whipstaff.Testing.Mediator
{
    /// <summary>
    /// CQRS Query Handler for the CRUD View Query.
    /// </summary>
    public class FakeCrudViewQueryHandler : IQueryHandler<FakeCrudViewQuery, FakeCrudViewResponse?>
    {
        /// <inheritdoc />
        public async ValueTask<FakeCrudViewResponse?> Handle(FakeCrudViewQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            return await ValueTask.FromResult(new FakeCrudViewResponse(request.RequestDto));
        }
    }
}
