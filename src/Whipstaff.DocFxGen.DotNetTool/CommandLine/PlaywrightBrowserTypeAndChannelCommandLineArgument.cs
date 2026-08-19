// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Whipstaff.DocFxGen.DotNetTool.CommandLine
{
    /// <summary>
    /// enum representation of the supported Playwright browser types and channels.
    /// </summary>
    public enum PlaywrightBrowserTypeAndChannelCommandLineArgument
    {
        /// <summary>
        /// No browser type and channel specified.
        /// </summary>
        None,

        /// <summary>
        /// Use Chrome.
        /// </summary>
        Chrome,

        /// <summary>
        /// Use the beta version of Chrome.
        /// </summary>
        ChromeBeta,

        /// <summary>
        /// Use the default Chromium browser type and channel.
        /// </summary>
        ChromiumDefault,

        /// <summary>
        /// Use a custom Chromium browser type and channel.
        /// </summary>
        ChromiumCustom,

        /// <summary>
        /// Use Microsoft Edge.
        /// </summary>
        MsEdge,

        /// <summary>
        /// Use beta version of Microsoft Edge.
        /// </summary>
        MsEdgeBeta,

        /// <summary>
        /// Use a dev version of Microsoft Edge.
        /// </summary>
        MsEdgeDev,

        /// <summary>
        /// Use Firefox.
        /// </summary>
        Firefox,

        /// <summary>
        /// Use webkit.
        /// </summary>
        Webkit
    }
}
