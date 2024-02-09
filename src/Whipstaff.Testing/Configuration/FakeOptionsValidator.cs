// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Options;

namespace Whipstaff.Testing.Configuration
{
    /// <summary>
    /// Fake options validator for testing.
    /// </summary>
    [OptionsValidator]
    public partial class FakeOptionsValidator : IValidateOptions<FakeOptions>
    {
    }
}
