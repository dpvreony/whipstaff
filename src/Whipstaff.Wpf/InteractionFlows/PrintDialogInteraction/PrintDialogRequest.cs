// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Printing;
using System.Windows.Controls;

namespace Whipstaff.Wpf.InteractionFlows.PrintDialogInteraction
{
    /// <summary>
    /// Request Model for a Print Dialog.
    /// </summary>
    /// <param name="CurrentPageEnabled">Whether the current page is enabled.</param>
    /// <param name="MaxPage">The maximum page number.</param>
    /// <param name="MinPage">The minimum page number.</param>
    /// <param name="PageRange">The page range.</param>
    /// <param name="PageRangeSelection">The page range selection.</param>
    /// <param name="PrintQueue">The print queue.</param>
    /// <param name="SelectedPagesEnabled">Whether the selected pages are enabled.</param>
    /// <param name="UserPageRangeEnabled">Whether the user page range is enabled.</param>
    public sealed record PrintDialogRequest(
        bool? CurrentPageEnabled,
        uint? MaxPage,
        uint? MinPage,
        PageRange? PageRange,
        PageRangeSelection? PageRangeSelection,
        PrintQueue? PrintQueue,
        bool? SelectedPagesEnabled,
        bool? UserPageRangeEnabled)
    {
    }
}
