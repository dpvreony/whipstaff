// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Whipstaff.Playwright;

namespace Whipstaff.DocFxGen.DotNetTool.CommandLine
{
    /// <summary>
    /// Helpers for <see cref="PlaywrightBrowserTypeAndChannel"/>.
    /// </summary>
    public static class PlaywrightBrowserTypeAndChannelHelper
    {
        /// <summary>
        /// Converts a <see cref="PlaywrightBrowserTypeAndChannelCommandLineArgument"/> to a <see cref="PlaywrightBrowserTypeAndChannel"/>.
        /// </summary>
        /// <param name="playwrightBrowserTypeAndChannel">The command line argument representing the browser type and channel.</param>
        /// <returns>The corresponding <see cref="PlaywrightBrowserTypeAndChannel"/>.</returns>
        public static PlaywrightBrowserTypeAndChannel GetPlaywrightBrowserTypeAndChannel(PlaywrightBrowserTypeAndChannelCommandLineArgument playwrightBrowserTypeAndChannel)
        {
            switch (playwrightBrowserTypeAndChannel)
            {
                case PlaywrightBrowserTypeAndChannelCommandLineArgument.Chrome:
                    return PlaywrightBrowserTypeAndChannel.Chrome();
                case PlaywrightBrowserTypeAndChannelCommandLineArgument.ChromiumDefault:
                    return PlaywrightBrowserTypeAndChannel.ChromiumDefault();
                case PlaywrightBrowserTypeAndChannelCommandLineArgument.ChromeBeta:
                    return PlaywrightBrowserTypeAndChannel.ChromeBeta();
#if ChromiumCustom
                // case PlaywrightBrowserTypeAndChannelCommandLineArgument.ChromiumCustom:
                // return PlaywrightBrowserTypeAndChannel.ChromiumCustom();
#endif
                case PlaywrightBrowserTypeAndChannelCommandLineArgument.MsEdge:
                    return PlaywrightBrowserTypeAndChannel.MsEdge();
                case PlaywrightBrowserTypeAndChannelCommandLineArgument.MsEdgeBeta:
                    return PlaywrightBrowserTypeAndChannel.MsEdgeBeta();
                case PlaywrightBrowserTypeAndChannelCommandLineArgument.MsEdgeDev:
                    return PlaywrightBrowserTypeAndChannel.MsEdgeDev();
                case PlaywrightBrowserTypeAndChannelCommandLineArgument.Firefox:
                    return PlaywrightBrowserTypeAndChannel.Firefox();
                case PlaywrightBrowserTypeAndChannelCommandLineArgument.Webkit:
                    return PlaywrightBrowserTypeAndChannel.Webkit();
                default:
                    throw new ArgumentOutOfRangeException(nameof(playwrightBrowserTypeAndChannel), playwrightBrowserTypeAndChannel, null);
            }
        }
    }
}
