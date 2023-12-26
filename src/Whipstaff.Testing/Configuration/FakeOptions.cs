// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel.DataAnnotations;

namespace Whipstaff.Testing.Configuration
{
    /// <summary>
    /// Fake options class for testing.
    /// </summary>
    public sealed class FakeOptions
    {
        /// <summary>
        /// Gets or sets the fake string.
        /// </summary>
        [Required]
        [MinLength(5)]
        [MaxLength(10)]
        public string? FakeString { get; set; }
    }
}
