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
    internal static class ControllerExtensions
    {
        internal static async Task<IActionResult> GetAddActionAsync<TAddRequestDto, TAddResponseDto, TAddCommand>(
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

        internal static async Task<IActionResult> GetDeleteActionAsync<TDeleteResponseDto, TDeleteCommand>(
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

        internal static async Task<IActionResult> GetUpdateActionAsync<TUpdateRequestDto, TUpdateResponseDto, TUpdateCommand>(
            this Controller instance,
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
        {
            logger.LogDebug(eventId, "Entered UpdateAsync");

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

            var query = await updateCommandFactoryAsync(
                updateRequestDto,
                user,
                cancellationToken).ConfigureAwait(false);

            var result = await mediator.Send(
                query,
                cancellationToken).ConfigureAwait(false);

            var viewResult = await getUpdateActionResultAsync(result).ConfigureAwait(false);
            logger.LogDebug(
                eventId,
                "Finished UpdateAsync");

            return viewResult;
        }
    }
}
