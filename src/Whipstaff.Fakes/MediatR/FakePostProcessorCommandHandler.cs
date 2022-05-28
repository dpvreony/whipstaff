// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

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
    /// Fake Post Processor for MediatR.
    /// </summary>
    public sealed class FakePostProcessorCommandHandler
        : IRequestPostProcessor<FakeCrudAddCommand, int?>
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
            int? response,
            CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (var dbContext = new FakeDbContext(_fakeDbContextOptions))
            {
                var entity = new FakeAddPostProcessAuditDbSet
                {
                    Value = request.RequestDto,
                    Created = DateTimeOffset.UtcNow
                };

                _ = dbContext.FakeAddPostProcessAudit.Add(entity);

                var saveResult = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
        }
    }
}
