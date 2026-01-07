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
#pragma warning disable GR0027 // Constructor should have a logging framework instance as the final parameter.
        public MainWindow()
        {
            InitializeComponent();

            // ReSharper disable once ConvertClosureToMethodGroup
#pragma warning disable GR0012 // Constructors should minimise work and not execute methods
            _ = this.WhenActivated(d => OnWhenActivated(d));
#pragma warning restore GR0012 // Constructors should minimise work and not execute methods
        }
#pragma warning restore GR0027 // Constructor should have a logging framework instance as the final parameter.

        private void OnWhenActivated(Action<IDisposable> disposer)
        {
            disposer(this.BindSetWindowDisplayAffinityInteraction<MainWindow, IMainViewModel>());
        }
    }
}
