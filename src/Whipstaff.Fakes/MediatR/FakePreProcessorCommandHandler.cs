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
        private readonly Action<ILogger, int, Exception?> _saveResultLogMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakePreProcessorCommandHandler"/> class.
        /// </summary>
        /// <param name="fakeDbContextOptions">Entity Framework DB Context options for initializing instance.</param>
        /// <param name="logger">Logging framework instance.</param>
        public FakePreProcessorCommandHandler(
            DbContextOptions<FakeDbContext> fakeDbContextOptions,
            ILogger<FakePreProcessorCommandHandler> logger)
        {
            _fakeDbContextOptions = fakeDbContextOptions ?? throw new ArgumentNullException(nameof(fakeDbContextOptions));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _saveResultLogMessage = LoggerMessageFactory.GetDbContextSaveResultLoggerMessageAction();
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

                _saveResultLogMessage(
                    _logger,
                    saveResult,
                    null);
            }
        }
    }
}
