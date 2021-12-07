using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeCrudAddCommandHandler"/> class.
        /// </summary>
        /// <param name="fakeDbContextOptions">Entity Framework DB Context options for initializing instance.</param>
        public FakePreProcessorCommandHandler(DbContextOptions<FakeDbContext> fakeDbContextOptions)
        {
            _fakeDbContextOptions = fakeDbContextOptions ?? throw new ArgumentNullException(nameof(fakeDbContextOptions));
        }

        /// <inheritdoc />
        public async Task Process(FakeCrudAddCommand request, CancellationToken cancellationToken)
        {
            using (var dbContext = new FakeDbContext(_fakeDbContextOptions))
            {
                var entity = new FakeAddPreProcessAuditDbSet
                {
                    Value = request.RequestDto,
                    Created = DateTimeOffset.UtcNow
                };

                dbContext.FakeAddPreProcessAudit.Add(entity);
                await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
        }
    }
}
