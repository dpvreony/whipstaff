using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dhgms.AspNetCoreContrib.Abstractions
{
    public interface IAuditableCommandFactory<TAddRequestDto, TAddResponseDto, TDeleteResponse, TUpdateRequestDto, TUpdateResponseDto>
    {
        Task<IAuditableRequest<TAddRequestDto, TAddResponseDto>> GetAddCommandAsync(
            TAddRequestDto requestDto,
            System.Security.Claims.ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken);

        Task<IAuditableRequest<long, TDeleteResponse>> GetDeleteCommandAsync(
            long id,
            System.Security.Claims.ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken);

        Task<IAuditableRequest<TUpdateRequestDto, TUpdateResponseDto>> GetUpdateCommandAsync(
            TUpdateRequestDto requestDto,
            System.Security.Claims.ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken);
    }
}
