// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows;
using Whipstaff.Example.WpfApp.Features.Bootstrapping;
using Whipstaff.Example.WpfApp.Features.MainWindow;
using Whipstaff.ReactiveUI.Bootstrap;
using Whipstaff.Runtime.AppDomains;
using Whipstaff.Wpf;

namespace Whipstaff.Example.WpfApp
{
    /// <summary>
    /// Interaction logic for App.
    /// </summary>
    internal sealed partial class App
    {
#pragma warning disable GR0027 // Constructor should have a logging framework instance as the final parameter.
        public App()
            : base(new NullAssemblyResolveHelper())
        {
        }
#pragma warning restore GR0027 // Constructor should have a logging framework instance as the final parameter.

        /// <inheritdoc/>
        protected override string? GetCultureName()
        {
            return "en-GB";
        }

        /// <inheritdoc/>
        protected override void DoSplatDependencyInjectionInitialization()
        {
        }

        /// <inheritdoc/>
        protected override IReactiveUIBuilderRegistration GetReactiveUIRegistrations()
        {
            return new SampleContractModelBasedRegistration();
        }

        /// <inheritdoc/>
        protected override void DoApplicationPerformanceMonitoringInitialization()
        {
        }

        /// <inheritdoc/>
        protected override void DoLoggingInitialization()
        {
        }

        /// <inheritdoc/>
        protected override void OnApplicationStartup(StartupEventArgs startupEventArgs)
        {
            var view = new MainWindow();
            view.ViewModel = new MainViewModel();

            MainWindow = view;

            view.Show();
        }
    }
}
