using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Fakes.Cqrs;
using MediatR;

namespace Dhgms.AspNetCoreContrib.Fakes.MediatR
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
