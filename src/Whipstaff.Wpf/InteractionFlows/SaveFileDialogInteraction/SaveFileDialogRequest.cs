using Whipstaff.Core.Entities;

namespace Whipstaff.Wpf.InteractionFlows.SaveFileDialogInteraction
{
    /// <summary>
    /// Request DTO for firing off a Save File Dialog interaction.
    /// </summary>
    /// <param name="Title">The title to set on the dialog.</param>
    public record SaveFileDialogRequest(string Title) : ITitle
    {
    }
}
