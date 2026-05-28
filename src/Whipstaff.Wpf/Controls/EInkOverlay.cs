// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.VisualStudio.Threading;
using ReactiveMarbles.ObservableEvents;
using ReactiveUI;
using ReactiveUI.Extensions;

namespace Whipstaff.Wpf.Controls
{
    /// <summary>
    /// A ContentControl that provides an E-Ink style grayscale overlay effect.
    /// </summary>
    public class EInkOverlay : AbstractNoViewModelReactiveContentControl
    {
        private readonly JoinableTaskFactory _joinableTaskFactory = new JoinableTaskFactory(new JoinableTaskContext());
        private Grid? _mainContent;
        private Image? _eInkImage;
        private bool _isRefreshing;

        /// <summary>
        /// Initializes a new instance of the <see cref="EInkOverlay"/> class.
        /// </summary>
#pragma warning disable GR0027 // Constructor should have a logging framework instance as the final parameter.
#pragma warning disable GR0012 // Constructors should minimise work and not execute methods
        public EInkOverlay()
        {
            Background = System.Windows.Media.Brushes.Transparent;

            // Create the control template
            var controlTemplate = new ControlTemplate(typeof(EInkOverlay));

            // Create the visual tree factory
            var rootGrid = new FrameworkElementFactory(typeof(Grid));

            // Main content grid that will hold the ContentPresenter
            var mainContentGrid = new FrameworkElementFactory(typeof(Grid), "MainContent");
            var contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenter.SetValue(ContentPresenter.ContentProperty, new TemplateBindingExtension(ContentProperty));
            mainContentGrid.AppendChild(contentPresenter);

            // E-Ink overlay image
            var eInkImage = new FrameworkElementFactory(typeof(Image), "EInkImage");
            eInkImage.SetValue(Image.StretchProperty, Stretch.Fill);
            eInkImage.SetValue(Image.OpacityProperty, 0d);
            eInkImage.SetValue(Image.IsHitTestVisibleProperty, false);

            rootGrid.AppendChild(mainContentGrid);
            rootGrid.AppendChild(eInkImage);

            controlTemplate.VisualTree = rootGrid;
            Template = controlTemplate;

            _ = this.WhenActivated(compositeDisposable => HandleActivated(compositeDisposable));
        }
#pragma warning restore GR0012 // Constructors should minimise work and not execute methods
#pragma warning restore GR0027 // Constructor should have a logging framework instance as the final parameter.

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _mainContent = GetTemplateChild("MainContent") as Grid;
            _eInkImage = GetTemplateChild("EInkImage") as Image;
        }

        private static FormatConvertedBitmap ConvertToGrayscale(BitmapSource source)
        {
            var grayBitmap = new FormatConvertedBitmap();
            grayBitmap.BeginInit();
            grayBitmap.Source = source;
            grayBitmap.DestinationFormat = PixelFormats.Gray8;
            grayBitmap.EndInit();
            return grayBitmap;
        }

        private void HandleActivated(CompositeDisposable compositeDisposable)
        {
            var events = this.Events();
            _ = events.Loaded.SubscribeAsync(_ => HandleLoadedAsync())
                .DisposeWith(compositeDisposable);
            _ = events.LayoutUpdated.SubscribeAsync(_ => HandleLayoutUpdatedAsync())
                .DisposeWith(compositeDisposable);
        }

        private async Task HandleLayoutUpdatedAsync()
        {
            if (!_isRefreshing)
            {
                _isRefreshing = true;
                await SimulateRefreshAsync();
                _isRefreshing = false;
            }
        }

        private async Task HandleLoadedAsync()
        {
            await SimulateRefreshAsync();
        }

        private async Task CaptureAndApplyGrayscaleAsync()
        {
            if (_mainContent == null || _eInkImage == null)
            {
                return;
            }

            await Task.Delay(50); // Ensure UI is stable before capturing

            await _joinableTaskFactory.RunAsync(async () =>
            {
                await _joinableTaskFactory.SwitchToMainThreadAsync(alwaysYield: true);

                // Capture only MainContent (excluding the overlay itself)
                RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
                    (int)_mainContent.ActualWidth, (int)_mainContent.ActualHeight, 96, 96, PixelFormats.Pbgra32);

                renderBitmap.Render(_mainContent);

                _eInkImage.Source = ConvertToGrayscale(renderBitmap);
            });
        }

        private async Task SimulateRefreshAsync()
        {
            if (_eInkImage == null)
            {
                return;
            }

            await _joinableTaskFactory.RunAsync(async () =>
            {
                await _joinableTaskFactory.SwitchToMainThreadAsync(alwaysYield: true);
                _eInkImage.Opacity = 0; // Simulate blanking effect
            });
            await Task.Delay(300); // Pause before updating
            await CaptureAndApplyGrayscaleAsync();

            await _joinableTaskFactory.RunAsync(async () =>
            {
                await _joinableTaskFactory.SwitchToMainThreadAsync(alwaysYield: true);
                _eInkImage.Opacity = 1; // Show refreshed grayscale image
            });
        }
    }
}
