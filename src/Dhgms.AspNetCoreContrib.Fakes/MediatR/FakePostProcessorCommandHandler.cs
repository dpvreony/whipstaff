using System;
using System.Threading;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Fakes.Cqrs;
using Dhgms.AspNetCoreContrib.Fakes.EntityFramework;
using Dhgms.AspNetCoreContrib.Fakes.EntityFramework.DbSets;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;

namespace Dhgms.AspNetCoreContrib.Fakes.MediatR
{
    /// <summary>
    /// Fake Post Processor for MediatR.
    /// </summary>
    public sealed class FakePostProcessorCommandHandler
        : IRequestPostProcessor<FakeCrudAddCommand, int>
    {
        private readonly DbContextOptions<FakeDbContext> _fakeDbContextOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeCrudAddCommandHandler"/> class.
        /// </summary>
        /// <param name="fakeDbContextOptions">Entity Framework DB Context options for initializing instance.</param>
        public FakePostProcessorCommandHandler(DbContextOptions<FakeDbContext> fakeDbContextOptions)
        {
            _fakeDbContextOptions = fakeDbContextOptions ?? throw new ArgumentNullException(nameof(fakeDbContextOptions));
        }

        /// <inheritdoc />
        public async Task Process(
            FakeCrudAddCommand request,
            int response,
            CancellationToken cancellationToken)
        {
            using (var dbContext = new FakeDbContext(_fakeDbContextOptions))
            {
                var entity = new FakeAddPostProcessAuditDbSet
                {
                    Value = request.RequestDto,
                    Created = DateTimeOffset.UtcNow
                };

                dbContext.FakeAddPostProcessAudit.Add(entity);
                await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
        }
    }
}
