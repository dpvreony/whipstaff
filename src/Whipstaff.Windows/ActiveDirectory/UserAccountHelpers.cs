// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;

namespace Whipstaff.Windows.ActiveDirectory
{
    /// <summary>
    /// Helper methods for Active Directory User Accounts.
    /// </summary>
    public static class UserAccountHelpers
    {
        /// <summary>
        /// Gets the state of a user account. It checks if the account is locked, the password has expired,
        /// the password is close to expiring based on a threshold, the account has expired, or the
        /// account is close to expiring.
        /// </summary>
        /// <param name="authenticablePrincipal">The account principal to check.</param>
        /// <param name="expiryThreshold">The expiry threshold to check for.</param>
        /// <returns>State of the User Account.</returns>
        /// <exception cref="ArgumentNullException">authenticablePrincipal is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">expiryThreshold is not a positive number.</exception>
        public static UserAccountState GetActiveDirectoryUserState(
            AuthenticablePrincipal authenticablePrincipal,
            TimeSpan expiryThreshold)
        {
            ArgumentNullException.ThrowIfNull(authenticablePrincipal);

            if (expiryThreshold.TotalMilliseconds < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(expiryThreshold), "Expiry threshold can not be negative");
            }

            var useThreshold = Math.Abs(expiryThreshold.TotalMilliseconds) > 1;

            if (authenticablePrincipal.IsAccountLockedOut())
            {
                return UserAccountState.Locked;
            }

            var maxDays = GetMaxPasswordAge()?.TotalDays;
            if (!authenticablePrincipal.PasswordNeverExpires && authenticablePrincipal.LastPasswordSet.HasValue && maxDays.HasValue)
            {
                var expiry = authenticablePrincipal.LastPasswordSet.Value.AddDays(maxDays.Value);

                // don't reorder these
                if (expiry < DateTime.Now)
                {
                    return UserAccountState.PasswordExpired;
                }

                // don't reorder these
                if (useThreshold && expiry < DateTime.Now.Add(expiryThreshold))
                {
                    return UserAccountState.PasswordExpiringWithinThreshold;
                }
            }

            var accountExpiry = authenticablePrincipal.AccountExpirationDate;
            if (accountExpiry.HasValue && accountExpiry.Value < DateTime.Now.Add(expiryThreshold))
            {
                // don't reorder these
                if (accountExpiry.Value < DateTime.Now)
                {
                    return UserAccountState.AccountExpired;
                }

                // don't reorder these
                if (useThreshold && accountExpiry.Value < DateTime.Now.Add(expiryThreshold))
                {
                    return UserAccountState.AccountExpiringWithinThreshold;
                }
            }

            return UserAccountState.Ok;
        }

        /// <summary>
        /// Gets the Maximum Password Age on the current domain.
        /// </summary>
        /// <returns>The max password age, if set.</returns>
        public static TimeSpan? GetMaxPasswordAge()
        {
            using (Domain d = Domain.GetCurrentDomain())
            {
                return GetMaxPasswordAge(d);
            }
        }

        /// <summary>
        /// Gets the maximum password age on the specified domain.
        /// </summary>
        /// <param name="domain">The active directory domain to check.</param>
        /// <returns>The max password age, if set.</returns>
        public static TimeSpan? GetMaxPasswordAge(Domain domain)
        {
            ArgumentNullException.ThrowIfNull(domain);

            using (DirectoryEntry directoryEntry = domain.GetDirectoryEntry())
            {
                using (DirectorySearcher ds = new(
                           directoryEntry,
                           "(objectClass=*)",
                           null,
                           SearchScope.Base))
                {
                    var sr = ds.FindOne();
                    if (sr == null)
                    {
                        return null;
                    }

                    foreach (DictionaryEntry dictionaryEntry in sr.Properties)
                    {
                        if (dictionaryEntry.Key is not string keyName)
                        {
                            continue;
                        }

                        if (!keyName.Equals("maxPwdAge", StringComparison.Ordinal))
                        {
                            continue;
                        }

                        if (dictionaryEntry.Value == null)
                        {
                            return null;
                        }

                        ResultPropertyValueCollection valueCollection = (ResultPropertyValueCollection)dictionaryEntry.Value;
                        var resultAsObject = valueCollection[0];

                        var resultAsLong = (long)resultAsObject;

                        return TimeSpan.FromTicks(resultAsLong).Duration();
                    }

                    return null;
                }
            }
        }
    }
}
