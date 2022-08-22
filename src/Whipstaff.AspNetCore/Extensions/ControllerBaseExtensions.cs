// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Whipstaff.Core.Mediatr;

namespace Whipstaff.AspNetCore.Extensions
{
    /// <summary>
    /// Extension methods for ASP.NET Core Controllers.
    /// </summary>
    public static class ControllerBaseExtensions
    {
        /// <summary>
        /// Extension method for common behaviour in Add API operations.
        /// </summary>
        /// <typeparam name="TAddCommandRequestDto">The type for the Request DTO for the Add Operation.</typeparam>
        /// <typeparam name="TAddCommandResponseDto">The type for the Response DTO for the Add Operation.</typeparam>
        /// <typeparam name="TAddCommand">The type for the CQRS Command for the Add Operation.</typeparam>
        /// <typeparam name="TAddApiResponseDto">The type for API response of the Add Operation.</typeparam>
        /// <param name="instance">Web Controller instance.</param>
        /// <param name="logger">Logger object.</param>
        /// <param name="mediator">Mediatr object for publishing commands to.</param>
        /// <param name="authorizationService">Authorization service.</param>
        /// <param name="addRequestDto">The Request DTO for the Add operation.</param>
        /// <param name="logAction">
        /// Log Message Action for the event to log against.
        ///
        /// You can create and cache a LoggerMessage action to pass in by calling <see cref="LoggerMessage.Define{T}(LogLevel, EventId, string)"/>.
        /// You can either have a single LoggerMessage definition for all controllers, or have one for each controller. The flexibility is there
        /// for you to choose.
        /// </param>
        /// <param name="addPolicyName">The policy name to use for Authorization verification.</param>
        /// <param name="getAddActionResultAsync">Task to format the result of CQRS operation into an IActionResult. Allows for controllers to make decisions on what views or data manipulation to carry out.</param>
        /// <param name="addCommandFactoryAsync">The Command Factory for the Add operation.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public static async Task<ActionResult<TAddApiResponseDto>> GetAddActionAsync<TAddCommandRequestDto, TAddCommandResponseDto, TAddCommand, TAddApiResponseDto>(
            this ControllerBase instance,
            ILogger logger,
            IMediator mediator,
            IAuthorizationService authorizationService,
            TAddCommandRequestDto addRequestDto,
            Action<ILogger, string, Exception?> logAction,
            string addPolicyName,
            Func<TAddCommandResponseDto, Task<ActionResult<TAddApiResponseDto>>> getAddActionResultAsync,
            Func<TAddCommandRequestDto, System.Security.Claims.ClaimsPrincipal, CancellationToken, Task<TAddCommand>> addCommandFactoryAsync,
            CancellationToken cancellationToken)
            where TAddCommand : IAuditableCommand<TAddCommandRequestDto, TAddCommandResponseDto?>
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (logAction == null)
            {
                throw new ArgumentNullException(nameof(logAction));
            }

            logAction(logger, "Started", null);

            if (mediator == null)
            {
                throw new ArgumentNullException(nameof(mediator));
            }

            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (authorizationService == null)
            {
                throw new ArgumentNullException(nameof(authorizationService));
            }

            if (addCommandFactoryAsync == null)
            {
                throw new ArgumentNullException(nameof(addCommandFactoryAsync));
            }

            if (getAddActionResultAsync == null)
            {
                throw new ArgumentNullException(nameof(getAddActionResultAsync));
            }

            if (!instance.Request.IsHttps)
            {
                logAction(logger, "Non HTTPS request", null);
                return instance.BadRequest();
            }

            var user = instance.HttpContext.User;

            // someone needs to have permission to do a general add
            // but there is a chance the request dto also has details such as a parent id
            // so while someone may have a generic add permission
            // they may not be able to add to a specific parent item
            var methodAuthorization = await authorizationService.AuthorizeAsync(
                user,
                addRequestDto,
                addPolicyName).ConfigureAwait(false);

            if (!methodAuthorization.Succeeded)
            {
                logAction(logger, "Method Authorization Failed", null);
                return instance.Forbid();
            }

            var query = await addCommandFactoryAsync(
                addRequestDto,
                user,
                cancellationToken).ConfigureAwait(false);

            var result = await mediator.Send(
                query,
                cancellationToken).ConfigureAwait(false);

            if (result == null)
            {
                return instance.NotFound();
            }

            var viewResult = await getAddActionResultAsync(result).ConfigureAwait(false);
            logAction(logger, "Finished", null);

            return viewResult;
        }

        /// <summary>
        /// Extension method for common behaviour in Delete API operations.
        /// </summary>
        /// <typeparam name="TDeleteCommandResponseDto">The type for the Response DTO for the Add Operation.</typeparam>
        /// <typeparam name="TDeleteCommand">The type for the CQRS Command for the Delete Operation.</typeparam>
        /// <typeparam name="TDeleteApiResponseDto">The type for API response of the Add Operation.</typeparam>
        /// <param name="instance">Web Controller instance.</param>
        /// <param name="logger">Logger object.</param>
        /// <param name="mediator">Mediatr object for publishing commands to.</param>
        /// <param name="authorizationService">Authorization service.</param>
        /// <param name="id">Unique Id for the identity being deleted.</param>
        /// <param name="logAction">
        /// Log Message Action for the event to log against.
        ///
        /// You can create and cache a LoggerMessage action to pass in by calling <see cref="LoggerMessage.Define{T}(LogLevel, EventId, string)"/>.
        /// You can either have a single LoggerMessage definition for all controllers, or have one for each controller. The flexibility is there
        /// for you to choose.
        /// </param>
        /// <param name="deletePolicyName">The policy name to use for Authorization verification.</param>
        /// <param name="getDeleteActionResultAsync">Task to format the result of CQRS operation into an IActionResult. Allows for controllers to make decisions on what views or data manipulation to carry out.</param>
        /// <param name="deleteCommandFactoryAsync">The Command Factory for the Delete operation.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public static async Task<ActionResult<TDeleteApiResponseDto>> GetDeleteActionAsync<TDeleteCommandResponseDto, TDeleteCommand, TDeleteApiResponseDto>(
            this ControllerBase instance,
            ILogger logger,
            IMediator mediator,
            IAuthorizationService authorizationService,
            long id,
            Action<ILogger, string, Exception?> logAction,
            string deletePolicyName,
            Func<TDeleteCommandResponseDto, Task<ActionResult<TDeleteApiResponseDto>>> getDeleteActionResultAsync,
            Func<long, System.Security.Claims.ClaimsPrincipal, CancellationToken, Task<TDeleteCommand>> deleteCommandFactoryAsync,
            CancellationToken cancellationToken)
            where TDeleteCommand : IAuditableCommand<long, TDeleteCommandResponseDto?>
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (logAction == null)
            {
                throw new ArgumentNullException(nameof(logAction));
            }

            logAction(logger, "Started", null);

            if (mediator == null)
            {
                throw new ArgumentNullException(nameof(mediator));
            }

            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (authorizationService == null)
            {
                throw new ArgumentNullException(nameof(authorizationService));
            }

            if (deleteCommandFactoryAsync == null)
            {
                throw new ArgumentNullException(nameof(deleteCommandFactoryAsync));
            }

            if (getDeleteActionResultAsync == null)
            {
                throw new ArgumentNullException(nameof(getDeleteActionResultAsync));
            }

            if (!instance.Request.IsHttps)
            {
                return instance.BadRequest();
            }

            if (id < 1)
            {
                return instance.NotFound();
            }

            var user = instance.HttpContext.User;

            var methodAuthorization = await authorizationService.AuthorizeAsync(
                user,
                deletePolicyName).ConfigureAwait(false);

            if (!methodAuthorization.Succeeded)
            {
                logAction(logger, "Resource Authorization Failed", null);
                return instance.Forbid();
            }

            var resourceAuthorization = await authorizationService.AuthorizeAsync(
                user,
                id,
                deletePolicyName).ConfigureAwait(false);

            if (!resourceAuthorization.Succeeded)
            {
                logAction(logger, "Resource Authorization Failed", null);
                return instance.Forbid();
            }

            var query = await deleteCommandFactoryAsync(
                id,
                user,
                cancellationToken).ConfigureAwait(false);

            var result = await mediator.Send(
                query,
                cancellationToken).ConfigureAwait(false);

            if (result == null)
            {
                return instance.NotFound();
            }

            var viewResult = await getDeleteActionResultAsync(result).ConfigureAwait(false);
            logAction(logger, "Finished", null);

            return viewResult;
        }

        /// <summary>
        /// Extension method for common behaviour in List API operations, this method doesn't constrain the requirement for an auditable request.
        /// However the auditable request does call this.
        /// </summary>
        /// <typeparam name="TListQueryRequestDto">The type for the Request DTO for the List Operation.</typeparam>
        /// <typeparam name="TListQueryResponseDto">The type for the Response DTO for the List Operation.</typeparam>
        /// <typeparam name="TListQuery">The type for the CQRS Command for the List Operation.</typeparam>
        /// <typeparam name="TListApiResponseDto">The type for API response of the Add Operation.</typeparam>
        /// <param name="instance">Web Controller instance.</param>
        /// <param name="logger">Logger object.</param>
        /// <param name="mediator">Mediatr object for publishing commands to.</param>
        /// <param name="authorizationService">Authorization service.</param>
        /// <param name="listRequestDto">The Request DTO for the List operation.</param>
        /// <param name="logAction">
        /// Log Message Action for the event to log against.
        ///
        /// You can create and cache a LoggerMessage action to pass in by calling <see cref="LoggerMessage.Define(LogLevel, EventId, string)"/>.
        /// You can either have a single LoggerMessage definition for all controllers, or have one for each controller. The flexibility is there
        /// for you to choose.
        /// </param>
        /// <param name="listPolicyName">The policy name to use for Authorization verification.</param>
        /// <param name="getListActionResultAsync">Task to format the result of CQRS operation into an IActionResult. Allows for controllers to make decisions on what views or data manipulation to carry out.</param>
        /// <param name="listQueryFactoryAsync">The Query Factory for the List operation.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public static async Task<ActionResult<TListApiResponseDto>> GetListActionFlexibleAsync<TListQueryRequestDto, TListQueryResponseDto, TListQuery, TListApiResponseDto>(
            this ControllerBase instance,
            ILogger logger,
            IMediator mediator,
            IAuthorizationService authorizationService,
            TListQueryRequestDto listRequestDto,
            Action<ILogger, string, Exception?> logAction,
            string listPolicyName,
            Func<TListQueryResponseDto, Task<ActionResult<TListApiResponseDto>>> getListActionResultAsync,
            Func<TListQueryRequestDto, System.Security.Claims.ClaimsPrincipal, CancellationToken, Task<TListQuery>> listQueryFactoryAsync,
            CancellationToken cancellationToken)
            where TListQuery : IQuery<TListQueryResponseDto?>
            where TListQueryResponseDto : class
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (logAction == null)
            {
                throw new ArgumentNullException(nameof(logAction));
            }

            if (mediator == null)
            {
                throw new ArgumentNullException(nameof(mediator));
            }

            logAction(logger, "Started", null);

            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (authorizationService == null)
            {
                throw new ArgumentNullException(nameof(authorizationService));
            }

            if (listQueryFactoryAsync == null)
            {
                throw new ArgumentNullException(nameof(listQueryFactoryAsync));
            }

            if (getListActionResultAsync == null)
            {
                throw new ArgumentNullException(nameof(getListActionResultAsync));
            }

            if (!instance.Request.IsHttps)
            {
                return instance.BadRequest();
            }

            var user = instance.HttpContext.User;

            var methodAuthorization = await authorizationService.AuthorizeAsync(
                user,
                listRequestDto,
                listPolicyName).ConfigureAwait(false);
            if (!methodAuthorization.Succeeded)
            {
                return instance.Forbid();
            }

            var query = await listQueryFactoryAsync(
                listRequestDto,
                user,
                cancellationToken).ConfigureAwait(false);

            var result = await mediator.Send(
                query,
                cancellationToken).ConfigureAwait(false);

            if (result == null)
            {
                return instance.NotFound();
            }

            var resourceAuthorization = await authorizationService.AuthorizeAsync(
                user,
                result,
                listPolicyName).ConfigureAwait(false);
            if (!resourceAuthorization.Succeeded)
            {
                // not found rather than forbidden
                return instance.NotFound();
            }

            var viewResult = await getListActionResultAsync(result).ConfigureAwait(false);
            logAction(logger, "Finished", null);

            return viewResult;
        }

        /// <summary>
        /// Extension method for common behaviour in List API operations.
        /// </summary>
        /// <typeparam name="TListQueryRequestDto">The type for the Request DTO for the List Operation.</typeparam>
        /// <typeparam name="TListQueryResponseDto">The type for the Response DTO for the List Operation.</typeparam>
        /// <typeparam name="TListQuery">The type for the CQRS Command for the List Operation.</typeparam>
        /// <typeparam name="TListApiResponseDto">The type for API response of the List Operation.</typeparam>
        /// <param name="instance">Web Controller instance.</param>
        /// <param name="logger">Logger object.</param>
        /// <param name="mediator">Mediatr object for publishing commands to.</param>
        /// <param name="authorizationService">Authorization service.</param>
        /// <param name="listRequestDto">The Request DTO for the List operation.</param>
        /// <param name="logAction">
        /// Log Message Action for the event to log against.
        ///
        /// You can create and cache a LoggerMessage action to pass in by calling <see cref="LoggerMessage.Define(LogLevel, EventId, string)"/>.
        /// You can either have a single LoggerMessage definition for all controllers, or have one for each controller. The flexibility is there
        /// for you to choose.
        /// </param>
        /// <param name="listPolicyName">The policy name to use for Authorization verification.</param>
        /// <param name="getListActionResultAsync">Task to format the result of CQRS operation into an IActionResult. Allows for controllers to make decisions on what views or data manipulation to carry out.</param>
        /// <param name="listCommandFactoryAsync">The Command Factory for the List operation.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public static Task<ActionResult<TListApiResponseDto>> GetListActionAsync<TListQueryRequestDto, TListQueryResponseDto, TListQuery, TListApiResponseDto>(
            this ControllerBase instance,
            ILogger logger,
            IMediator mediator,
            IAuthorizationService authorizationService,
            TListQueryRequestDto listRequestDto,
            Action<ILogger, string, Exception?> logAction,
            string listPolicyName,
            Func<TListQueryResponseDto, Task<ActionResult<TListApiResponseDto>>> getListActionResultAsync,
            Func<TListQueryRequestDto, System.Security.Claims.ClaimsPrincipal, CancellationToken, Task<TListQuery>> listCommandFactoryAsync,
            CancellationToken cancellationToken)
            where TListQuery : IAuditableQuery<TListQueryRequestDto, TListQueryResponseDto?>
            where TListQueryResponseDto : class
        {
            return GetListActionFlexibleAsync(
                instance,
                logger,
                mediator,
                authorizationService,
                listRequestDto,
                logAction,
                listPolicyName,
                getListActionResultAsync,
                listCommandFactoryAsync,
                cancellationToken);
        }

        /// <summary>
        /// Extension method for common behaviour in View API operations.
        /// </summary>
        /// <typeparam name="TViewQueryRequestDto">The type for the Query DTO for the View Operation.</typeparam>
        /// <typeparam name="TViewQueryResponseDto">The type for the Query Response DTO for the View Operation.</typeparam>
        /// <typeparam name="TViewQuery">The type for the CQRS Command for the View Operation.</typeparam>
        /// <typeparam name="TViewApiResponseDto">The type for API response of the View Operation.</typeparam>
        /// <param name="instance">Web Controller instance.</param>
        /// <param name="logger">Logger object.</param>
        /// <param name="mediator">Mediatr object for publishing commands to.</param>
        /// <param name="authorizationService">Authorization service.</param>
        /// <param name="viewRequestDto">The Request DTO for the View operation.</param>
        /// <param name="logAction">
        /// Log Message Action for the event to log against.
        ///
        /// You can create and cache a LoggerMessage action to pass in by calling <see cref="LoggerMessage.Define(LogLevel, EventId, string)"/>.
        /// You can either have a single LoggerMessage definition for all controllers, or have one for each controller. The flexibility is there
        /// for you to choose.
        /// </param>
        /// <param name="viewPolicyName">The policy name to use for Authorization verification.</param>
        /// <param name="getViewActionResultAsync">Task to format the result of CQRS operation into an IActionResult. Allows for controllers to make decisions on what views or data manipulation to carry out.</param>
        /// <param name="viewCommandFactoryAsync">The Command Factory for the View operation.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public static async Task<ActionResult<TViewApiResponseDto>> GetViewActionAsync<TViewQueryRequestDto, TViewQueryResponseDto, TViewQuery, TViewApiResponseDto>(
            this ControllerBase instance,
            ILogger logger,
            IMediator mediator,
            IAuthorizationService authorizationService,
            TViewQueryRequestDto viewRequestDto,
            Action<ILogger, string, Exception?> logAction,
            string viewPolicyName,
            Func<TViewQueryResponseDto, Task<ActionResult<TViewApiResponseDto>>> getViewActionResultAsync,
            Func<TViewQueryRequestDto, System.Security.Claims.ClaimsPrincipal, CancellationToken, Task<TViewQuery>> viewCommandFactoryAsync,
            CancellationToken cancellationToken)
            where TViewQuery : IAuditableQuery<TViewQueryRequestDto, TViewQueryResponseDto?>
            where TViewQueryResponseDto : class
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (logAction == null)
            {
                throw new ArgumentNullException(nameof(logAction));
            }

            logAction(logger, "Started", null);

            if (mediator == null)
            {
                throw new ArgumentNullException(nameof(mediator));
            }

            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (authorizationService == null)
            {
                throw new ArgumentNullException(nameof(authorizationService));
            }

            if (viewCommandFactoryAsync == null)
            {
                throw new ArgumentNullException(nameof(viewCommandFactoryAsync));
            }

            if (getViewActionResultAsync == null)
            {
                throw new ArgumentNullException(nameof(getViewActionResultAsync));
            }

            if (!instance.Request.IsHttps)
            {
                return instance.BadRequest();
            }

            var user = instance.HttpContext.User;

            var methodAuthorization = await authorizationService.AuthorizeAsync(
                user,
                viewRequestDto,
                viewPolicyName).ConfigureAwait(false);
            if (!methodAuthorization.Succeeded)
            {
                return instance.Forbid();
            }

            var query = await viewCommandFactoryAsync(
                viewRequestDto,
                user,
                cancellationToken).ConfigureAwait(false);

            var result = await mediator.Send(
                query,
                cancellationToken).ConfigureAwait(false);

            if (result == null)
            {
                return instance.NotFound();
            }

            var resourceAuthorization = await authorizationService.AuthorizeAsync(
                user,
                result,
                viewPolicyName).ConfigureAwait(false);
            if (!resourceAuthorization.Succeeded)
            {
                // not found rather than forbidden
                return instance.NotFound();
            }

            var viewResult = await getViewActionResultAsync(result).ConfigureAwait(false);
            logAction(logger, "View Finished", null);

            return viewResult;
        }

        /// <summary>
        /// Extension method for common behaviour in Update API operations.
        /// </summary>
        /// <typeparam name="TUpdateCommandRequestDto">The type for the Request DTO for the Update Operation.</typeparam>
        /// <typeparam name="TUpdateCommandResponseDto">The type for the Response DTO for the Update Operation.</typeparam>
        /// <typeparam name="TUpdateCommand">The type for the CQRS Command for the Update Operation.</typeparam>
        /// <typeparam name="TUpdateApiResponseDto">The type for API response of the View Operation.</typeparam>
        /// <param name="instance">Web Controller instance.</param>
        /// <param name="logger">Logger object.</param>
        /// <param name="mediator">Mediatr object for publishing commands to.</param>
        /// <param name="authorizationService">Authorization service.</param>
        /// <param name="id">The unique id of the entity to be updated.</param>
        /// <param name="updateRequestDto">The Request DTO for the Update operation.</param>
        /// <param name="logAction">
        /// Log Message Action for the event to log against.
        ///
        /// You can create and cache a LoggerMessage action to pass in by calling <see cref="LoggerMessage.Define{T}(LogLevel, EventId, string)"/>.
        /// You can either have a single LoggerMessage definition for all controllers, or have one for each controller. The flexibility is there
        /// for you to choose.
        /// </param>
        /// <param name="updatePolicyName">The policy name to use for Authorization verification.</param>
        /// <param name="getUpdateActionResultAsync">Task to format the result of CQRS operation into an IActionResult. Allows for controllers to make decisions on what views or data manipulation to carry out.</param>
        /// <param name="updateCommandFactoryAsync">The Command Factory for the Update operation.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public static async Task<ActionResult<TUpdateApiResponseDto>> GetUpdateActionAsync<TUpdateCommandRequestDto, TUpdateCommandResponseDto, TUpdateCommand, TUpdateApiResponseDto>(
            this ControllerBase instance,
            ILogger logger,
            IMediator mediator,
            IAuthorizationService authorizationService,
            long id,
            TUpdateCommandRequestDto updateRequestDto,
            Action<ILogger, string, Exception?> logAction,
            string updatePolicyName,
            Func<TUpdateCommandResponseDto, Task<ActionResult<TUpdateApiResponseDto>>> getUpdateActionResultAsync,
            Func<TUpdateCommandRequestDto, System.Security.Claims.ClaimsPrincipal, CancellationToken, Task<TUpdateCommand>> updateCommandFactoryAsync,
            CancellationToken cancellationToken)
            where TUpdateCommand : IAuditableCommand<TUpdateCommandRequestDto, TUpdateCommandResponseDto?>
            where TUpdateCommandResponseDto : class
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (logAction == null)
            {
                throw new ArgumentNullException(nameof(logAction));
            }

            logAction(logger, "Started UpdateAsync", null);

            if (mediator == null)
            {
                throw new ArgumentNullException(nameof(mediator));
            }

            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (authorizationService == null)
            {
                throw new ArgumentNullException(nameof(authorizationService));
            }

            if (updateCommandFactoryAsync == null)
            {
                throw new ArgumentNullException(nameof(updateCommandFactoryAsync));
            }

            if (getUpdateActionResultAsync == null)
            {
                throw new ArgumentNullException(nameof(getUpdateActionResultAsync));
            }

            if (!instance.Request.IsHttps)
            {
                return instance.BadRequest();
            }

            if (id < 1)
            {
                logAction(logger, "Id less than 1", null);
                return instance.NotFound();
            }

            var user = instance.HttpContext.User;

            var methodAuthorization = await authorizationService.AuthorizeAsync(
                user,
                updateRequestDto,
                updatePolicyName).ConfigureAwait(false);

            if (!methodAuthorization.Succeeded)
            {
                logAction(logger, "Method Authorization Failed", null);
                return instance.Forbid();
            }

            var command = await updateCommandFactoryAsync(
                updateRequestDto,
                user,
                cancellationToken).ConfigureAwait(false);

            var result = await mediator.Send(
                command,
                cancellationToken).ConfigureAwait(false);

            // the result will return null
            // if the requested object didn't exist
            // or the user doesn't have permission to update
            if (result == null)
            {
                logAction(logger, "No result from mediator", null);
                return instance.NotFound();
            }

            var viewResult = await getUpdateActionResultAsync(result).ConfigureAwait(false);
            logAction(logger, "Finished", null);

            return viewResult;
        }
    }
}
