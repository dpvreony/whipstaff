// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive.Concurrency;
using ReactiveUI;
using Windows.Win32.UI.WindowsAndMessaging;

namespace Whipstaff.Example.WpfApp.Features.MainWindow
{
    /// <summary>
    /// View model for an XML Viewer.
    /// </summary>
    internal sealed class MainViewModel : ReactiveObject, IMainViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel(IScheduler? scheduler = null)
        {
            WindowDisplayAffinity = new(scheduler);
        }

        /// <inheritdoc/>
        public Interaction<WINDOW_DISPLAY_AFFINITY, bool> WindowDisplayAffinity { get; }
    }
}
