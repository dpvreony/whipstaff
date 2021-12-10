using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Dhgms.Whipstaff.Model.Helper;

namespace Whipstaff.Windows
{
    internal static class NativeMethods
    {
        [DllImport("Wscapi.dll")]
        public static extern HRESULT WscGetSecurityProviderHealth(int providers, out int healthState);
    }
}
