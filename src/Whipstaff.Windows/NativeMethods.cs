using System.Runtime.InteropServices;
using Dhgms.Whipstaff.Model.Helper;

namespace Whipstaff.Windows
{
    internal static class NativeMethods
    {
        [DllImport("Wscapi.dll")]
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        public static extern HRESULT WscGetSecurityProviderHealth(int providers, out int healthState);
    }
}
