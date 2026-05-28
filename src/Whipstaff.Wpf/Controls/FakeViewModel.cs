// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using ReactiveUI;

namespace Whipstaff.Wpf.Controls
{
    /// <summary>
    /// This is a fake view model class used solely to satisfy the generic type parameter requirement of ReactiveUserControl. It does not contain any properties or logic and is not intended for actual use in the application.
    /// </summary>
#pragma warning disable GR0034 // ViewModel classes should inherit from a ViewModel interface.
#pragma warning disable S2094 // Classes should not be empty
    public sealed class FakeViewModel : ReactiveObject;
#pragma warning restore S2094 // Classes should not be empty
#pragma warning restore GR0034 // ViewModel classes should inherit from a ViewModel interface.
}
