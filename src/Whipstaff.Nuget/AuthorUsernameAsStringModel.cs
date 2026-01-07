// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Whipstaff.Core.Entities;
using Whipstaff.Runtime.Extensions;

namespace Whipstaff.Nuget
{
    /// <summary>
    /// Represents a nuget author username.
    /// </summary>
    public sealed class AuthorUsernameAsStringModel : IEntityAsString
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorUsernameAsStringModel"/> class.
        /// </summary>
        /// <param name="value">Nuget username as a string.</param>
        public AuthorUsernameAsStringModel(string value)
        {
#pragma warning disable GR0012 // Constructors should minimise work and not execute methods
            value.ThrowIfNullOrWhitespace();
            if (!value.IsAsciiLettersOrNumbers())
            {
                throw new ArgumentException("Author name must be ASCII letters or numbers", nameof(value));
            }
#pragma warning restore GR0012 // Constructors should minimise work and not execute methods

            Value = value;
        }

        /// <inheritdoc/>
        public string Value { get; }
    }
}
