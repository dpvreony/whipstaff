// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Whipstaff.AspNetCore.Extensions;
using Whipstaff.AspNetCore.Features.Logging;
using Whipstaff.Core;
using Whipstaff.MediatR;

namespace Whipstaff.AspNetCore
{
    /// <summary>
    /// A generic API controller supporting List and View operations. Pre-defines CQRS activities along with Authorization and logging.
    /// </summary>
    /// <typeparam name="TListQuery">The type for the List Query.</typeparam>
    /// <typeparam name="TListRequestDto">The type for the Request DTO for the List Operation.</typeparam>
    /// <typeparam name="TListQueryResponse">The type for the Response DTO for the List Operation.</typeparam>
    /// <typeparam name="TViewQuery">The type for the View Query.</typeparam>
    /// <typeparam name="TViewQueryResponse">The type for the Response DTO for the View Operation.</typeparam>
    /// <typeparam name="TQueryOnlyControllerLogMessageActions">The type for the log message actions mapping class.</typeparam>
#pragma warning disable S6934
    public abstract class QueryOnlyApiController<TListQuery, TListRequestDto, TListQueryResponse, TViewQuery, TViewQueryResponse, TQueryOnlyControllerLogMessageActions>
#pragma warning restore S6934
        : ControllerBase
        where TListQuery : IAuditableRequest<TListRequestDto, TListQueryResponse>
        where TListRequestDto : class, new()
        where TListQueryResponse : class
        where TViewQuery : IAuditableRequest<long, TViewQueryResponse?>
        where TViewQueryResponse : class
        where TQueryOnlyControllerLogMessageActions : IQueryOnlyControllerLogMessageActions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryOnlyApiController{TListQuery, TListRequestDto, TListQueryResponse, TViewQuery, TViewQueryResponse, TQueryOnlyControllerLogMessageActions}"/> class.
        /// </summary>
        /// <param name="authorizationService">The authorization service for validating access.</param>
        /// <param name="logger">The logger object.</param>
        /// <param name="mediator">The mediatr object to publish CQRS messages to.</param>
        /// <param name="queryFactory">The factory for generating Query messages.</param>
        /// <param name="logMessageActionMappings">Log Message Action mappings.</param>
        protected QueryOnlyApiController(
            IAuthorizationService authorizationService,
            ILogger<QueryOnlyApiController<TListQuery, TListRequestDto, TListQueryResponse, TViewQuery, TViewQueryResponse, TQueryOnlyControllerLogMessageActions>> logger,
            IMediator mediator,
            IAuditableQueryFactory<TListQuery, TListRequestDto, TListQueryResponse, TViewQuery, TViewQueryResponse> queryFactory,
            TQueryOnlyControllerLogMessageActions logMessageActionMappings)
        {
            ArgumentNullException.ThrowIfNull(authorizationService);
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(mediator);
            ArgumentNullException.ThrowIfNull(queryFactory);
            ArgumentNullException.ThrowIfNull(logMessageActionMappings);

            AuthorizationService = authorizationService;
            Logger = logger;
            Mediator = mediator;
            QueryFactory = queryFactory;
            LogMessageActionMappings = logMessageActionMappings;
        }

        /// <summary>
        /// Gets the authorization service instance used to authorize requests.
        /// </summary>
        protected IAuthorizationService AuthorizationService { get; }

        /// <summary>
        /// Gets the logging framework instance.
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// Gets the log message action mappings.
        /// </summary>
        protected TQueryOnlyControllerLogMessageActions LogMessageActionMappings { get; }

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
        /// Entry point for HTTP GET operations to list items.
        /// </summary>
        /// <remarks>
        /// Directs requests to List or View operations depending on whether an id is passed.
        /// </remarks>
        /// <param name="cancellationToken">Cancellation token for the operations.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet]
        public async Task<ActionResult<TListQueryResponse>> ListAsync(CancellationToken cancellationToken)
        {
            var listPolicyName = await GetListPolicyAsync().ConfigureAwait(false);
            var requestDto = new TListRequestDto();

            return await this.GetApiListActionAsync<TListRequestDto, TListQueryResponse, TListQuery>(
                Logger,
                Mediator,
                AuthorizationService,
                requestDto,
                LogMessageActionMappings.ListEventLogMessageAction,
                listPolicyName,
                GetListActionResultAsync,
                GetListQueryAsync,
                cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Entry point for HTTP GET operation to view items.
        /// </summary>
        /// <remarks>
        /// Directs requests to List or View operations depending on whether an id is passed.
        /// </remarks>
        /// <param name="id">unique id of the entity to view.</param>
        /// <param name="cancellationToken">Cancellation token for the operations.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet("{id:long}")]
#pragma warning disable S6967
        public async Task<ActionResult<TViewQueryResponse>> ViewAsync(
            long id,
            CancellationToken cancellationToken)
#pragma warning restore S6967
        {
            var viewPolicyName = await GetViewPolicyAsync().ConfigureAwait(false);

            return await this.GetApiViewActionAsync<long, TViewQueryResponse, TViewQuery>(
                Logger,
                Mediator,
                AuthorizationService,
                id,
                LogMessageActionMappings.ViewEventLogMessageAction,
                viewPolicyName,
                GetViewActionResultAsync,
                GetViewQueryAsync,
                cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the Action Result for the List operation.
        /// </summary>
        /// <param name="listResponse">The Response DTO from the CQRS operation.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        protected abstract Task<ActionResult<TListQueryResponse>> GetListActionResultAsync(TListQueryResponse listResponse);

        /// <summary>
        /// Gets the Action Result for the View operation.
        /// </summary>
        /// <param name="viewResponse">The Response DTO from the CQRS operation.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        protected abstract Task<ActionResult<TViewQueryResponse>> GetViewActionResultAsync(TViewQueryResponse viewResponse);

        /// <summary>
        /// Gets the CQRS List Query.
        /// </summary>
        /// <param name="listRequestDto">The List Request DTO.</param>
        /// <param name="claimsPrincipal">The claims principal for the user making the list request.</param>
        /// <param name="cancellationToken">The cancellation token for the request.</param>
        /// <returns>The list query.</returns>
        protected abstract Task<TListQuery> GetListQueryAsync(
            TListRequestDto listRequestDto,
            ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken);

        /// <summary>
        /// Gets the authorization policy for the List operation.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        protected abstract Task<string> GetListPolicyAsync();

        /// <summary>
        /// Gets the authorization policy for the View operation.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        protected abstract Task<string> GetViewPolicyAsync();

        /// <summary>
        /// Gets CQRS the View Query for the request.
        /// </summary>
        /// <param name="id">Unique id of the item to view.</param>
        /// <param name="claimsPrincipal">The claims principal of the user requesting the view.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>The view query.</returns>
        protected abstract Task<TViewQuery> GetViewQueryAsync(
            long id,
            ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken);
    }
}
