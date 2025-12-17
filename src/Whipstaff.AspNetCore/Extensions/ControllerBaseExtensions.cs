// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Whipstaff.Mediator;

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
        /// <typeparam name="TAddRequestDto">The type for the Request DTO for the Add Operation.</typeparam>
        /// <typeparam name="TAddResponseDto">The type for the Response DTO for the Add Operation.</typeparam>
        /// <typeparam name="TAddCommand">The type for the CQRS Command for the Add Operation.</typeparam>
        /// <param name="instance">Web Controller instance.</param>
        /// <param name="logger">Logger object.</param>
        /// <param name="mediator">Mediator object for publishing commands to.</param>
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
        public static async Task<ActionResult<TAddResponseDto>> GetAddActionAsync<TAddRequestDto, TAddResponseDto, TAddCommand>(
            this ControllerBase instance,
            ILogger logger,
            IMediator mediator,
            IAuthorizationService authorizationService,
            TAddRequestDto addRequestDto,
            Action<ILogger, string, Exception?> logAction,
            string addPolicyName,
            Func<TAddResponseDto, Task<ActionResult<TAddResponseDto>>> getAddActionResultAsync,
            Func<TAddRequestDto, System.Security.Claims.ClaimsPrincipal, CancellationToken, Task<TAddCommand>> addCommandFactoryAsync,
            CancellationToken cancellationToken)
            where TAddCommand : IAuditableCommand<TAddRequestDto, TAddResponseDto?>
        {
            ArgumentNullException.ThrowIfNull(instance);
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(logAction);
            ArgumentNullException.ThrowIfNull(mediator);
            ArgumentNullException.ThrowIfNull(authorizationService);
            ArgumentNullException.ThrowIfNull(addCommandFactoryAsync);
            ArgumentNullException.ThrowIfNull(getAddActionResultAsync);

            logAction(logger, "Started", null);

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

            if (!instance.ModelState.IsValid)
            {
                logAction(logger, "Model State Invalid", null);
                return instance.BadRequest(instance.ModelState);
            }

            var query = await addCommandFactoryAsync(
                addRequestDto,
                user,
                cancellationToken).ConfigureAwait(false);

            var result = await mediator.Send(
                query,
                cancellationToken).ConfigureAwait(false);

#pragma warning disable S2955 // Generic parameters not constrained to reference types should not be compared to "null"
            if (result == null)
            {
                return instance.NotFound();
            }
#pragma warning restore S2955 // Generic parameters not constrained to reference types should not be compared to "null"

            var viewResult = await getAddActionResultAsync(result).ConfigureAwait(false);
            logAction(logger, "Finished", null);

            return viewResult;
        }

        /// <summary>
        /// Extension method for common behaviour in Delete API operations.
        /// </summary>
        /// <typeparam name="TDeleteResponseDto">The type for the Response DTO for the Add Operation.</typeparam>
        /// <typeparam name="TDeleteCommand">The type for the CQRS Command for the Delete Operation.</typeparam>
        /// <param name="instance">Web Controller instance.</param>
        /// <param name="logger">Logger object.</param>
        /// <param name="mediator">Mediator object for publishing commands to.</param>
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
        public static async Task<ActionResult<TDeleteResponseDto>> GetDeleteActionAsync<TDeleteResponseDto, TDeleteCommand>(
            this ControllerBase instance,
            ILogger logger,
            IMediator mediator,
            IAuthorizationService authorizationService,
            long id,
            Action<ILogger, string, Exception?> logAction,
            string deletePolicyName,
            Func<TDeleteResponseDto, Task<ActionResult<TDeleteResponseDto>>> getDeleteActionResultAsync,
            Func<long, System.Security.Claims.ClaimsPrincipal, CancellationToken, Task<TDeleteCommand>> deleteCommandFactoryAsync,
            CancellationToken cancellationToken)
            where TDeleteCommand : IAuditableCommand<long, TDeleteResponseDto?>
        {
            ArgumentNullException.ThrowIfNull(instance);
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(logAction);
            ArgumentNullException.ThrowIfNull(mediator);
            ArgumentNullException.ThrowIfNull(authorizationService);
            ArgumentNullException.ThrowIfNull(deleteCommandFactoryAsync);
            ArgumentNullException.ThrowIfNull(getDeleteActionResultAsync);

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

#pragma warning disable S2955 // Generic parameters not constrained to reference types should not be compared to "null"
            if (result == null)
            {
                return instance.NotFound();
            }
#pragma warning restore S2955 // Generic parameters not constrained to reference types should not be compared to "null"

            var viewResult = await getDeleteActionResultAsync(result).ConfigureAwait(false);
            logAction(logger, "Finished", null);

            return viewResult;
        }

        /// <summary>
        /// Extension method for common behaviour in List API operations, this method doesn't constrain the requirement for an auditable request.
        /// However the auditable request does call this.
        /// </summary>
        /// <typeparam name="TListRequestDto">The type for the Request DTO for the List Operation.</typeparam>
        /// <typeparam name="TListResponseDto">The type for the Response DTO for the List Operation.</typeparam>
        /// <typeparam name="TListQuery">The type for the CQRS Command for the List Operation.</typeparam>
        /// <param name="instance">Web Controller instance.</param>
        /// <param name="logger">Logger object.</param>
        /// <param name="mediator">Mediator object for publishing commands to.</param>
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
        public static Task<ActionResult<TListResponseDto>> GetApiListActionFlexibleAsync<TListRequestDto, TListResponseDto, TListQuery>(
            this ControllerBase instance,
            ILogger logger,
            IMediator mediator,
            IAuthorizationService authorizationService,
            TListRequestDto listRequestDto,
            Action<ILogger, string, Exception?> logAction,
            string listPolicyName,
            Func<TListResponseDto, Task<ActionResult<TListResponseDto>>> getListActionResultAsync,
            Func<TListRequestDto, System.Security.Claims.ClaimsPrincipal, CancellationToken, Task<TListQuery>> listCommandFactoryAsync,
            CancellationToken cancellationToken)
            where TListQuery : IAuditableQuery<TListRequestDto, TListResponseDto?>
            where TListResponseDto : class
        {
            ArgumentNullException.ThrowIfNull(instance);
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(logAction);
            ArgumentNullException.ThrowIfNull(mediator);
            ArgumentNullException.ThrowIfNull(authorizationService);
            ArgumentNullException.ThrowIfNull(getListActionResultAsync);
            ArgumentNullException.ThrowIfNull(listCommandFactoryAsync);

            return InternalGetListActionFlexibleAsync(
                instance,
                logger,
                mediator,
                authorizationService,
                listRequestDto,
                logAction,
                listPolicyName,
                getListActionResultAsync,
                listCommandFactoryAsync,
                i => i.Forbid(),
                i => i.NotFound(),
                cancellationToken);
        }

        /// <summary>
        /// Extension method for common behaviour in List API operations, this method doesn't constrain the requirement for an auditable request.
        /// However the auditable request does call this.
        /// </summary>
        /// <typeparam name="TListRequestDto">The type for the Request DTO for the List Operation.</typeparam>
        /// <typeparam name="TListResponseDto">The type for the Response DTO for the List Operation.</typeparam>
        /// <typeparam name="TListQuery">The type for the CQRS Command for the List Operation.</typeparam>
        /// <param name="instance">Web Controller instance.</param>
        /// <param name="logger">Logger object.</param>
        /// <param name="mediator">Mediator object for publishing commands to.</param>
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
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public static Task<IActionResult> GetListActionFlexibleAsync<TListRequestDto, TListResponseDto, TListQuery>(
            this Controller instance,
            ILogger logger,
            IMediator mediator,
            IAuthorizationService authorizationService,
            TListRequestDto listRequestDto,
            Action<ILogger, string, Exception?> logAction,
            string listPolicyName,
            Func<TListResponseDto, Task<IActionResult>> getListActionResultAsync,
            Func<TListRequestDto, System.Security.Claims.ClaimsPrincipal, CancellationToken, Task<TListQuery>> listCommandFactoryAsync,
            CancellationToken cancellationToken)
            where TListQuery : IAuditableQuery<TListRequestDto, TListResponseDto?>
            where TListResponseDto : class
        {
            ArgumentNullException.ThrowIfNull(instance);
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(logAction);
            ArgumentNullException.ThrowIfNull(mediator);
            ArgumentNullException.ThrowIfNull(authorizationService);
            ArgumentNullException.ThrowIfNull(getListActionResultAsync);
            ArgumentNullException.ThrowIfNull(listCommandFactoryAsync);

            return InternalGetListActionFlexibleAsync(
                instance,
                logger,
                mediator,
                authorizationService,
                listRequestDto,
                logAction,
                listPolicyName,
                getListActionResultAsync,
                listCommandFactoryAsync,
                i => i.Forbid(),
                i => i.NotFound(),
                cancellationToken);
        }

        /// <summary>
        /// Extension method for common behaviour in List API operations.
        /// </summary>
        /// <typeparam name="TListRequestDto">The type for the Request DTO for the List Operation.</typeparam>
        /// <typeparam name="TListResponseDto">The type for the Response DTO for the List Operation.</typeparam>
        /// <typeparam name="TListQuery">The type for the CQRS Command for the List Operation.</typeparam>
        /// <param name="instance">Web Controller instance.</param>
        /// <param name="logger">Logger object.</param>
        /// <param name="mediator">Mediator object for publishing commands to.</param>
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
        public static Task<ActionResult<TListResponseDto>> GetApiListActionAsync<TListRequestDto, TListResponseDto, TListQuery>(
            this ControllerBase instance,
            ILogger logger,
            IMediator mediator,
            IAuthorizationService authorizationService,
            TListRequestDto listRequestDto,
            Action<ILogger, string, Exception?> logAction,
            string listPolicyName,
            Func<TListResponseDto, Task<ActionResult<TListResponseDto>>> getListActionResultAsync,
            Func<TListRequestDto, System.Security.Claims.ClaimsPrincipal, CancellationToken, Task<TListQuery>> listCommandFactoryAsync,
            CancellationToken cancellationToken)
            where TListQuery : IAuditableQuery<TListRequestDto, TListResponseDto?>
            where TListResponseDto : class
        {
            return GetApiListActionFlexibleAsync(
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
        /// Extension method for common behaviour in List API operations.
        /// </summary>
        /// <typeparam name="TListRequestDto">The type for the Request DTO for the List Operation.</typeparam>
        /// <typeparam name="TListResponseDto">The type for the Response DTO for the List Operation.</typeparam>
        /// <typeparam name="TListQuery">The type for the CQRS Command for the List Operation.</typeparam>
        /// <param name="instance">Web Controller instance.</param>
        /// <param name="logger">Logger object.</param>
        /// <param name="mediator">Mediator object for publishing commands to.</param>
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
        public static Task<IActionResult> GetListActionAsync<TListRequestDto, TListResponseDto, TListQuery>(
            this Controller instance,
            ILogger logger,
            IMediator mediator,
            IAuthorizationService authorizationService,
            TListRequestDto listRequestDto,
            Action<ILogger, string, Exception?> logAction,
            string listPolicyName,
            Func<TListResponseDto, Task<IActionResult>> getListActionResultAsync,
            Func<TListRequestDto, System.Security.Claims.ClaimsPrincipal, CancellationToken, Task<TListQuery>> listCommandFactoryAsync,
            CancellationToken cancellationToken)
            where TListQuery : IAuditableQuery<TListRequestDto, TListResponseDto?>
            where TListResponseDto : class
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
        /// <typeparam name="TViewRequestDto">The type for the Request DTO for the View Operation.</typeparam>
        /// <typeparam name="TViewResponseDto">The type for the Response DTO for the View Operation.</typeparam>
        /// <typeparam name="TViewQuery">The type for the CQRS Command for the View Operation.</typeparam>
        /// <param name="instance">Web Controller instance.</param>
        /// <param name="logger">Logger object.</param>
        /// <param name="mediator">Mediator object for publishing commands to.</param>
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
        public static Task<ActionResult<TViewResponseDto>> GetApiViewActionAsync<TViewRequestDto, TViewResponseDto, TViewQuery>(
            this ControllerBase instance,
            ILogger logger,
            IMediator mediator,
            IAuthorizationService authorizationService,
            TViewRequestDto viewRequestDto,
            Action<ILogger, string, Exception?> logAction,
            string viewPolicyName,
            Func<TViewResponseDto, Task<ActionResult<TViewResponseDto>>> getViewActionResultAsync,
            Func<TViewRequestDto, System.Security.Claims.ClaimsPrincipal, CancellationToken, Task<TViewQuery>> viewCommandFactoryAsync,
            CancellationToken cancellationToken)
            where TViewQuery : IAuditableQuery<TViewRequestDto, TViewResponseDto?>
            where TViewResponseDto : class
        {
            ArgumentNullException.ThrowIfNull(instance);
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(logAction);
            ArgumentNullException.ThrowIfNull(mediator);
            ArgumentNullException.ThrowIfNull(authorizationService);
            ArgumentNullException.ThrowIfNull(getViewActionResultAsync);
            ArgumentNullException.ThrowIfNull(viewCommandFactoryAsync);

            return InternalGetViewActionAsync(
                instance,
                logger,
                mediator,
                authorizationService,
                viewRequestDto,
                logAction,
                viewPolicyName,
                getViewActionResultAsync,
                viewCommandFactoryAsync,
                i => i.Forbid(),
                i => i.NotFound(),
                cancellationToken);
        }

        /// <summary>
        /// Extension method for common behaviour in View API operations.
        /// </summary>
        /// <typeparam name="TViewRequestDto">The type for the Request DTO for the View Operation.</typeparam>
        /// <typeparam name="TViewResponseDto">The type for the Response DTO for the View Operation.</typeparam>
        /// <typeparam name="TViewQuery">The type for the CQRS Command for the View Operation.</typeparam>
        /// <param name="instance">Web Controller instance.</param>
        /// <param name="logger">Logger object.</param>
        /// <param name="mediator">Mediator object for publishing commands to.</param>
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
        public static Task<IActionResult> GetViewActionAsync<TViewRequestDto, TViewResponseDto, TViewQuery>(
            this ControllerBase instance,
            ILogger logger,
            IMediator mediator,
            IAuthorizationService authorizationService,
            TViewRequestDto viewRequestDto,
            Action<ILogger, string, Exception?> logAction,
            string viewPolicyName,
            Func<TViewResponseDto, Task<IActionResult>> getViewActionResultAsync,
            Func<TViewRequestDto, System.Security.Claims.ClaimsPrincipal, CancellationToken, Task<TViewQuery>> viewCommandFactoryAsync,
            CancellationToken cancellationToken)
            where TViewQuery : IAuditableQuery<TViewRequestDto, TViewResponseDto?>
            where TViewResponseDto : class
        {
            ArgumentNullException.ThrowIfNull(instance);
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(logAction);
            ArgumentNullException.ThrowIfNull(mediator);
            ArgumentNullException.ThrowIfNull(authorizationService);
            ArgumentNullException.ThrowIfNull(getViewActionResultAsync);
            ArgumentNullException.ThrowIfNull(viewCommandFactoryAsync);

            return InternalGetViewActionAsync(
                instance,
                logger,
                mediator,
                authorizationService,
                viewRequestDto,
                logAction,
                viewPolicyName,
                getViewActionResultAsync,
                viewCommandFactoryAsync,
                i => i.Forbid(),
                i => i.NotFound(),
                cancellationToken);
        }

        /// <summary>
        /// Extension method for common behaviour in Update API operations.
        /// </summary>
        /// <typeparam name="TUpdateRequestDto">The type for the Request DTO for the Update Operation.</typeparam>
        /// <typeparam name="TUpdateResponseDto">The type for the Response DTO for the Update Operation.</typeparam>
        /// <typeparam name="TUpdateCommand">The type for the CQRS Command for the Update Operation.</typeparam>
        /// <param name="instance">Web Controller instance.</param>
        /// <param name="logger">Logger object.</param>
        /// <param name="mediator">Mediator object for publishing commands to.</param>
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
        public static async Task<ActionResult<TUpdateResponseDto>> GetUpdateActionAsync<TUpdateRequestDto, TUpdateResponseDto, TUpdateCommand>(
            this ControllerBase instance,
            ILogger logger,
            IMediator mediator,
            IAuthorizationService authorizationService,
            long id,
            TUpdateRequestDto updateRequestDto,
            Action<ILogger, string, Exception?> logAction,
            string updatePolicyName,
            Func<TUpdateResponseDto, Task<ActionResult<TUpdateResponseDto>>> getUpdateActionResultAsync,
            Func<TUpdateRequestDto, System.Security.Claims.ClaimsPrincipal, CancellationToken, Task<TUpdateCommand>> updateCommandFactoryAsync,
            CancellationToken cancellationToken)
            where TUpdateCommand : IAuditableCommand<TUpdateRequestDto, TUpdateResponseDto?>
            where TUpdateResponseDto : class?
        {
            ArgumentNullException.ThrowIfNull(instance);
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(logAction);
            ArgumentNullException.ThrowIfNull(mediator);
            ArgumentNullException.ThrowIfNull(authorizationService);
            ArgumentNullException.ThrowIfNull(updateCommandFactoryAsync);
            ArgumentNullException.ThrowIfNull(getUpdateActionResultAsync);

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

            if (!instance.ModelState.IsValid)
            {
                logAction(logger, "Model State Invalid", null);
                return instance.BadRequest(instance.ModelState);
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

        private static Task<TResult> InternalGetListActionFlexibleAsync<TListRequestDto, TListResponseDto, TListQuery, TResult>(
            this ControllerBase instance,
            ILogger logger,
            IMediator mediator,
            IAuthorizationService authorizationService,
            TListRequestDto listRequestDto,
            Action<ILogger, string, Exception?> logAction,
            string listPolicyName,
            Func<TListResponseDto, Task<TResult>> getListActionResultAsync,
            Func<TListRequestDto, System.Security.Claims.ClaimsPrincipal, CancellationToken, Task<TListQuery>> listCommandFactoryAsync,
            Func<ControllerBase, TResult> forbidResultFunc,
            Func<ControllerBase, TResult> notFoundResultFunc,
            CancellationToken cancellationToken)
            where TListQuery : IAuditableQuery<TListRequestDto, TListResponseDto?>
            where TListResponseDto : class
        {
            return InternalGetActionAsync(
                instance,
                logger,
                mediator,
                authorizationService,
                listRequestDto,
                logAction,
                listPolicyName,
                getListActionResultAsync,
                listCommandFactoryAsync,
                i => forbidResultFunc(i),
                i => notFoundResultFunc(i),
                cancellationToken);
        }

        private static Task<TResult> InternalGetViewActionAsync<TViewRequestDto, TViewResponseDto, TViewQuery, TResult>(
            this ControllerBase instance,
            ILogger logger,
            IMediator mediator,
            IAuthorizationService authorizationService,
            TViewRequestDto viewRequestDto,
            Action<ILogger, string, Exception?> logAction,
            string viewPolicyName,
            Func<TViewResponseDto, Task<TResult>> getViewActionResultAsync,
            Func<TViewRequestDto, System.Security.Claims.ClaimsPrincipal, CancellationToken, Task<TViewQuery>> viewCommandFactoryAsync,
            Func<ControllerBase, TResult> forbidResultFunc,
            Func<ControllerBase, TResult> notFoundResultFunc,
            CancellationToken cancellationToken)
            where TViewQuery : IAuditableQuery<TViewRequestDto, TViewResponseDto?>
            where TViewResponseDto : class
        {
            return InternalGetActionAsync(
                instance,
                logger,
                mediator,
                authorizationService,
                viewRequestDto,
                logAction,
                viewPolicyName,
                getViewActionResultAsync,
                viewCommandFactoryAsync,
                i => forbidResultFunc(i),
                i => notFoundResultFunc(i),
                cancellationToken);
        }

        private static async Task<TResult> InternalGetActionAsync<TViewRequestDto, TViewResponseDto, TViewQuery, TResult>(
            this ControllerBase instance,
            ILogger logger,
            IMediator mediator,
            IAuthorizationService authorizationService,
            TViewRequestDto viewRequestDto,
            Action<ILogger, string, Exception?> logAction,
            string viewPolicyName,
            Func<TViewResponseDto, Task<TResult>> getViewActionResultAsync,
            Func<TViewRequestDto, System.Security.Claims.ClaimsPrincipal, CancellationToken, Task<TViewQuery>> viewCommandFactoryAsync,
            Func<ControllerBase, TResult> forbidResultFunc,
            Func<ControllerBase, TResult> notFoundResultFunc,
            CancellationToken cancellationToken)
            where TViewQuery : IAuditableQuery<TViewRequestDto, TViewResponseDto?>
            where TViewResponseDto : class
        {
            ArgumentNullException.ThrowIfNull(instance);
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(logAction);
            ArgumentNullException.ThrowIfNull(mediator);
            ArgumentNullException.ThrowIfNull(authorizationService);
            ArgumentNullException.ThrowIfNull(getViewActionResultAsync);
            ArgumentNullException.ThrowIfNull(viewCommandFactoryAsync);

            logAction(logger, "Started", null);

            var user = instance.HttpContext.User;

            var methodAuthorization = await authorizationService.AuthorizeAsync(
                user,
                viewRequestDto,
                viewPolicyName).ConfigureAwait(false);
            if (!methodAuthorization.Succeeded)
            {
                return forbidResultFunc(instance);
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
                return notFoundResultFunc(instance);
            }

            var resourceAuthorization = await authorizationService.AuthorizeAsync(
                user,
                result,
                viewPolicyName).ConfigureAwait(false);
            if (!resourceAuthorization.Succeeded)
            {
                // not found rather than forbidden
                return notFoundResultFunc(instance);
            }

            var viewResult = await getViewActionResultAsync(result).ConfigureAwait(false);
            logAction(logger, "View Finished", null);

            return viewResult;
        }
    }
}
