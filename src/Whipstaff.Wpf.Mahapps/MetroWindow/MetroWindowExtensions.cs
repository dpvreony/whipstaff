// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;
using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;
using ReactiveUI;
using Whipstaff.Wpf.InteractionFlows;

namespace Whipstaff.Wpf.Mahapps.MetroWindow
{
    /// <summary>
    /// Extension methods for <see cref="MahApps.Metro.Controls.MetroWindow"/>.
    /// </summary>
    public static class MetroWindowExtensions
    {
        /// <summary>
        /// Handles the interaction request for an "Ok only" interaction.
        /// </summary>
        /// <param name="metroWindow">Metro window instance that will host the interaction notification.</param>
        /// <param name="interactionContext">Contains contextual information for the "Ok only" interaction.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public static async Task ShowMessageForOkCancelResponseAsync(
            this MahApps.Metro.Controls.MetroWindow metroWindow,
            InteractionContext<OkCancelInteractionRequest, bool> interactionContext)
        {
            ArgumentNullException.ThrowIfNull(interactionContext);

            var input = interactionContext.Input;
            var title = input.Title;
            var message = input.Message;
            var defaultButtonFocus = input.DefaultAffirmative
                ? MessageDialogResult.Affirmative
                : MessageDialogResult.Negative;

            var settings = new MetroDialogSettings
            {
                AffirmativeButtonText = input.OkButtonText,
                NegativeButtonText = input.CancelButtonText,
                DefaultButtonFocus = defaultButtonFocus
            };

            var response = await metroWindow.ShowMessageAsync(
                title,
                message,
                MessageDialogStyle.AffirmativeAndNegative,
                settings)
                .ConfigureAwait(false);

            interactionContext.SetOutput(response == MessageDialogResult.Affirmative);
        }

        /// <summary>
        /// Handles the interaction request for an "Error only" interaction.
        /// </summary>
        /// <param name="metroWindow">Metro window instance that will host the interaction notification.</param>
        /// <param name="interactionContext">Contains contextual information for the "Error only" interaction.</param>
        /// <param name="messagePrefix">Prefix to apply to the error message. Helpful when you have a standard "we're sorry" or "please report this" prefix.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public static async Task ShowMessageForErrorOnlyNotificationAsync(
            this MahApps.Metro.Controls.MetroWindow metroWindow,
            InteractionContext<ErrorOnlyInteractionRequest, Unit> interactionContext,
            string messagePrefix)
        {
            ArgumentNullException.ThrowIfNull(interactionContext);

            var title = "Error";
            var input = interactionContext.Input;
            var message = $"{messagePrefix}\r\n\r\n{input.Message}";
            _ = await metroWindow.ShowMessageAsync(title, message)
                .ConfigureAwait(false);

            interactionContext.SetOutput(Unit.Default);
        }

        /// <summary>
        /// Handles the interaction request for an "Ok only" interaction.
        /// </summary>
        /// <param name="metroWindow">Metro window instance that will host the interaction notification.</param>
        /// <param name="interactionContext">Contains contextual information for the "Ok only" interaction.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public static async Task ShowMessageForOkOnlyNotification(
            this MahApps.Metro.Controls.MetroWindow metroWindow,
            InteractionContext<OkOnlyInteractionRequest, Unit> interactionContext)
        {
            ArgumentNullException.ThrowIfNull(interactionContext);

            var input = interactionContext.Input;
            var title = input.Title;
            var message = input.Message;
            _ = await metroWindow.ShowMessageAsync(
                    title,
                    message)
                .ConfigureAwait(false);

            interactionContext.SetOutput(Unit.Default);
        }
    }
}
