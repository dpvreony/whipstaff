using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Abstractions;
using Dhgms.AspNetCoreContrib.Controllers;
using Dhgms.AspNetCoreContrib.Example.WebSite.Features.FileTransfer;

namespace Dhgms.AspNetCoreContrib.Example.WebSite.Features.Pdf
{
    public sealed class DownloadPdfRequestDto : AuditableRequest<int, FileNameAndStream>
    {
        public DownloadPdfRequestDto(int requestDto, ClaimsPrincipal claimsPrincipal)
            : base(requestDto, claimsPrincipal)
        {
        }
    }
}
