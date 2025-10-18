// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;

namespace Whipstaff.Mediator
{
    /// <summary>
    /// Represents a command factory for auditable Requests.
    /// The command is the message that will be pumped into the CQRS architecture, it is not running any logic itself.
    /// </summary>
    /// <typeparam name="TAddCommand">The type of the Add command.</typeparam>
    /// <typeparam name="TAddRequestDto">The type of the Request DTO for the Add Command.</typeparam>
    /// <typeparam name="TAddResponseDto">The type of the Response DTO for the Add Command.</typeparam>
    /// <typeparam name="TDeleteCommand">The type of the Delete command.</typeparam>
    /// <typeparam name="TDeleteResponseDto">The type of the Response DTO for the Delete Command.</typeparam>
    /// <typeparam name="TUpdateCommand">The type of the Update command.</typeparam>
    /// <typeparam name="TUpdateRequestDto">The type of the Request DTO for the Update Command.</typeparam>
    /// <typeparam name="TUpdateResponseDto">The type of the Response DTO for the Update Command.</typeparam>
    public interface IAuditableCommandFactory<TAddCommand, in TAddRequestDto, TAddResponseDto, TDeleteCommand, TDeleteResponseDto, TUpdateCommand, in TUpdateRequestDto, TUpdateResponseDto>
        where TAddCommand : IAuditableRequest<TAddRequestDto, TAddResponseDto?>
        where TDeleteCommand : IAuditableRequest<long, TDeleteResponseDto?>
        where TUpdateCommand : IAuditableRequest<TUpdateRequestDto, TUpdateResponseDto?>
    {
        /// <summary>
        /// Gets the auditable Command for use in an Add Operation.
        /// </summary>
        /// <param name="requestDto">The Request DTO for the Add Command.</param>
        /// <param name="claimsPrincipal">The Claims principal attached to the request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<TAddCommand> GetAddCommandAsync(
            TAddRequestDto requestDto,
            System.Security.Claims.ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken);

        /// <summary>
        /// Gets the auditable Command for use in an Delete Operation.
        /// </summary>
        /// <param name="id">The unique id of the object for the Delete Command.</param>
        /// <param name="claimsPrincipal">The Claims principal attached to the request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<TDeleteCommand> GetDeleteCommandAsync(
            long id,
            System.Security.Claims.ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken);

        /// <summary>
        /// Gets the auditable Command for use in an Update Operation.
        /// </summary>
        /// <param name="requestDto">The Request DTO for the Update Command.</param>
        /// <param name="claimsPrincipal">The Claims principal attached to the request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<TUpdateCommand> GetUpdateCommandAsync(
            TUpdateRequestDto requestDto,
            System.Security.Claims.ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken);
    }
}
