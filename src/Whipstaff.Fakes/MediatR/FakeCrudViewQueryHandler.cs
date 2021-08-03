using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Whipstaff.Testing.Cqrs;

namespace Whipstaff.Testing.MediatR
{
    /// <summary>
    /// CQRS Query Handler for the CRUD View Query.
    /// </summary>
    public class FakeCrudViewQueryHandler : IRequestHandler<FakeCrudViewQuery, FakeCrudViewResponse>
    {
        /// <inheritdoc />
        public Task<FakeCrudViewResponse> Handle(FakeCrudViewQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new FakeCrudViewResponse());
        }
    }
}
