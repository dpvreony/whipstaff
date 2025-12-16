// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Security.Claims;
using Whipstaff.AspNetCore.FileTransfer;

namespace Whipstaff.AspNetCore.Features.Pdf
{
    /// <summary>
    /// Represents a PDF download request dto.
    /// </summary>
    public sealed class DownloadPdfQueryDto : AuditableQuery<int, FileNameAndStreamModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadPdfQueryDto"/> class.
        /// </summary>
        /// <param name="requestDto">Unique id for the file.</param>
        /// <param name="claimsPrincipal">The claims principal associated with the request.</param>
        public DownloadPdfQueryDto(int requestDto, ClaimsPrincipal claimsPrincipal)
            : base(requestDto, claimsPrincipal)
        {
        }
    }
}
