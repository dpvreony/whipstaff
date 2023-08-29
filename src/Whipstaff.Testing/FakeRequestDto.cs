// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Testing
{
    /// <summary>
    /// Request DTO for testing http requests.
    /// </summary>
    /// <param name="Name">Sample Name.</param>
    /// <param name="Age">Sample Age.</param>
    public sealed record FakeRequestDto(string? Name, int? Age);
}
