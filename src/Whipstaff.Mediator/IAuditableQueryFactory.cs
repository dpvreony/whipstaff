// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;

namespace Whipstaff.Mediator
{
    /// <summary>
    /// Represents a query factory for Auditable Requests.
    /// The command is the message that will be pumped into the CQRS architecture, it is not running any logic itself.
    /// The reason for having a factory is so you can push this into the controller without it having any knowledge
    /// of how the commands generate or operate.
    /// </summary>
    /// <typeparam name="TListQuery">The type of the List query.</typeparam>
    /// <typeparam name="TListRequestDto">The type of the Request DTO for the List Query.</typeparam>
    /// <typeparam name="TListResponse">The type of the Response DTO for the List Query.</typeparam>
    /// <typeparam name="TViewQuery">The type of the View query.</typeparam>
    /// <typeparam name="TViewResponse">The type of the Response DTO for the View Query.</typeparam>
    public interface IAuditableQueryFactory<TListQuery, in TListRequestDto, TListResponse, TViewQuery, TViewResponse>
        where TListQuery : IAuditableQuery<TListRequestDto, TListResponse>
        where TViewQuery : IAuditableQuery<long, TViewResponse?>
    {
        /// <summary>
        /// Gets the auditable Query for use in a List Operation.
        /// </summary>
        /// <param name="requestDto">The Request DTO for the List Query.</param>
        /// <param name="claimsPrincipal">The Claims principal attached to the request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<TListQuery> GetListQueryAsync(
            TListRequestDto requestDto,
            System.Security.Claims.ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken);

        /// <summary>
        /// Gets the auditable Query for use in a View Operation.
        /// </summary>
        /// <param name="id">The unique id for the view query.</param>
        /// <param name="claimsPrincipal">The Claims principal attached to the request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<TViewQuery> GetViewQueryAsync(
            long id,
            System.Security.Claims.ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken);
    }
}
