namespace Whipstaff.Wpf.Extensions
{
    /// <summary>
    /// Extensions for working with WPF windows.
    /// </summary>
    public static class WindowExtensions
    {
        /// <summary>Flashes the specified window. It does not change the active state of the window.</summary>
        /// <param name="window">WPF window to flash.</param>
        /// <returns>
        ///     The return value specifies the window's state before the call to the FlashWindowEx function. If the window
        ///     caption was drawn as active before the call, the return value is nonzero. Otherwise, the return value is zero.
        /// </returns>
        public static bool FlashWindow(this System.Windows.Window window)
        {
            var windowInteropHelper = new System.Windows.Interop.WindowInteropHelper(window);
            return Whipstaff.Windows.PInvoke.User32Helpers.FlashWindowEx(windowInteropHelper.Handle);
        }
    }
}
