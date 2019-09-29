// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Security.Claims;
using Dhgms.AspNetCoreContrib.App.Features.FileTransfer;
using Dhgms.AspNetCoreContrib.Controllers;

namespace Dhgms.AspNetCoreContrib.App.Features.Pdf
{
    public sealed class DownloadPdfRequestDto : AuditableRequest<int, FileNameAndStream>
    {
        public DownloadPdfRequestDto(int requestDto, ClaimsPrincipal claimsPrincipal)
            : base(requestDto, claimsPrincipal)
        {
        }
    }
}
