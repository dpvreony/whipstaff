// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows.Media;

namespace Whipstaff.Wpf.ViewModelToViewSelectors
{
    /// <summary>
    /// Factory methods for converting view model values to <see cref="SolidColorBrush"/> instances.
    /// </summary>
    public static class SolidColorBrushFactory
    {
        /// <summary>
        /// Gets a brush for the length remaining.
        /// </summary>
        /// <param name="valueRemaining">The amount of the value remaining.</param>
        /// <returns><see cref="SolidColorBrush"/> instance.</returns>
        public static SolidColorBrush GetBrushForLengthRemaining(int valueRemaining)
        {
            return valueRemaining switch
            {
                < 0 => Brushes.Red,
                < 10 => Brushes.Orange,
                < 20 => Brushes.Gold,
                _ => Brushes.Black
            };
        }
    }
}
