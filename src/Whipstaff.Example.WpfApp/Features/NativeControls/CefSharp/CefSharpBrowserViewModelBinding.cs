// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using Vetuviem.Core;
using Whipstaff.Wpf.ViewToViewModelBindings.CefSharp.Wpf;

namespace Whipstaff.Example.WpfApp.Features.NativeControls.CefSharp
{
    internal sealed class CefSharpBrowserViewModelBinding : AbstractEnableViewToViewModelBindings<CefSharpBrowserView, CefSharpBrowserViewModel>
    {
        protected override IEnumerable<IControlBindingModel<CefSharpBrowserView, CefSharpBrowserViewModel>> GetBindings()
        {
            yield return new ChromiumWebBrowserControlBindingModel<CefSharpBrowserView, CefSharpBrowserViewModel>(vw => vw.ChromiumWebBrowser)
            {
                Address = new TwoWayBinding<CefSharpBrowserViewModel, string>(vm => vm.CurrentUrl)
            };
        }

        protected override IEnumerable<IDisposable> GetSubscriptions(CefSharpBrowserView view, CefSharpBrowserViewModel viewModel, IScheduler? scheduler)
        {
            yield break;
        }
    }
}
