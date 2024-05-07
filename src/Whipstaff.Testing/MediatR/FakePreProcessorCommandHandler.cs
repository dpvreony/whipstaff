// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Whipstaff.Core.Logging;
using Whipstaff.EntityFramework.ModelCreation;
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
        private readonly Func<IModelCreator<FakeDbContext>> _modelCreatorFunc;
        private readonly ILogger<FakePreProcessorCommandHandler> _logger;
        private readonly Action<ILogger, int, Exception?> _saveResultLogMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakePreProcessorCommandHandler"/> class.
        /// </summary>
        /// <param name="fakeDbContextOptions">Entity Framework DB Context options for initializing instance.</param>
        /// <param name="modelCreatorFunc">Function used to build the database model. Allows for extra control for versions, features, and provider specific customization to be injected.</param>
        /// <param name="logger">Logging framework instance.</param>
        public FakePreProcessorCommandHandler(
            DbContextOptions<FakeDbContext> fakeDbContextOptions,
            Func<IModelCreator<FakeDbContext>> modelCreatorFunc,
            ILogger<FakePreProcessorCommandHandler> logger)
        {
            _fakeDbContextOptions = fakeDbContextOptions ?? throw new ArgumentNullException(nameof(fakeDbContextOptions));
            _modelCreatorFunc = modelCreatorFunc ?? throw new ArgumentNullException(nameof(modelCreatorFunc));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _saveResultLogMessage = LoggerMessageFactory.GetDbContextSaveResultLoggerMessageAction();
        }

        /// <inheritdoc />
        public async Task Process(FakeCrudAddCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            using (var dbContext = new FakeDbContext(_fakeDbContextOptions, _modelCreatorFunc))
            {
                var entity = new FakeAddPreProcessAuditDbSet
                {
                    Value = request.RequestDto,
                    Created = DateTimeOffset.UtcNow
                };

                _ = await dbContext.FakeAddPreProcessAudit.AddAsync(entity, CancellationToken.None).ConfigureAwait(false);
                var saveResult = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);

                _saveResultLogMessage(
                    _logger,
                    saveResult,
                    null);
            }
        }
    }
}
