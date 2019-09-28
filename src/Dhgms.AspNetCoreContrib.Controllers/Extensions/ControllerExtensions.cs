// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Abstractions;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dhgms.AspNetCoreContrib.Controllers.Extensions
{
    /// <summary>
    /// Extension methods for ASP.NET Core Controllers.
    /// </summary>
    public static class ControllerExtensions
    {
        public static async Task<IActionResult> GetAddActionAsync<TAddRequestDto, TAddResponseDto, TAddCommand>(
            this Controller instance,
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
            if (addCommandFactoryAsync == null)
            {
                throw new ArgumentNullException(nameof(addCommandFactoryAsync));
            }

            if (getAddActionResultAsync == null)
            {
                throw new ArgumentNullException(nameof(getAddActionResultAsync));
            }

            logger.LogDebug(
                eventId,
                "Entered AddAsync");

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

        public static async Task<IActionResult> GetDeleteActionAsync<TDeleteResponseDto, TDeleteCommand>(
            this Controller instance,
            [NotNull] ILogger logger,
            [NotNull] IMediator mediator,
            [NotNull] IAuthorizationService authorizationService,
            [NotNull] long id,
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

            if (!instance.Request.IsHttps)
            {
                return instance.BadRequest();
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

        public static async Task<IActionResult> GetListActionAsync<TListRequestDto, TListResponseDto, TListQuery>(
            [NotNull] this Controller instance,
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

        public static async Task<IActionResult> GetViewActionAsync<TViewRequestDto, TViewResponseDto, TViewQuery>(
            [NotNull] this Controller instance,
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

        public static async Task<IActionResult> GetUpdateActionAsync<TUpdateRequestDto, TUpdateResponseDto, TUpdateCommand>(
            [NotNull] this Controller instance,
            [NotNull] ILogger logger,
            [NotNull] IMediator mediator,
            [NotNull] IAuthorizationService authorizationService,
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

            if (!instance.Request.IsHttps)
            {
                return instance.BadRequest();
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
