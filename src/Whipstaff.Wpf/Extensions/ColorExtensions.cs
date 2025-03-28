// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Wpf.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="global::Windows.UI.Color"/>.
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Checks if a color is considered to be a light color.
        /// </summary>
        /// <param name="color">Color to check.</param>
        /// <returns>Whether the color is considered to be light.</returns>
        public static bool IsLightColor(this global::Windows.UI.Color color)
            => (5 * color.G)
               + (2 * color.R)
               + color.B
               > 1024;
    }
}
