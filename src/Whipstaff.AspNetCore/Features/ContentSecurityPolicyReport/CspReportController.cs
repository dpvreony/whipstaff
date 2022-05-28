// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Mvc;

namespace Whipstaff.AspNetCore.Features.ContentSecurityPolicyReport
{
    /// <summary>
    /// Controller for handling CSP requests.
    /// </summary>
    /// <remarks>
    /// Based upon:
    /// https://github.com/anthonychu/aspnet-core-csp/
    /// https://anthonychu.ca/post/aspnet-core-csp/
    /// Concept is identical, some adjustments for CQRS etc.
    /// </remarks>
    public sealed class CspReportController : Controller
    {
    }
}
