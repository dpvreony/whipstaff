// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Whipstaff.EntityFramework.ModelCreation;
using Whipstaff.Testing.Cqrs;
using Whipstaff.Testing.EntityFramework;
using Whipstaff.Testing.EntityFramework.DbSets;

namespace Whipstaff.Testing.Mediator
{
    /// <summary>
    /// Fake Crud Command Handler.
    /// </summary>
    public sealed class FakeCrudAddCommandHandler : ICommandHandler<FakeCrudAddCommand, int?>
    {
        private readonly DbContextOptions<FakeDbContext> _fakeDbContextOptions;
        private readonly Func<IModelCreator<FakeDbContext>> _modelCreatorFunc;
        private readonly ILogger<FakeCrudAddCommandHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeCrudAddCommandHandler"/> class.
        /// </summary>
        /// <param name="fakeDbContextOptions">Entity Framework DB Context options for initializing instance.</param>
        /// <param name="modelCreatorFunc">Function used to build the database model. Allows for extra control for versions, features, and provider specific customization to be injected.</param>
        /// <param name="logger">Logging framework instance.</param>
        public FakeCrudAddCommandHandler(
            DbContextOptions<FakeDbContext> fakeDbContextOptions,
            Func<IModelCreator<FakeDbContext>> modelCreatorFunc,
            ILogger<FakeCrudAddCommandHandler> logger)
        {
            _fakeDbContextOptions = fakeDbContextOptions ?? throw new ArgumentNullException(nameof(fakeDbContextOptions));
            _modelCreatorFunc = modelCreatorFunc ?? throw new ArgumentNullException(nameof(modelCreatorFunc));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public async ValueTask<int?> Handle(FakeCrudAddCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            using (var dbContext = new FakeDbContext(_fakeDbContextOptions, _modelCreatorFunc))
            {
                var entity = new FakeAddAuditDbSet
                {
                    Value = request.RequestDto,
                    Created = DateTimeOffset.UtcNow
                };

                _ = await dbContext.FakeAddAudit.AddAsync(entity, CancellationToken.None).ConfigureAwait(false);
                var saveResult = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

#pragma warning disable CA1848 // Use the LoggerMessage delegates
#pragma warning disable CA1873 // Avoid potentially expensive logging
                _logger.LogDebug("DbContext Save Result: {SaveResult}", saveResult);
#pragma warning restore CA1873 // Avoid potentially expensive logging
#pragma warning restore CA1848 // Use the LoggerMessage delegates
            }

            return await Task.FromResult(request.RequestDto).ConfigureAwait(false);
        }
    }
}
