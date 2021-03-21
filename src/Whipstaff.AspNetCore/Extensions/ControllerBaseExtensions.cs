// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Whipstaff.Core;
using Whipstaff.Core.Logging;

namespace Dhgms.AspNetCoreContrib.Controllers.Extensions
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
        /// <param name="mediator">Mediatr object for publishing commands to.</param>
        /// <param name="authorizationService">Authorization service.</param>
        /// <param name="addRequestDto">The Request DTO for the Add operation.</param>
        /// <param name="eventId">The unique event id for logging, application performance management feature usage tracking, etc.</param>
        /// <param name="addPolicyName">The policy name to use for Authorization verification.</param>
        /// <param name="getAddActionResultAsync">Task to format the result of CQRS operation into an IActionResult. Allows for controllers to make decisions on what views or data manipulation to carry out.</param>
        /// <param name="addCommandFactoryAsync">The Command Factory for the Add operation.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public static async Task<IActionResult> GetAddActionAsync<TAddRequestDto, TAddResponseDto, TAddCommand>(
            [NotNull]this ControllerBase instance,
            [NotNull]ILogger logger,
            [NotNull]IMediator mediator,
            [NotNull]IAuthorizationService authorizationService,
            [NotNull]TAddRequestDto addRequestDto,
            EventId eventId,
            [NotNull]string addPolicyName,
            [NotNull]Func<TAddResponseDto, Task<IActionResult>> getAddActionResultAsync,
            [NotNull]Func<TAddRequestDto, System.Security.Claims.ClaimsPrincipal, CancellationToken, Task<TAddCommand>> addCommandFactoryAsync,
            CancellationToken cancellationToken)
            where TAddCommand : IAuditableRequest<TAddRequestDto, TAddResponseDto>
        {
            logger.TraceMethodEntry();

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
                return instance.Forbid();
            }

            var query = await addCommandFactoryAsync(
                addRequestDto,
                user,
                cancellationToken).ConfigureAwait(false);

            var result = await mediator.Send(
                query,
                cancellationToken).ConfigureAwait(false);

            var viewResult = await getAddActionResultAsync(result).ConfigureAwait(false);
            logger.LogDebug(
                eventId,
                "Finished AddAsync");

            return viewResult;
        }

        /// <summary>
        /// Extension method for common behaviour in Delete API operations.
        /// </summary>
        /// <typeparam name="TDeleteResponseDto">The type for the Response DTO for the Add Operation.</typeparam>
        /// <typeparam name="TDeleteCommand">The type for the CQRS Command for the Delete Operation.</typeparam>
        /// <param name="instance">Web Controller instance.</param>
        /// <param name="logger">Logger object.</param>
        /// <param name="mediator">Mediatr object for publishing commands to.</param>
        /// <param name="authorizationService">Authorization service.</param>
        /// <param name="id">Unique Id for the identity being deleted.</param>
        /// <param name="eventId">The unique event id for logging, application performance management feature usage tracking, etc.</param>
        /// <param name="deletePolicyName">The policy name to use for Authorization verification.</param>
        /// <param name="getDeleteActionResultAsync">Task to format the result of CQRS operation into an IActionResult. Allows for controllers to make decisions on what views or data manipulation to carry out.</param>
        /// <param name="deleteCommandFactoryAsync">The Command Factory for the Delete operation.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public static async Task<IActionResult> GetDeleteActionAsync<TDeleteResponseDto, TDeleteCommand>(
            this ControllerBase instance,
            [NotNull] ILogger logger,
            [NotNull] IMediator mediator,
            [NotNull] IAuthorizationService authorizationService,
            long id,
            EventId eventId,
            [NotNull] string deletePolicyName,
            [NotNull] Func<TDeleteResponseDto, Task<IActionResult>> getDeleteActionResultAsync,
            [NotNull] Func<long, System.Security.Claims.ClaimsPrincipal, CancellationToken, Task<TDeleteCommand>> deleteCommandFactoryAsync,
            CancellationToken cancellationToken)
            where TDeleteCommand : IAuditableRequest<long, TDeleteResponseDto>
        {
            logger.LogDebug(
                eventId,
                "Entered DeleteAsync");

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
                id,
                deletePolicyName).ConfigureAwait(false);
            if (!methodAuthorization.Succeeded)
            {
                return instance.Forbid();
            }

            var query = await deleteCommandFactoryAsync(
                id,
                user,
                cancellationToken).ConfigureAwait(false);

            var result = await mediator.Send(
                query,
                cancellationToken).ConfigureAwait(false);

            var viewResult = await getDeleteActionResultAsync(result).ConfigureAwait(false);
            logger.LogDebug(
                eventId,
                "Finished DeleteAsync");

            return viewResult;
        }

        /// <summary>
        /// Extension method for common behaviour in List API operations, this method doesn't constrain the requirement for an auditable request.
        /// However the auditable request calls this. ;)
        /// </summary>
        /// <typeparam name="TListRequestDto">The type for the Request DTO for the List Operation.</typeparam>
        /// <typeparam name="TListResponseDto">The type for the Response DTO for the List Operation.</typeparam>
        /// <typeparam name="TListQuery">The type for the CQRS Command for the List Operation.</typeparam>
        /// <param name="instance">Web Controller instance.</param>
        /// <param name="logger">Logger object.</param>
        /// <param name="mediator">Mediatr object for publishing commands to.</param>
        /// <param name="authorizationService">Authorization service.</param>
        /// <param name="listRequestDto">The Request DTO for the List operation.</param>
        /// <param name="eventId">The unique event id for logging, application performance management feature usage tracking, etc.</param>
        /// <param name="listPolicyName">The policy name to use for Authorization verification.</param>
        /// <param name="getListActionResultAsync">Task to format the result of CQRS operation into an IActionResult. Allows for controllers to make decisions on what views or data manipulation to carry out.</param>
        /// <param name="listCommandFactoryAsync">The Command Factory for the List operation.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public static async Task<IActionResult> GetListActionFlexibleAsync<TListRequestDto, TListResponseDto, TListQuery>(
            [NotNull] this ControllerBase instance,
            [NotNull] ILogger logger,
            [NotNull] IMediator mediator,
            [NotNull] IAuthorizationService authorizationService,
            [NotNull] TListRequestDto listRequestDto,
            EventId eventId,
            [NotNull] string listPolicyName,
            [NotNull] Func<TListResponseDto, Task<IActionResult>> getListActionResultAsync,
            [NotNull] Func<TListRequestDto, System.Security.Claims.ClaimsPrincipal, CancellationToken, Task<TListQuery>> listCommandFactoryAsync,
            CancellationToken cancellationToken)
            where TListQuery : IRequest<TListResponseDto>
            where TListResponseDto : class
        {
            logger.LogDebug(eventId, "Entered ListAsync");

            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (authorizationService == null)
            {
                throw new ArgumentNullException(nameof(authorizationService));
            }

            if (listCommandFactoryAsync == null)
            {
                throw new ArgumentNullException(nameof(listCommandFactoryAsync));
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

            var query = await listCommandFactoryAsync(
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
            logger.LogDebug(
                eventId,
                "Finished ListAsync");

            return viewResult;
        }

        /// <summary>
        /// Extension method for common behaviour in List API operations.
        /// </summary>
        /// <typeparam name="TListRequestDto">The type for the Request DTO for the List Operation.</typeparam>
        /// <typeparam name="TListResponseDto">The type for the Response DTO for the List Operation.</typeparam>
        /// <typeparam name="TListQuery">The type for the CQRS Command for the List Operation.</typeparam>
        /// <param name="instance">Web Controller instance.</param>
        /// <param name="logger">Logger object.</param>
        /// <param name="mediator">Mediatr object for publishing commands to.</param>
        /// <param name="authorizationService">Authorization service.</param>
        /// <param name="listRequestDto">The Request DTO for the List operation.</param>
        /// <param name="eventId">The unique event id for logging, application performance management feature usage tracking, etc.</param>
        /// <param name="listPolicyName">The policy name to use for Authorization verification.</param>
        /// <param name="getListActionResultAsync">Task to format the result of CQRS operation into an IActionResult. Allows for controllers to make decisions on what views or data manipulation to carry out.</param>
        /// <param name="listCommandFactoryAsync">The Command Factory for the List operation.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public static Task<IActionResult> GetListActionAsync<TListRequestDto, TListResponseDto, TListQuery>(
            [NotNull] this ControllerBase instance,
            [NotNull] ILogger logger,
            [NotNull] IMediator mediator,
            [NotNull] IAuthorizationService authorizationService,
            [NotNull] TListRequestDto listRequestDto,
            EventId eventId,
            [NotNull] string listPolicyName,
            [NotNull] Func<TListResponseDto, Task<IActionResult>> getListActionResultAsync,
            [NotNull] Func<TListRequestDto, System.Security.Claims.ClaimsPrincipal, CancellationToken, Task<TListQuery>> listCommandFactoryAsync,
            CancellationToken cancellationToken)
            where TListQuery : IAuditableRequest<TListRequestDto, TListResponseDto>
            where TListResponseDto : class
        {
            return GetListActionFlexibleAsync(
                instance,
                logger,
                mediator,
                authorizationService,
                listRequestDto,
                eventId,
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
        /// <param name="mediator">Mediatr object for publishing commands to.</param>
        /// <param name="authorizationService">Authorization service.</param>
        /// <param name="viewRequestDto">The Request DTO for the View operation.</param>
        /// <param name="eventId">The unique event id for logging, application performance management feature usage tracking, etc.</param>
        /// <param name="viewPolicyName">The policy name to use for Authorization verification.</param>
        /// <param name="getViewActionResultAsync">Task to format the result of CQRS operation into an IActionResult. Allows for controllers to make decisions on what views or data manipulation to carry out.</param>
        /// <param name="viewCommandFactoryAsync">The Command Factory for the View operation.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public static async Task<IActionResult> GetViewActionAsync<TViewRequestDto, TViewResponseDto, TViewQuery>(
            [NotNull] this ControllerBase instance,
            [NotNull] ILogger logger,
            [NotNull] IMediator mediator,
            [NotNull] IAuthorizationService authorizationService,
            [NotNull] TViewRequestDto viewRequestDto,
            EventId eventId,
            [NotNull] string viewPolicyName,
            [NotNull] Func<TViewResponseDto, Task<IActionResult>> getViewActionResultAsync,
            [NotNull] Func<TViewRequestDto, System.Security.Claims.ClaimsPrincipal, CancellationToken, Task<TViewQuery>> viewCommandFactoryAsync,
            CancellationToken cancellationToken)
            where TViewQuery : IAuditableRequest<TViewRequestDto, TViewResponseDto>
            where TViewResponseDto : class
        {
            logger.LogDebug(eventId, "Entered ViewAsync");

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
            logger.LogDebug(
                eventId,
                "Finished ViewAsync");

            return viewResult;
        }

        /// <summary>
        /// Extension method for common behaviour in Update API operations.
        /// </summary>
        /// <typeparam name="TUpdateRequestDto">The type for the Request DTO for the Update Operation.</typeparam>
        /// <typeparam name="TUpdateResponseDto">The type for the Response DTO for the Update Operation.</typeparam>
        /// <typeparam name="TUpdateCommand">The type for the CQRS Command for the Update Operation.</typeparam>
        /// <param name="instance">Web Controller instance.</param>
        /// <param name="logger">Logger object.</param>
        /// <param name="mediator">Mediatr object for publishing commands to.</param>
        /// <param name="authorizationService">Authorization service.</param>
        /// <param name="id">The unique id of the entity to be updated.</param>
        /// <param name="updateRequestDto">The Request DTO for the Update operation.</param>
        /// <param name="eventId">The unique event id for logging, application performance management feature usage tracking, etc.</param>
        /// <param name="updatePolicyName">The policy name to use for Authorization verification.</param>
        /// <param name="getUpdateActionResultAsync">Task to format the result of CQRS operation into an IActionResult. Allows for controllers to make decisions on what views or data manipulation to carry out.</param>
        /// <param name="updateCommandFactoryAsync">The Command Factory for the Update operation.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public static async Task<IActionResult> GetUpdateActionAsync<TUpdateRequestDto, TUpdateResponseDto, TUpdateCommand>(
            [NotNull] this ControllerBase instance,
            [NotNull] ILogger logger,
            [NotNull] IMediator mediator,
            [NotNull] IAuthorizationService authorizationService,
            long id,
            [NotNull] TUpdateRequestDto updateRequestDto,
            EventId eventId,
            [NotNull] string updatePolicyName,
            [NotNull] Func<TUpdateResponseDto, Task<IActionResult>> getUpdateActionResultAsync,
            [NotNull]
            Func<TUpdateRequestDto, System.Security.Claims.ClaimsPrincipal, CancellationToken, Task<TUpdateCommand>>
                updateCommandFactoryAsync,
            CancellationToken cancellationToken)
            where TUpdateCommand : IAuditableRequest<TUpdateRequestDto, TUpdateResponseDto>
            where TUpdateResponseDto : class
        {
            logger.LogDebug(eventId, "Entered UpdateAsync");

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
                return instance.NotFound();
            }

            var user = instance.HttpContext.User;

            var methodAuthorization = await authorizationService.AuthorizeAsync(
                user,
                updateRequestDto,
                updatePolicyName).ConfigureAwait(false);
            if (!methodAuthorization.Succeeded)
            {
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
                return instance.NotFound();
            }

            var viewResult = await getUpdateActionResultAsync(result).ConfigureAwait(false);
            logger.LogDebug(
                eventId,
                "Finished UpdateAsync");

            return viewResult;
        }
    }
}
