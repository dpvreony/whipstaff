using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeCrudAddCommandHandler"/> class.
        /// </summary>
        /// <param name="fakeDbContextOptions">Entity Framework DB Context options for initializing instance.</param>
        public FakeCrudAddCommandHandler(DbContextOptions<FakeDbContext> fakeDbContextOptions)
        {
            _fakeDbContextOptions = fakeDbContextOptions ?? throw new ArgumentNullException(nameof(fakeDbContextOptions));
        }

        /// <inheritdoc />
        public async Task<int> Handle(FakeCrudAddCommand request, CancellationToken cancellationToken)
        {
            using (var dbContext = new FakeDbContext(_fakeDbContextOptions))
            {
                var entity = new FakeAddAuditDbSet
                {
                    Value = request.RequestDto,
                    Created = DateTimeOffset.UtcNow
                };

                dbContext.FakeAddAudit.Add(entity);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }

            return await Task.FromResult(request.RequestDto).ConfigureAwait(false);
        }
    }
}
