// -----------------------------------------------------------------------
// <copyright file="SpeechSynthesizer.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Speech.Synthesis;
using ReactiveMarbles.ObservableEvents;

namespace Dhgms.Whipstaff.Model
{

    using Dhgms.Whipstaff.Core.Model;
    /// <summary>
    /// Handler for speech synthesis.
    /// </summary>
    public sealed class SpeechAnnouncements : IDisposable
    {
        private readonly SpeechSynthesizer speechSynthesizer;

        /// <summary>
        /// Creates an instance of <see cref="SpeechAnnouncements"/>
        /// </summary>
        /// <param name="bookmarkReachedObserver"></param>
        /// <param name="phonemeReachedObserver"></param>
        /// <param name="speakCompletedObserver"></param>
        /// <param name="speakProgressObserver"></param>
        /// <param name="speakStartedObserver"></param>
        /// <param name="stateChangedObserver"></param>
        /// <param name="visemeReachedObserver"></param>
        /// <param name="voiceChangeObserver"></param>
        public SpeechAnnouncements(
            IObserver<BookmarkReachedEventArgs>? bookmarkReachedObserver,
            IObserver<PhonemeReachedEventArgs>? phonemeReachedObserver,
            IObserver<SpeakCompletedEventArgs>? speakCompletedObserver,
            IObserver<SpeakProgressEventArgs>? speakProgressObserver,
            IObserver<SpeakStartedEventArgs>? speakStartedObserver,
            IObserver<StateChangedEventArgs>? stateChangedObserver,
            IObserver<VisemeReachedEventArgs>? visemeReachedObserver,
            IObserver<VoiceChangeEventArgs>? voiceChangeObserver)
        {
            this.speechSynthesizer = new SpeechSynthesizer
            {
                Volume = 50,
                Rate = 3
            };

            if (bookmarkReachedObserver != null)
            {
                this.speechSynthesizer.Events().BookmarkReached.Subscribe(bookmarkReachedObserver);
            }

            if (phonemeReachedObserver != null)
            {
                this.speechSynthesizer.Events().PhonemeReached.Subscribe(phonemeReachedObserver);
            }

            if (speakCompletedObserver != null)
            {
                this.speechSynthesizer.Events().SpeakCompleted.Subscribe(speakCompletedObserver);
            }

            if (speakProgressObserver != null)
            {
                this.speechSynthesizer.Events().SpeakProgress.Subscribe(speakProgressObserver);
            }

            if (speakStartedObserver != null)
            {
                this.speechSynthesizer.Events().SpeakStarted.Subscribe(speakStartedObserver);
            }

            if (stateChangedObserver != null)
            {
                this.speechSynthesizer.Events().StateChanged.Subscribe(stateChangedObserver);
            }

            if (visemeReachedObserver != null)
            {
                this.speechSynthesizer.Events().VisemeReached.Subscribe(visemeReachedObserver);
            }

            if (voiceChangeObserver != null)
            {
                this.speechSynthesizer.Events().VoiceChange.Subscribe(voiceChangeObserver);
            }
        }

        public void Announce(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            speechSynthesizer.Speak(text);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.speechSynthesizer.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
