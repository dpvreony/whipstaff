namespace Whipstaff.Windows.ActiveDirectory
{
    /// <summary>
    /// Represents the state of a user account.
    /// </summary>
    public enum UserAccountState
    {
        /// <summary>
        /// The account state 
        /// </summary>
        Unknown,

        /// <summary>
        /// The user account is ok.
        /// </summary>
        Ok,

        /// <summary>
        /// The account is locked
        /// </summary>
        Locked,

        /// <summary>
        /// The password is expiring within the defined threshold.
        /// </summary>
        PasswordExpiringWithinThreshold,

        /// <summary>
        /// The account password has expired.
        /// </summary>
        PasswordExpired,

        /// <summary>
        /// The account is expiring within the defined threshold.
        /// </summary>
        AccountExpiringWithinThreshold,

        /// <summary>
        /// The account has expired.
        /// </summary>
        AccountExpired
    }
}
