// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using ReactiveMarbles.ObservableEvents;
using ReactiveUI;

namespace Whipstaff.Wpf
{
    /// <summary>
    /// WPF Application with reusable initialization logic. This makes ReactiveUI initialization more platform specific than the default.
    /// </summary>
    public abstract class WpfApplication : Application
    {
        private readonly CompositeDisposable _compositeDisposable;

        /// <summary>
        /// Initializes a new instance of the <see cref="WpfApplication"/> class.
        /// </summary>
        protected WpfApplication()
        {
            _compositeDisposable =
            [
                Observable.FromEvent<ResolveEventHandler, ResolveEventArgs?>(
                    conversion => (sender, args) => OnAssemblyResolutionFailure(sender, args),
                    action => AppDomain.CurrentDomain.AssemblyResolve += action,
                    action => AppDomain.CurrentDomain.AssemblyResolve -= action).Subscribe(),

                this.Events().DispatcherUnhandledException.Subscribe(x => OnDispatcherUnhandledException(x))
            ];
        }

        /// <inheritdoc />
        protected sealed override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DoLoggingInitialization();
            DoApplicationPerformanceMonitoringInitialization();
            DoReactiveUIInitialization();
            OnApplicationStartup(e);
        }

        /// <inheritdoc />
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            _compositeDisposable.Dispose();
        }

        /// <summary>
        /// Placeholder for Application Performance Monitoring Initialization.
        /// </summary>
        protected abstract void DoApplicationPerformanceMonitoringInitialization();

        /// <summary>
        /// Placeholder for Logging Initialization.
        /// </summary>
        protected abstract void DoLoggingInitialization();

        /// <summary>
        /// Carries out Application Startup logic. This is the equivalent of <see cref="OnStartup"/> on <see cref="Application"/>. Which has been wrapped to carry out common initialization logic for ReactiveUI, Logging and APM.
        /// </summary>
        /// <param name="startupEventArgs">Contains the arguments for the application startup event.</param>
        protected abstract void OnApplicationStartup(StartupEventArgs startupEventArgs);

        /// <summary>
        /// This removes some of the assembly scanning ReactiveUI does by default by specifying the platform as WPF.
       /// </summary>
        private static void DoReactiveUIInitialization()
        {
            global::ReactiveUI.PlatformRegistrationManager.SetRegistrationNamespaces(RegistrationNamespace.Wpf);
        }

        private static void OnDispatcherUnhandledException(DispatcherUnhandledExceptionEventArgs dispatcherUnhandledExceptionEventArgs)
        {
            // the app should be written in a way this never happens.
            // but that is stating the obvious.
        }

        private static Assembly? OnAssemblyResolutionFailure(object? sender, ResolveEventArgs args)
        {
            return null;
        }
    }
}
