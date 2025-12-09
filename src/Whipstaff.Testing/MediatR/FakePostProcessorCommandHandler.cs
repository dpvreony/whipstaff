// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Whipstaff.EntityFramework.ModelCreation;
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
        private readonly Func<IModelCreator<FakeDbContext>> _modelCreatorFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakePostProcessorCommandHandler"/> class.
        /// </summary>
        /// <param name="fakeDbContextOptions">Entity Framework DB Context options for initializing instance.</param>
        /// <param name="modelCreatorFunc">Function used to build the database model. Allows for extra control for versions, features, and provider specific customization to be injected.</param>
        public FakePostProcessorCommandHandler(DbContextOptions<FakeDbContext> fakeDbContextOptions, Func<IModelCreator<FakeDbContext>> modelCreatorFunc)
        {
            _fakeDbContextOptions = fakeDbContextOptions ?? throw new ArgumentNullException(nameof(fakeDbContextOptions));
            _modelCreatorFunc = modelCreatorFunc ?? throw new ArgumentNullException(nameof(modelCreatorFunc));
        }

        /// <inheritdoc />
        public async Task Process(
            FakeCrudAddCommand request,
            int? response,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            using (var dbContext = new FakeDbContext(_fakeDbContextOptions, _modelCreatorFunc))
            {
                var entity = new FakeAddPostProcessAuditDbSet
                {
                    Value = request.RequestDto,
                    Created = DateTimeOffset.UtcNow
                };

                _ = await dbContext.FakeAddPostProcessAudit.AddAsync(entity, CancellationToken.None).ConfigureAwait(false);

                _ = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
        }
    }
}
