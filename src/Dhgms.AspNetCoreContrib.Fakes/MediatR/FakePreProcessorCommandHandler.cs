using System.Threading;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Fakes.Cqrs;
using MediatR.Pipeline;

namespace Dhgms.AspNetCoreContrib.Fakes.MediatR
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
