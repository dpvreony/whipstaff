// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows;
using Whipstaff.Example.WpfApp.Features.MainWindow;

namespace Whipstaff.Example.WpfApp
{
    /// <summary>
    /// Interaction logic for App.
    /// </summary>
    internal sealed partial class App : Application
    {
        /// <inheritdoc/>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var view = new MainWindow();
            view.ViewModel = new MainViewModel();

            MainWindow = view;

            view.Show();
        }
    }
}
