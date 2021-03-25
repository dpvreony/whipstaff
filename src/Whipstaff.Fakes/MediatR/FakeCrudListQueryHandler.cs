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
    /// CQRS Query Handler for the CRUD List Query.
    /// </summary>
    public sealed class FakeCrudListQueryHandler : IRequestHandler<FakeCrudListQuery, IList<int>>
    {
        /// <inheritdoc />
        public async Task<IList<int>> Handle(FakeCrudListQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<int>
            {
                1,
                2,
                3,
                4,
                5
            }).ConfigureAwait(false);
        }
    }
}
