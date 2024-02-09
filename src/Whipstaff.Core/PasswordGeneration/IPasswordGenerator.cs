// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Core.PasswordGeneration
{
    /// <summary>
    /// Represents a Password generator.
    /// </summary>
    public interface IPasswordGenerator
    {
        /// <summary>
        /// Gets a generated password.
        /// </summary>
        /// <returns>generated password.</returns>
        string GetPassword();
    }
}
