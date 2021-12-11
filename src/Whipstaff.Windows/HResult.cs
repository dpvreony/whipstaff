
namespace Dhgms.Whipstaff.Model.Helper
{
    /// <summary>
    /// HRESULT for WinApi calls
    /// </summary>
#pragma warning disable CA1028 // Enum Storage should be Int32
    public enum HRESULT : long
#pragma warning restore CA1028 // Enum Storage should be Int32
    {
        /// <summary>
        /// Generic failure.
        /// </summary>
        S_FALSE = 0x0001,

        /// <summary>
        /// Windows API call succeeded.
        /// </summary>
        S_OK = 0x0000,

        /// <summary>
        /// Invalid argument passed to method.
        /// </summary>
        E_INVALIDARG = 0x80070057,

        /// <summary>
        /// API call returned out of memory error.
        /// </summary>
        E_OUTOFMEMORY = 0x8007000E
    }
}
