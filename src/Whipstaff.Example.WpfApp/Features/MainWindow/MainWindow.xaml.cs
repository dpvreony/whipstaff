// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using ReactiveUI;
using Whipstaff.Wpf.UiElements;

namespace Whipstaff.Example.WpfApp.Features.MainWindow
{
    /// <summary>
    /// Interaction logic for the Main Window.
    /// </summary>
    internal sealed partial class MainWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // ReSharper disable once ConvertClosureToMethodGroup
            _ = this.WhenActivated(d => OnWhenActivated(d));
        }

        private void OnWhenActivated(Action<IDisposable> disposer)
        {
            disposer(this.BindSetWindowDisplayAffinityInteraction<MainWindow, IMainViewModel>());
        }
    }
}
