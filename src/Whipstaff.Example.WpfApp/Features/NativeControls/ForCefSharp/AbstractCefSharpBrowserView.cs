// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveUI;

namespace Whipstaff.Example.WpfApp.Features.NativeControls.ForCefSharp
{
    /// <summary>
    /// Abstraction of the CefSharp Browser View ReactiveWindow. This is workaround usability issues with generics and XAML.
    /// </summary>
    public abstract class AbstractCefSharpBrowserView : ReactiveWindow<CefSharpBrowserViewModel>;
}
