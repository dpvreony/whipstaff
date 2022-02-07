using Whipstaff.Core.Entities;

namespace Whipstaff.Wpf.InteractionFlows.OpenFileDialogInteraction
{
    /// <summary>
    /// Request DTO for firing off an Open File Dialog interaction.
    /// </summary>
    /// <param name="Title">The title to set on the dialog.</param>
    public sealed record OpenFileDialogRequest(string Title) : ITitle
    {
    }
}
