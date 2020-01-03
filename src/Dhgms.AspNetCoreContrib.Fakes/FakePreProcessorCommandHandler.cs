using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;

namespace Dhgms.AspNetCoreContrib.Fakes
{
    /// <summary>
    /// Fake Pre-Processor Request Handler for MediatR.
    /// </summary>
    public sealed class FakePreProcessorCommandHandler : IRequestPreProcessor<FakeCrudAddCommand>
    {
        /// <inheritdoc />
        public Task Process(FakeCrudAddCommand request, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
