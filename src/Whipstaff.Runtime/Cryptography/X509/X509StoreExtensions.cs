// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

#if ARGUMENT_NULL_EXCEPTION_SHIM
using ArgumentNullException = Whipstaff.Runtime.Exceptions.ArgumentNullException;
#endif

namespace Whipstaff.Runtime.Cryptography.X509
{
    /// <summary>
    /// Extension methods for <see cref="X509Store"/>.
    /// </summary>
    public static class X509StoreExtensions
    {
        /// <summary>
        /// Gets a collection of expired certificates.
        /// </summary>
        /// <param name="store">The X509 store.</param>
        /// <returns>Expired certificates.</returns>
        public static IEnumerable<X509Certificate2> GetExpiredCertificateCollection(this X509Store store)
        {
            var cutOff = DateTime.UtcNow;

            return GetExpiringCertificateCollection(store, cutOff);
        }

        /// <summary>
        /// Gets a collection of expiring certificates.
        /// </summary>
        /// <param name="store">The X509 store.</param>
        /// <param name="cutOff">The timestamp to use as the cut off.</param>
        /// <returns>Expiring certificates.</returns>
        public static IEnumerable<X509Certificate2> GetExpiringCertificateCollection(this X509Store store, DateTime cutOff)
        {
            return GetCertificateCollectionViaSelector(
                store,
                certificate => certificate.NotAfter > cutOff);
        }

        /// <summary>
        /// Gets a collection of certificates based on a selector.
        /// </summary>
        /// <param name="store">The X509 store.</param>
        /// <param name="selectorFunc">Filter to apply to the certificate collection.</param>
        /// <returns>Collection of certificates.</returns>
        public static IEnumerable<X509Certificate2> GetCertificateCollectionViaSelector(this X509Store store, Func<X509Certificate2, bool> selectorFunc)
        {
            ArgumentNullException.ThrowIfNull(store);
            ArgumentNullException.ThrowIfNull(selectorFunc);

            return InternalGetCertificateCollectionViaSelector(store, selectorFunc);
        }

        private static IEnumerable<X509Certificate2> InternalGetCertificateCollectionViaSelector(this X509Store store, Func<X509Certificate2, bool> selectorFunc)
        {
            foreach (var storeCertificate in store.Certificates)
            {
                if (!selectorFunc(storeCertificate))
                {
                    continue;
                }

                yield return storeCertificate;
            }
        }
    }
}
