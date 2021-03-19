// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Security.Claims;
using Dhgms.AspNetCoreContrib.App.Features.FileTransfer;
using Dhgms.AspNetCoreContrib.Controllers;

namespace Dhgms.AspNetCoreContrib.App.Features.Excel
{
    /// <summary>
    /// Auditable request for a spreadsheet download.
    /// </summary>
    public sealed class DownloadSpreadsheetRequestDto : AuditableRequest<int, FileNameAndStream>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadSpreadsheetRequestDto"/> class.
        /// </summary>
        /// <param name="requestDto">DTO for the request.</param>
        /// <param name="claimsPrincipal">Claims principal attached to the request.</param>
        public DownloadSpreadsheetRequestDto(int requestDto, ClaimsPrincipal claimsPrincipal)
            : base(requestDto, claimsPrincipal)
        {
        }
    }
}
