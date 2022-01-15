using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;

namespace Whipstaff.Windows.ActiveDirectory
{
    public static class UserAccountHelpers
    {
        public static UserAccountState GetActiveDirectoryUserState(UserPrincipal userPrincipal, TimeSpan expiryThreshold)
        {
            if (userPrincipal == null)
            {
                throw new ArgumentNullException(nameof(userPrincipal));
            }

            if (expiryThreshold.TotalMilliseconds < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(expiryThreshold), "Expiry threshold can not be negative");
            }

            var useThreshold = Math.Abs(expiryThreshold.TotalMilliseconds) > 1;

            if (userPrincipal.IsAccountLockedOut())
            {
                return UserAccountState.Locked;
            }

            var maxDays = GetMaxPasswordAge()?.TotalDays;
            if (!userPrincipal.PasswordNeverExpires && userPrincipal.LastPasswordSet.HasValue && maxDays.HasValue)
            {
                var expiry = userPrincipal.LastPasswordSet.Value.AddDays(maxDays.Value);

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

            var accountExpiry = userPrincipal.AccountExpirationDate;
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

        public static TimeSpan? GetMaxPasswordAge()
        {
            using (Domain d = Domain.GetCurrentDomain())
            {
                return GetMaxPasswordAge(d);
            }
        }

        public static TimeSpan? GetMaxPasswordAge(Domain d)
        {
            using (DirectoryEntry domain = d.GetDirectoryEntry())
            {
                using (DirectorySearcher ds = new(
                           domain,
                           "(objectClass=*)",
                           null,
                           SearchScope.Base
                       ))
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
