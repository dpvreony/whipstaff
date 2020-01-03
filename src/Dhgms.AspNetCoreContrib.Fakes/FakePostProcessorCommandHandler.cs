using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;

namespace Dhgms.AspNetCoreContrib.Fakes
{
    /// <summary>
    /// Fake Post Processor for MediatR.
    /// </summary>
    public sealed class FakePostProcessorCommandHandler
        : IRequestPostProcessor<FakeCrudAddCommand, int>
    {
        /// <inheritdoc />
        public Task Process(FakeCrudAddCommand request, int response, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
