// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Security.Claims;
using Dhgms.AspNetCoreContrib.App.Features.FileTransfer;
using Dhgms.AspNetCoreContrib.Controllers;

namespace Dhgms.AspNetCoreContrib.App.Features.Pdf
{
    /// <summary>
    /// Represents a PDF download request dto.
    /// </summary>
    public sealed class DownloadPdfRequestDto : AuditableRequest<int, FileNameAndStream>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadPdfRequestDto"/> class.
        /// </summary>
        /// <param name="requestDto">Unique id for the file.</param>
        /// <param name="claimsPrincipal">The claims principal associated with the request.</param>
        public DownloadPdfRequestDto(int requestDto, ClaimsPrincipal claimsPrincipal)
            : base(requestDto, claimsPrincipal)
        {
        }
    }
}
