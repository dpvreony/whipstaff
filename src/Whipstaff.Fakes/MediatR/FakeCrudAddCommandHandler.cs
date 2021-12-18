using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Whipstaff.Testing.Cqrs;
using Whipstaff.Testing.EntityFramework;
using Whipstaff.Testing.EntityFramework.DbSets;

namespace Whipstaff.Testing.MediatR
{
    /// <summary>
    /// Fake Crud Command Handler.
    /// </summary>
    public sealed class FakeCrudAddCommandHandler : IRequestHandler<FakeCrudAddCommand, int>
    {
        private readonly DbContextOptions<FakeDbContext> _fakeDbContextOptions;
        private readonly ILogger<FakeCrudAddCommandHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeCrudAddCommandHandler"/> class.
        /// </summary>
        /// <param name="fakeDbContextOptions">Entity Framework DB Context options for initializing instance.</param>
        /// <param name="logger">Logging framework instance.</param>
        public FakeCrudAddCommandHandler(
            DbContextOptions<FakeDbContext> fakeDbContextOptions,
            ILogger<FakeCrudAddCommandHandler> logger)
        {
            _fakeDbContextOptions = fakeDbContextOptions ?? throw new ArgumentNullException(nameof(fakeDbContextOptions));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public async Task<int> Handle(FakeCrudAddCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (var dbContext = new FakeDbContext(_fakeDbContextOptions))
            {
                var entity = new FakeAddAuditDbSet
                {
                    Value = request.RequestDto,
                    Created = DateTimeOffset.UtcNow
                };

                _ = dbContext.FakeAddAudit.Add(entity);
                var saveResult = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

#pragma warning disable CA1848 // Use the LoggerMessage delegates
                this._logger.LogDebug("DbContext Save Result: {SaveResult}", saveResult);
#pragma warning restore CA1848 // Use the LoggerMessage delegates
            }

            return await Task.FromResult(request.RequestDto).ConfigureAwait(false);
        }
    }
}
