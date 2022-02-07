namespace Whipstaff.Wpf.InteractionFlows.SaveFileDialogInteraction
{
    /// <summary>
    /// Represents the result of a Save File Dialog interaction.
    /// </summary>
    public sealed class SaveFileDialogResult
    {
        private SaveFileDialogResult(
            bool userProceeded,
            string? fileName)
        {
            UserProceeded = userProceeded;
            FileName = fileName;
        }

        /// <summary>
        /// Gets a flag indicating whether the user chose to proceed with the file open dialog.
        /// </summary>
        public bool UserProceeded { get; }

        /// <summary>
        /// Gets an array of file names, if the user chose to proceed.
        /// </summary>
        public string? FileName { get; }
        
        /// <summary>
        /// Creates an instance of <see cref="SaveFileDialogResult"/> indicating the user chose to cancel saving a file.
        /// </summary>
        /// <returns>An instance of <see cref="SaveFileDialogResult"/>.</returns>
        public static SaveFileDialogResult Cancelled()
        {
            return new SaveFileDialogResult(
                false,
                null);
        }

        /// <summary>
        /// Creates an instance of <see cref="SaveFileDialogResult"/> indicating the user chose to proceed with saving a file.
        /// </summary>
        /// <param name="fileName">The file name chosen by the user.</param>
        /// <returns>An instance of <see cref="SaveFileDialogResult"/>.</returns>
        public static SaveFileDialogResult Proceed(string fileName)
        {
            return new SaveFileDialogResult(
                true,
                fileName);
        }
    }
}
