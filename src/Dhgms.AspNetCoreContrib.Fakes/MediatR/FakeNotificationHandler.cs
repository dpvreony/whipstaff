using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Dhgms.AspNetCoreContrib.Fakes.MediatR
{
    /// <summary>
    /// Fake Notification Handler for MediatR.
    /// </summary>
    public sealed class FakeNotificationHandler : INotificationHandler<FakeNotification>
    {
        /// <inheritdoc />
        public Task Handle(FakeNotification notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
