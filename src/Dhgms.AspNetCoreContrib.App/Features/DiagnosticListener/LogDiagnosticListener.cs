// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DiagnosticAdapter;
using Microsoft.Extensions.Logging;

namespace Dhgms.AspNetCoreContrib.App.Features.DiagnosticListener
{
    /// <summary>
    /// Middleware Log Diagnostic Listener.
    /// </summary>
    /// <remarks>
    /// Based upon https://andrewlock.net/understanding-your-middleware-pipeline-with-the-middleware-analysis-package/
    /// uses reflection. might be able to remove it.
    /// </remarks>
    public sealed class LogDiagnosticListener
    {
        private readonly ILogger<LogDiagnosticListener> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogDiagnosticListener"/> class.
        /// </summary>
        /// <param name="logger">Logging Framework Instance.</param>
        public LogDiagnosticListener(ILogger<LogDiagnosticListener> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Diagnostic event for when a middleware starts.
        /// </summary>
        /// <param name="httpContext">Http Context for the request.</param>
        /// <param name="name">Name of the middleware.</param>
        [DiagnosticName("Microsoft.AspNetCore.MiddlewareAnalysis.MiddlewareStarting")]
        public void OnMiddlewareStarting(HttpContext httpContext, string name)
        {
            _logger.LogInformation($"MiddlewareStarting: {name}; {httpContext?.Request.Path}");
        }

        /// <summary>
        /// Diagnostic event for when a middleware throws an exception.
        /// </summary>
        /// <param name="exception">Exception thrown by the middleware.</param>
        /// <param name="name">Name of the middleware.</param>
        [DiagnosticName("Microsoft.AspNetCore.MiddlewareAnalysis.MiddlewareException")]
        public void OnMiddlewareException(Exception exception, string name)
        {
            _logger.LogInformation(
                $"MiddlewareException: {name}",
                exception);
        }

        /// <summary>
        /// Diagnostic event for when a middleware has finished processing.
        /// </summary>
        /// <param name="httpContext">Http Context for the request.</param>
        /// <param name="name">Name of the middleware.</param>
        [DiagnosticName("Microsoft.AspNetCore.MiddlewareAnalysis.MiddlewareFinished")]
        public void OnMiddlewareFinished(HttpContext httpContext, string name)
        {
            _logger.LogInformation(
                $"MiddlewareFinished: {name}; {httpContext?.Response.StatusCode}");
        }
    }
}
