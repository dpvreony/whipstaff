// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Whipstaff.Runtime.Extensions;

namespace Whipstaff.Playwright
{
    /// <summary>
    /// Represents a Playwright browser type and channel combination.
    /// </summary>
    public sealed class PlaywrightBrowserTypeAndChannel
    {
        internal PlaywrightBrowserTypeAndChannel(PlaywrightBrowserType playwrightBrowserType, string? channel)
        {
            PlaywrightBrowserType = playwrightBrowserType;
            Channel = channel;
        }

        /// <summary>
        /// Gets the Playwright browser type.
        /// </summary>
        public PlaywrightBrowserType PlaywrightBrowserType { get; }

        /// <summary>
        /// Gets the browser channel, if any.
        /// </summary>
        public string? Channel { get; }

        /// <summary>
        /// Creates a new instance of the PlaywrightBrowserTypeAndChannel class for the default Playwright Chromium browser.
        /// </summary>
        /// <returns><see cref="PlaywrightBrowserTypeAndChannel"/> instance.</returns>
        public static PlaywrightBrowserTypeAndChannel ChromiumDefault() => new(PlaywrightBrowserType.Chromium, null);

        /// <summary>
        /// Creates a new instance of the PlaywrightBrowserTypeAndChannel class for the Google Chrome Beta browser.
        /// </summary>
        /// <returns><see cref="PlaywrightBrowserTypeAndChannel"/> instance.</returns>
        public static PlaywrightBrowserTypeAndChannel Chrome() => new(PlaywrightBrowserType.Chromium, "chrome");

        /// <summary>
        /// Creates a new instance of the PlaywrightBrowserTypeAndChannel class for the Google Chrome browser.
        /// </summary>
        /// <returns><see cref="PlaywrightBrowserTypeAndChannel"/> instance.</returns>
        public static PlaywrightBrowserTypeAndChannel ChromeBeta() => new(PlaywrightBrowserType.Chromium, "chrome-beta");

        /// <summary>
        /// Creates a new instance of the PlaywrightBrowserTypeAndChannel class for a custom Chromium browser.
        /// </summary>
        /// <param name="channel">The custom channel to use.</param>
        /// <returns><see cref="PlaywrightBrowserTypeAndChannel"/> instance.</returns>
        public static PlaywrightBrowserTypeAndChannel ChromiumCustom(string channel)
        {
            channel.ThrowIfNullOrWhitespace();
            return new PlaywrightBrowserTypeAndChannel(PlaywrightBrowserType.Chromium, channel);
        }

        /// <summary>
        /// Creates a new instance of the PlaywrightBrowserTypeAndChannel class for the default Microsoft Edge browser.
        /// </summary>
        /// <returns><see cref="PlaywrightBrowserTypeAndChannel"/> instance.</returns>
        public static PlaywrightBrowserTypeAndChannel MsEdge() => new(PlaywrightBrowserType.Chromium, "msedge");

        /// <summary>
        /// Creates a new instance of the PlaywrightBrowserTypeAndChannel class for the Microsoft Edge Beta browser.
        /// </summary>
        /// <returns><see cref="PlaywrightBrowserTypeAndChannel"/> instance.</returns>
        public static PlaywrightBrowserTypeAndChannel MsEdgeBeta() => new(PlaywrightBrowserType.Chromium, "msedge-beta");

        /// <summary>
        /// Creates a new instance of the PlaywrightBrowserTypeAndChannel class for the Microsoft Edge Dev browser.
        /// </summary>
        /// <returns><see cref="PlaywrightBrowserTypeAndChannel"/> instance.</returns>
        public static PlaywrightBrowserTypeAndChannel MsEdgeDev() => new(PlaywrightBrowserType.Chromium, "msedge-dev");

        /// <summary>
        /// Creates a new instance of the PlaywrightBrowserTypeAndChannel class for the default Playwright Firefox browser.
        /// </summary>
        /// <returns><see cref="PlaywrightBrowserTypeAndChannel"/> instance.</returns>
        public static PlaywrightBrowserTypeAndChannel Firefox() => new(PlaywrightBrowserType.Firefox, null);

        /// <summary>
        /// Creates a new instance of the PlaywrightBrowserTypeAndChannel class for the default Playwright Webkit browser.
        /// </summary>
        /// <returns><see cref="PlaywrightBrowserTypeAndChannel"/> instance.</returns>
        public static PlaywrightBrowserTypeAndChannel Webkit() => new(PlaywrightBrowserType.WebKit, null);
    }
}
