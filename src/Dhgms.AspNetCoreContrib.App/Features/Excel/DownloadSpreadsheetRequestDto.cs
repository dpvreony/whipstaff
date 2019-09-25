using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Controllers;
using Dhgms.AspNetCoreContrib.Example.WebSite.Features.FileTransfer;

namespace Dhgms.AspNetCoreContrib.Example.WebSite.Features.Excel
{
    public sealed class DownloadSpreadsheetRequestDto : AuditableRequest<int, FileNameAndStream>
    {
        public DownloadSpreadsheetRequestDto(int requestDto, ClaimsPrincipal claimsPrincipal)
            : base(requestDto, claimsPrincipal)
        {
        }
    }
}
