// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Security.Cryptography.X509Certificates;

#if ARGUMENT_NULL_EXCEPTION_SHIM
using ArgumentNullException = Whipstaff.Runtime.Exceptions.ArgumentNullException;
#endif

namespace Whipstaff.Runtime.Cryptography.X509
{
    /// <summary>
    /// Extension methods for <see cref="X509Certificate2"/>.
    /// </summary>
    public static class X509Certificate2Extensions
    {
        /// <summary>
        /// Check to ensure that the certificate has a private key.
        /// </summary>
        /// <param name="certificate">Certificate to check.</param>
        public static void EnsurePrivateKey(this X509Certificate2 certificate)
        {
            ArgumentNullException.ThrowIfNull(certificate);

            try
            {
                // we wrap in try block as HasPrivateKey throws an exception on certain runtimes.
                if (certificate.HasPrivateKey)
                {
                    return;
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch
#pragma warning restore CA1031 // Do not catch general exception types
            {
                // no op
            }

            throw new ArgumentException("Certificate does not have a private key", nameof(certificate));
        }
    }
}
