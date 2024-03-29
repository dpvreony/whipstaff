﻿// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Printing;
using System.Windows.Controls;

namespace Whipstaff.Wpf.InteractionFlows.PrintDialogInteraction
{
    /// <summary>
    /// Request Model for a Print Dialog.
    /// </summary>
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
