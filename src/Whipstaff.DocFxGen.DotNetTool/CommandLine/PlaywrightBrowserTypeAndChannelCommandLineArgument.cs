using System;
using System.Collections.Generic;
using System.Text;

namespace Whipstaff.DocFxGen.DotNetTool.CommandLine
{
    /// <summary>
    /// enum representation of the supported Playwright browser types and channels.
    /// </summary>
    public enum PlaywrightBrowserTypeAndChannelCommandLineArgument
    {
        None,
        ChromiumDefault,
        Chrome,
        ChromeBeta,
        ChromiumCustom,
        MsEdge,
        MsEdgeBeta,
        MsEdgeDev,
        Firefox,
        Webkit
    }
}
