// -----------------------------------------------------------------------
// <copyright file="SpeechSynthesizer.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Dhgms.Whipstaff.Model
{
    using System;
    using System.Speech.Synthesis;

    using Dhgms.Whipstaff.Core.Model;
    /// <summary>
    /// Handler for speech synthesis
    /// </summary>
    public class SpeechAnnouncements
    {
        private readonly SpeechSynthesizer speechSynthesizer;

        public SpeechAnnouncements(
            OperatingSystemFeatureSet actualFeatureSet,
            bool useSpeechAnnouncements)
        {
            if (actualFeatureSet == OperatingSystemFeatureSet.WindowsVistaOrLater && useSpeechAnnouncements)
            {
                this.speechSynthesizer = new SpeechSynthesizer
                {
                    Volume = 50,
                    Rate = 3
                };
            }
        }

        public void Announce(string text)
        {
            if (speechSynthesizer == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException("text");
            }

            speechSynthesizer.SpeakAsync(text);
        }
    }
}
