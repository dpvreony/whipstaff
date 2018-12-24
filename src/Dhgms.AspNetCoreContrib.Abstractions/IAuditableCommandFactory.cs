namespace Dhgms.AspNetCoreContrib.Abstractions
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IAuditableCommandFactory<TAddCommand, in TAddRequestDto, TAddResponseDto, TDeleteCommand, TDeleteResponseDto, TUpdateCommand, in TUpdateRequestDto, TUpdateResponseDto>
        where TAddCommand : IAuditableRequest<TAddRequestDto, TAddResponseDto>
        where TDeleteCommand : IAuditableRequest<long, TDeleteResponseDto>
        where TUpdateCommand : IAuditableRequest<TUpdateRequestDto, TUpdateResponseDto>
    {
        Task<TAddCommand> GetAddCommandAsync(
            TAddRequestDto requestDto,
            System.Security.Claims.ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken);

        Task<TDeleteCommand> GetDeleteCommandAsync(
            long id,
            System.Security.Claims.ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken);

        Task<TUpdateCommand> GetUpdateCommandAsync(
            TUpdateRequestDto requestDto,
            System.Security.Claims.ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken);
    }
}
