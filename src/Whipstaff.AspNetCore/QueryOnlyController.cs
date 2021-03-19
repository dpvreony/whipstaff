// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Abstractions;
using Dhgms.AspNetCoreContrib.Controllers.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dhgms.AspNetCoreContrib.Controllers
{
    /// <summary>
    /// A generic controller supporting List and View operations. Pre-defines CQRS activities along with Authorization and logging.
    /// </summary>
    /// <typeparam name="TInheritingClass">The type of the inheriting class. Used for compile time validation of objects passed in such as the logger.</typeparam>
    /// <typeparam name="TListQuery">The type for the List Query.</typeparam>
    /// <typeparam name="TListRequestDto">The type for the Request DTO for the List Operation.</typeparam>
    /// <typeparam name="TListQueryResponse">The type for the Response DTO for the List Operation.</typeparam>
    /// <typeparam name="TViewQuery">The type for the View Query.</typeparam>
    /// <typeparam name="TViewQueryResponse">The type for the Response DTO for the View Operation.</typeparam>
    [SuppressMessage("csharpsquid", "S2436: Classes and methods should not have too many generic parameters", Justification = "By design, need large number of generics to make this powerful enough for ru-use in pattern")]
    public abstract class QueryOnlyController<TInheritingClass, TListQuery, TListRequestDto, TListQueryResponse, TViewQuery, TViewQueryResponse>
        : Controller
        where TInheritingClass : QueryOnlyController<TInheritingClass, TListQuery, TListRequestDto, TListQueryResponse, TViewQuery, TViewQueryResponse>
        where TListRequestDto : class, new()
        where TListQuery : IAuditableRequest<TListRequestDto, TListQueryResponse>
        where TListQueryResponse : class
        where TViewQuery : IAuditableRequest<long, TViewQueryResponse>
        where TViewQueryResponse : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryOnlyController{TInheritingClass, TListQuery, TListRequestDto, TListQueryResponse, TViewQuery, TViewQueryResponse}"/> class.
        /// </summary>
        /// <param name="authorizationService">The authorization service for validating access.</param>
        /// <param name="logger">The logger object.</param>
        /// <param name="mediator">The mediatr object to publish CQRS messages to.</param>
        /// <param name="queryFactory">The factory for generating Query messages.</param>
        protected QueryOnlyController(
            IAuthorizationService authorizationService,
            ILogger<TInheritingClass> logger,
            IMediator mediator,
            IAuditableQueryFactory<TListQuery, TListRequestDto, TListQueryResponse, TViewQuery, TViewQueryResponse> queryFactory)
        {
            AuthorizationService = authorizationService ??
                                         throw new ArgumentNullException(nameof(authorizationService));
            Logger = logger ??
                                         throw new ArgumentNullException(nameof(logger));

            Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            QueryFactory = queryFactory ??
                            throw new ArgumentNullException(nameof(queryFactory));
        }

        /// <summary>
        /// Gets the authorization service instance used to authorize requests.
        /// </summary>
        protected IAuthorizationService AuthorizationService { get; }

        /// <summary>
        /// Gets the logging framework instance.
        /// </summary>
        protected ILogger<TInheritingClass> Logger { get; }

        /// <summary>
        /// Gets the Mediator instance used for issuing CQRS messages.
        /// </summary>
        protected IMediator Mediator { get; }

        /// <summary>
        /// Gets the query factory for creating queries to push through the mediator.
        /// </summary>
        protected IAuditableQueryFactory<
            TListQuery,
            TListRequestDto,
            TListQueryResponse,
            TViewQuery,
            TViewQueryResponse> QueryFactory { get; }

        /// <summary>
        /// Entry point for HTTP GET operations.
        /// </summary>
        /// <remarks>
        /// Directs requests to List or View operations depending on whether an id is passed.
        /// </remarks>
        /// <param name="id">unique id of the entity to view. or null if being used to list.</param>
        /// <param name="cancellationToken">Cancellation token for the operations.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<IActionResult> Get(
            long? id,
            CancellationToken cancellationToken)
        {
            if (!Request.IsHttps)
            {
                return BadRequest();
            }

            if (!id.HasValue)
            {
                return await ListAsync(cancellationToken).ConfigureAwait(false);
            }

            return await ViewAsync(id.Value, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the Action Result for the List operation.
        /// </summary>
        /// <param name="listResponse">The Response DTO from the CQRS operation.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        protected abstract Task<IActionResult> GetListActionResultAsync(TListQueryResponse listResponse);

        /// <summary>
        /// Gets the CQRS List Query.
        /// </summary>
        /// <param name="listRequestDto">The List Request DTO.</param>
        /// <param name="claimsPrincipal">The claims principal for the user making the list request.</param>
        /// <param name="cancellationToken">The cancellation token for the request.</param>
        /// <returns>The list query</returns>
        protected abstract Task<TListQuery> GetListQueryAsync(
            TListRequestDto listRequestDto,
            ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken);

        /// <summary>
        /// Gets the event id for List Event. Used in logging and APM tools.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        protected abstract Task<EventId> GetListEventIdAsync();

        /// <summary>
        /// Gets the authorization policy for the List operation.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        protected abstract Task<string> GetListPolicyAsync();

        /// <summary>
        /// Gets the event id for View Event. Used in logging and APM tools.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        protected abstract Task<EventId> GetViewEventIdAsync();

        /// <summary>
        /// Gets the authorization policy for the View operation.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        protected abstract Task<string> GetViewPolicyAsync();

        /// <summary>
        /// Gets the Action Result for the View operation.
        /// </summary>
        /// <param name="viewResponse">The Response DTO from the CQRS operation.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        protected abstract Task<IActionResult> GetViewActionResultAsync(TViewQueryResponse viewResponse);

        /// <summary>
        /// Gets CQRS the View Query for the request.
        /// </summary>
        /// <param name="id">Unique id of the item to view.</param>
        /// <param name="claimsPrincipal">The claims principal of the user requesting the view.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns></returns>
        protected abstract Task<TViewQuery> GetViewQueryAsync(
            long id,
            ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken);

        private async Task<IActionResult> ListAsync(CancellationToken cancellationToken)
        {
            var eventId = await GetListEventIdAsync().ConfigureAwait(false);
            var listPolicyName = await GetListPolicyAsync().ConfigureAwait(false);
            var requestDto = new TListRequestDto();

            return await this.GetListActionAsync<TListRequestDto, TListQueryResponse, TListQuery>(
                Logger,
                Mediator,
                AuthorizationService,
                requestDto,
                eventId,
                listPolicyName,
                GetListActionResultAsync,
                GetListQueryAsync,
                cancellationToken).ConfigureAwait(false);
        }

        private async Task<IActionResult> ViewAsync(
            long id,
            CancellationToken cancellationToken)
        {
            var eventId = await GetViewEventIdAsync().ConfigureAwait(false);
            var viewPolicyName = await GetViewPolicyAsync().ConfigureAwait(false);

            return await this.GetViewActionAsync<long, TViewQueryResponse, TViewQuery>(
                Logger,
                Mediator,
                AuthorizationService,
                id,
                eventId,
                viewPolicyName,
                GetViewActionResultAsync,
                GetViewQueryAsync,
                cancellationToken).ConfigureAwait(false);
        }
    }
}
