// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive.Disposables;
using ReactiveUI;

namespace Whipstaff.Example.WpfApp.Features.NativeControls.ForCefSharp
{
    /// <summary>
    /// Interaction logic for Cef Sharp Browser View sample.
    /// </summary>
    public sealed partial class CefSharpBrowserView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CefSharpBrowserView"/> class.
        /// </summary>
#pragma warning disable GR0027 // Constructor should have a logging framework instance as the final parameter.
        public CefSharpBrowserView()
        {
            InitializeComponent();

#pragma warning disable GR0012 // Constructors should minimise work and not execute methods
            _ = this.WhenActivated(compositeDisposable => OnWhenActivate(compositeDisposable));
#pragma warning restore GR0012 // Constructors should minimise work and not execute methods
        }
#pragma warning restore GR0027 // Constructor should have a logging framework instance as the final parameter.

        private void OnWhenActivate(CompositeDisposable compositeDisposable)
        {
            new CefSharpBrowserViewModelBinding().ApplyBindings(
                compositeDisposable,
                this,
                ViewModel!);
        }
    }
}
