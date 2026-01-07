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
using Whipstaff.Runtime.AppDomains;

namespace Whipstaff.Wpf
{
    /// <summary>
    /// WPF Application with reusable initialization logic. This makes ReactiveUI initialization more platform specific than the default.
    /// </summary>
    public abstract class AbstractWpfApplication : Application, IDisposable
    {
        private readonly CompositeDisposable _compositeDisposable;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractWpfApplication"/> class.
        /// </summary>
        /// <param name="assemblyResolveHelper">Helper to use for App domain assembly resolution failures.</param>
        protected AbstractWpfApplication(IAssemblyResolveHelper? assemblyResolveHelper)
        {
            ArgumentNullException.ThrowIfNull(assemblyResolveHelper);

            _compositeDisposable = new CompositeDisposable
            {
                CreateObservable(assemblyResolveHelper).Subscribe(),
                this.Events().DispatcherUnhandledException.Subscribe(x => OnDispatcherUnhandledException(x))
            };
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of the resources used by the application.
        /// </summary>
        /// <param name="disposing">Flag indicating whether disposing is taking place.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (!_compositeDisposable.IsDisposed)
            {
                _compositeDisposable.Dispose();
            }
        }

        /// <inheritdoc />
        protected sealed override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DoSplatDependencyInjectionInitialization();
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
        /// Placeholder for Splat Dependency Injection Initialization. This allows overriding the default Splat Dependency Injection initialization.
        /// </summary>
        protected abstract void DoSplatDependencyInjectionInitialization();

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

        private static IObservable<(ResolveEventArgs Args, Assembly? Assembly)> CreateObservable(IAssemblyResolveHelper? assemblyResolveHelper)
        {
            ArgumentNullException.ThrowIfNull(assemblyResolveHelper);

            return Observable.Create<(ResolveEventArgs Args, Assembly? Assembly)>(observer =>
            {
                // The handler will capture the message and produce a return value
                ResolveEventHandler handler = (sender, args) =>
                {
                    // Notify the observer with the message and the return value
                    var result = assemblyResolveHelper.OnAssemblyResolve(sender, args);
                    observer.OnNext((args, result));

                    return null; // return this value to the event invoker
                };

                AppDomain.CurrentDomain.AssemblyResolve += handler;

                // Return a disposable that unsubscribes from the event when disposed
                return () => AppDomain.CurrentDomain.AssemblyResolve -= handler;
            });
        }
    }
}
