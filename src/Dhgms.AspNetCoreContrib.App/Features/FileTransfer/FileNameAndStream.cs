using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Dhgms.AspNetCoreContrib.Example.WebSite.Features.FileTransfer
{
    public sealed class FileNameAndStream
    {
        public string FileName { get; set; }
        public Stream FileStream { get; set; }
    }
}
