namespace Whipstaff.Wpf.InteractionFlows.PrintDialogInteraction
{
    /// <summary>
    /// Represents the result of a Print Dialog interaction.
    /// </summary>
    public sealed class PrintDialogResult
    {
        /// <summary>
        /// Creates an instance of <see cref="PrintDialogResult"/> indicating the user chose to cancel printing.
        /// </summary>
        /// <returns>An instance of <see cref="PrintDialogResult"/>.</returns>
        public static PrintDialogResult Cancelled()
        {
            return new PrintDialogResult();
        }

        /// <summary>
        /// Creates an instance of <see cref="PrintDialogResult"/> indicating the user chose to proceed with printing.
        /// </summary>
        /// <returns>An instance of <see cref="PrintDialogResult"/>.</returns>
        public static PrintDialogResult Proceed()
        {
            return new PrintDialogResult();
        }
    }
}
