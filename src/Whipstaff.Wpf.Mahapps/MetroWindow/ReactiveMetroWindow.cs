// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows;
using MahApps.Metro.Controls;
using ReactiveUI;

namespace Whipstaff.Wpf.Mahapps.MetroWindow
{
    /// <summary>
    /// A <see cref="MetroWindow"/> that is reactive.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class is a <see cref="MetroWindow"/> that is also reactive. That is, it implements <see cref="IViewFor{TViewModel}"/>.
    /// You can extend this class to get an implementation of <see cref="IViewFor{TViewModel}"/> rather than writing one yourself.
    /// </para>
    /// <para>
    /// Note that the XAML for your control must specify the same base class, including the generic argument you provide for your view
    /// model. To do this, use the <c>TypeArguments</c> attribute as follows:
    /// <code>
    /// <![CDATA[
    /// <rxui:ReactiveWindow
    ///         x:Class="Foo.Bar.Views.YourView"
    ///         x:TypeArguments="vms:YourViewModel"
    ///         xmlns:rxui="http://reactiveui.net"
    ///         xmlns:vms="clr-namespace:Foo.Bar.ViewModels"
    ///         xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    ///         xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    ///         xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    ///         xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    ///         mc:Ignorable="d">
    ///     <!-- view XAML here -->
    /// </rxui:ReactiveWindow>
    /// ]]>
    /// </code>
    /// </para>
    /// </remarks>
    /// <typeparam name="TViewModel">
    /// The type of the view model backing the view.
    /// </typeparam>
    public class ReactiveMetroWindow<TViewModel> : MahApps.Metro.Controls.MetroWindow, IViewFor<TViewModel>
        where TViewModel : class
    {
        /// <summary>
        /// Dependency Property for the view model.
        /// </summary>
#pragma warning disable RCS1158 // Static member in generic type should use a type parameter.
        public static readonly DependencyProperty ViewModelProperty =
#pragma warning restore RCS1158 // Static member in generic type should use a type parameter.
            DependencyProperty.Register(
                nameof(ViewModel),
                typeof(TViewModel),
                typeof(ReactiveMetroWindow<TViewModel>),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets the binding root view model.
        /// </summary>
        public TViewModel? BindingRoot => ViewModel;

        /// <inheritdoc/>
        public TViewModel? ViewModel
        {
            get => (TViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        /// <inheritdoc/>
        object? IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (TViewModel?)value;
        }
    }
}
