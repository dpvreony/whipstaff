using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Whipstaff.Testing.Cqrs;
using Whipstaff.Testing.EntityFramework;
using Whipstaff.Testing.EntityFramework.DbSets;

namespace Whipstaff.Testing.MediatR
{
    /// <summary>
    /// Fake Pre-Processor Request Handler for MediatR.
    /// </summary>
    public sealed class FakePreProcessorCommandHandler : IRequestPreProcessor<FakeCrudAddCommand>
    {
        private readonly DbContextOptions<FakeDbContext> _fakeDbContextOptions;
        private readonly ILogger<FakePreProcessorCommandHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeCrudAddCommandHandler"/> class.
        /// </summary>
        /// <param name="fakeDbContextOptions">Entity Framework DB Context options for initializing instance.</param>
        /// <param name="logger">Logging framework instance.</param>
        public FakePreProcessorCommandHandler(
            DbContextOptions<FakeDbContext> fakeDbContextOptions,
            ILogger<FakePreProcessorCommandHandler> logger)
        {
            _fakeDbContextOptions = fakeDbContextOptions ?? throw new ArgumentNullException(nameof(fakeDbContextOptions));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public async Task Process(FakeCrudAddCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (var dbContext = new FakeDbContext(_fakeDbContextOptions))
            {
                var entity = new FakeAddPreProcessAuditDbSet
                {
                    Value = request.RequestDto,
                    Created = DateTimeOffset.UtcNow
                };

                _ = dbContext.FakeAddPreProcessAudit.Add(entity);
                var saveResult = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);

                this._logger.LogDebug($"DbContext Save Result: {saveResult}");
            }
        }
    }
}
