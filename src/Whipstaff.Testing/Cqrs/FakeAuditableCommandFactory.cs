// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Whipstaff.Core;
using Whipstaff.MediatR;

namespace Whipstaff.Testing.Cqrs
{
    /// <inheritdoc/>
    public sealed class FakeAuditableCommandFactory : IAuditableCommandFactory<FakeCrudAddCommand, int, int?, FakeCrudDeleteCommand, long?, FakeCrudUpdateCommand, int, FakeCrudUpdateResponse?>
    {
        /// <inheritdoc/>
        public Task<FakeCrudAddCommand> GetAddCommandAsync(int requestDto, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            return Task.FromResult(new FakeCrudAddCommand(requestDto, claimsPrincipal));
        }

        /// <inheritdoc/>
        public Task<FakeCrudDeleteCommand> GetDeleteCommandAsync(long id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            return Task.FromResult(new FakeCrudDeleteCommand(id, claimsPrincipal));
        }

        /// <inheritdoc/>
        public Task<FakeCrudUpdateCommand> GetUpdateCommandAsync(int requestDto, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            return Task.FromResult(new FakeCrudUpdateCommand(requestDto, claimsPrincipal));
        }
    }
}
