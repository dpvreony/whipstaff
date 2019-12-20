using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Dhgms.AspNetCoreContrib.Fakes
{
    /// <summary>
    /// Fake Crud Command Handler.
    /// </summary>
    public sealed class FakeCrudAddCommandHandler : IRequestHandler<FakeCrudAddCommand, int>
    {
        /// <inheritdoc />
        public async Task<int> Handle(FakeCrudAddCommand request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(request.RequestDto).ConfigureAwait(false);
        }
    }
}
