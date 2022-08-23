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
    public sealed record DownloadPdfRequestDto(
        int QueryDto,
        ClaimsPrincipal ClaimsPrincipal)
        : AuditableQuery<int, FileNameAndStreamModel>(
            QueryDto,
            ClaimsPrincipal)
    {
    }
}
