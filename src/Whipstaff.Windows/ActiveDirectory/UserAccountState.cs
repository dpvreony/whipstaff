namespace Whipstaff.Windows.ActiveDirectory
{
    public enum UserAccountState
    {
        Unknown,
        Ok,
        Locked,
        PasswordExpiringWithinThreshold,
        PasswordExpired,
        AccountExpiringWithinThreshold,
        AccountExpired
    }
}
