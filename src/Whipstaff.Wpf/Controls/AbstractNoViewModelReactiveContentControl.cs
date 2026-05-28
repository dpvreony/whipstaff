// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using ReactiveUI;

namespace Whipstaff.Wpf.Controls
{
    /// <summary>
    /// A base class for reactive content controls that do not require a view model. This class uses a fake view model to satisfy the requirements of ReactiveContentControl, allowing derived classes to focus on their specific functionality without needing to implement a view model.
    /// </summary>
    public abstract class AbstractNoViewModelReactiveContentControl : ReactiveContentControl<FakeViewModel>
    {
    }
}
