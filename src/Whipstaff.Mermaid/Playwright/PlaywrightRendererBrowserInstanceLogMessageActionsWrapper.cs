// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using Whipstaff.Core.Logging;

namespace Whipstaff.Mermaid.Playwright
{
    /// <summary>
    /// Wrapper for log message actions for <see cref="PlaywrightRenderer"/>.
    /// </summary>
    public sealed class PlaywrightRendererBrowserInstanceLogMessageActionsWrapper : AbstractLogMessageActionsWrapper<PlaywrightRendererBrowserInstance, PlaywrightRendererBrowserInstanceLogMessageActions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaywrightRendererBrowserInstanceLogMessageActionsWrapper"/> class.
        /// </summary>
        /// <param name="browserInstanceLogMessageActions">Log Message Actions instance.</param>
        /// <param name="logger">Logging framework instance.</param>
#pragma warning disable S6672 // Generic logger injection should match enclosing type
        public PlaywrightRendererBrowserInstanceLogMessageActionsWrapper(
            PlaywrightRendererBrowserInstanceLogMessageActions browserInstanceLogMessageActions,
            ILogger<PlaywrightRendererBrowserInstance> logger)
            : base(
                browserInstanceLogMessageActions,
                logger)
#pragma warning restore S6672 // Generic logger injection should match enclosing type
        {
        }

        /// <summary>
        /// Logs a failure to get a page response.
        /// </summary>
        public void FailedToGetPageResponse()
        {
            BrowserInstanceLogMessageActions.FailedToGetPageResponse(Logger);
        }

        /// <summary>
        /// Logs an unexpected page response.
        /// </summary>
        /// <param name="pageResponse">Page response that was returned.</param>
        public void UnexpectedPageResponse(IResponse pageResponse)
        {
            BrowserInstanceLogMessageActions.UnexpectedPageResponse(Logger, pageResponse);
        }

        /// <summary>
        /// Logs a failure to find the Mermaid element in the HTML.
        /// </summary>
        public void FailedToFindMermaidElement()
        {
            BrowserInstanceLogMessageActions.FailedToFindMermaidElement(Logger);
        }
    }
}
