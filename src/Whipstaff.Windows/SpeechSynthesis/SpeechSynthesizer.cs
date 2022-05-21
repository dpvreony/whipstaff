// -----------------------------------------------------------------------
// <copyright file="SpeechSynthesizer.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Speech.Synthesis;
using ReactiveMarbles.ObservableEvents;

namespace Whipstaff.Windows.SpeechSynthesis
{
    /// <summary>
    /// Handler for speech synthesis.
    /// </summary>
    public sealed class SpeechAnnouncements : IDisposable
    {
        private readonly SpeechSynthesizer _speechSynthesizer;
        private readonly IDisposable? _bookmarkReachedSubscription;
        private readonly IDisposable? _phonemeReachedSubscription;
        private readonly IDisposable? _speakCompletedSubscription;
        private readonly IDisposable? _speakProgressSubscription;
        private readonly IDisposable? _speakStartedSubscription;
        private readonly IDisposable? _stateChangedSubscription;
        private readonly IDisposable? _visemeReachedSubscription;
        private readonly IDisposable? _voiceChangeSubscription;

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
            this._speechSynthesizer = new SpeechSynthesizer
            {
                Volume = 50,
                Rate = 3
            };

            if (bookmarkReachedObserver != null)
            {
                this._bookmarkReachedSubscription = this._speechSynthesizer.Events().BookmarkReached.Subscribe(bookmarkReachedObserver);
            }

            if (phonemeReachedObserver != null)
            {
                this._phonemeReachedSubscription = this._speechSynthesizer.Events().PhonemeReached.Subscribe(phonemeReachedObserver);
            }

            if (speakCompletedObserver != null)
            {
                this._speakCompletedSubscription = this._speechSynthesizer.Events().SpeakCompleted.Subscribe(speakCompletedObserver);
            }

            if (speakProgressObserver != null)
            {
                this._speakProgressSubscription = this._speechSynthesizer.Events().SpeakProgress.Subscribe(speakProgressObserver);
            }

            if (speakStartedObserver != null)
            {
                this._speakStartedSubscription = this._speechSynthesizer.Events().SpeakStarted.Subscribe(speakStartedObserver);
            }

            if (stateChangedObserver != null)
            {
                this._stateChangedSubscription = this._speechSynthesizer.Events().StateChanged.Subscribe(stateChangedObserver);
            }

            if (visemeReachedObserver != null)
            {
                this._visemeReachedSubscription = this._speechSynthesizer.Events().VisemeReached.Subscribe(visemeReachedObserver);
            }

            if (voiceChangeObserver != null)
            {
                this._voiceChangeSubscription = this._speechSynthesizer.Events().VoiceChange.Subscribe(voiceChangeObserver);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Announce(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            _speechSynthesizer.Speak(text);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this._speechSynthesizer.Dispose();
            this._bookmarkReachedSubscription?.Dispose();
            this._phonemeReachedSubscription?.Dispose();
            this._speakCompletedSubscription?.Dispose();
            this._speakProgressSubscription?.Dispose();
            this._speakStartedSubscription?.Dispose();
            this._stateChangedSubscription?.Dispose();
            this._visemeReachedSubscription?.Dispose();
            this._voiceChangeSubscription?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
