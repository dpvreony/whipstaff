// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using Whipstaff.Core.Logging;

namespace Whipstaff.Mermaid.Playwright
{
    /// <summary>
    /// Log Message Actions for <see cref="PlaywrightRenderer"/>.
    /// </summary>
    public sealed class PlaywrightRendererLogMessageActions : ILogMessageActions<PlaywrightRenderer>
    {
        private readonly Action<ILogger, Exception?> _failedToGetPageResponse;
        private readonly Action<ILogger, IResponse, Exception?> _unexpectedPageResponse;
        private readonly Action<ILogger, Exception?> _failedToFindMermaidElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaywrightRendererLogMessageActions"/> class.
        /// </summary>
        public PlaywrightRendererLogMessageActions()
        {
            _failedToGetPageResponse = LoggerMessage.Define(
                LogLevel.Error,
                new EventId(1, nameof(FailedToGetPageResponse)),
                "Failed to get page response.");

            _unexpectedPageResponse = LoggerMessage.Define<IResponse>(
                LogLevel.Error,
                new EventId(2, nameof(UnexpectedPageResponse)),
                "Unexpected page response: {PageResponse}");

            _failedToFindMermaidElement = LoggerMessage.Define(
                LogLevel.Error,
                new EventId(3, nameof(FailedToFindMermaidElement)),
                "Failed to find Mermaid element.");
        }

        /// <summary>
        /// Logs a failure to get a page response.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        public void FailedToGetPageResponse(ILogger<PlaywrightRenderer> logger)
        {
            _failedToGetPageResponse(
                logger,
                null);
        }

        /// <summary>
        /// Logs an unexpected page response.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        /// <param name="pageResponse">Page response that was returned.</param>
        public void UnexpectedPageResponse(
            ILogger<PlaywrightRenderer> logger,
            IResponse pageResponse)
        {
            _unexpectedPageResponse(
                logger,
                pageResponse,
                null);
        }

        /// <summary>
        /// Logs a failure to find the Mermaid element in the HTML.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        public void FailedToFindMermaidElement(ILogger<PlaywrightRenderer> logger)
        {
            _failedToFindMermaidElement(logger, null);
        }
    }
}
