// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive.Concurrency;
using ReactiveUI;

namespace Whipstaff.Example.WpfApp.Features.NativeControls.CefSharp
{
    /// <summary>
    /// View model for the CefSharp browser control example.
    /// </summary>
    internal sealed class CefSharpBrowserViewModel : ReactiveObject, ICefSharpBrowserViewModel
    {
        private readonly IScheduler _scheduler;

        public CefSharpBrowserViewModel(IScheduler scheduler)
        {
            _scheduler = scheduler;
            CurrentUrl = string.Empty;
        }

        /// <summary>
        /// Gets or sets the current url of the browser control.
        /// </summary>
        public string CurrentUrl
        {
            get;
            set => this.RaiseAndSetIfChanged(
                ref field,
                value);
        }
    }
}
