namespace Dhgms.Whipstaff.Core.Model
{
    /// <summary>
    /// Enumeration representing the available security providers
    /// </summary>
    [Flags]
    public enum SecurityProvider
    {
        /// <summary>
        /// None Selected.
        /// </summary>
        None = 0,

        /// <summary>
        /// 
        /// </summary>
        WscSecurityProviderFirewall = 1,

        /// <summary>
        /// 
        /// </summary>
        WscSecurityProviderAutoupdateSettings = 2,

        /// <summary>
        /// 
        /// </summary>
        WscSecurityProviderAntivirus = 4,

        /// <summary>
        /// 
        /// </summary>
        WscSecurityProviderAntispyware = 8,

        /// <summary>
        /// 
        /// </summary>
        WscSecurityProviderInternetSettings = 16,

        /// <summary>
        /// 
        /// </summary>
        WscSecurityProviderUserAccountControl = 32,

        /// <summary>
        /// 
        /// </summary>
        WscSecurityProviderService = 64,

        /// <summary>
        /// 
        /// </summary>
        WscSecurityProviderAll = WscSecurityProviderFirewall | WscSecurityProviderAutoupdateSettings |
            WscSecurityProviderAntivirus | WscSecurityProviderAntispyware |
            WscSecurityProviderInternetSettings | WscSecurityProviderUserAccountControl |
            WscSecurityProviderService
    }
}
