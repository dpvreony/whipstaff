// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

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
        /// Initializes a new instance of the <see cref="SpeechAnnouncements"/> class.
        /// </summary>
        /// <param name="bookmarkReachedObserver">Observer for when a bookmark has been reached.</param>
        /// <param name="phonemeReachedObserver">Observer for when a phoneme has been reached.</param>
        /// <param name="speakCompletedObserver">Observer for when a speak method has been completed.</param>
        /// <param name="speakProgressObserver">Observer for when a speak progress event has triggered.</param>
        /// <param name="speakStartedObserver">Observer for when a speak method has started.</param>
        /// <param name="stateChangedObserver">Observer for when a speech state has changed.</param>
        /// <param name="visemeReachedObserver">Observer for when a viseme has been reached.</param>
        /// <param name="voiceChangeObserver">Observer for when the Voice has changed.</param>
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
            _speechSynthesizer = new SpeechSynthesizer
            {
                Volume = 50,
                Rate = 3
            };

            if (bookmarkReachedObserver != null)
            {
                _bookmarkReachedSubscription = _speechSynthesizer.Events().BookmarkReached.Subscribe(bookmarkReachedObserver);
            }

            if (phonemeReachedObserver != null)
            {
                _phonemeReachedSubscription = _speechSynthesizer.Events().PhonemeReached.Subscribe(phonemeReachedObserver);
            }

            if (speakCompletedObserver != null)
            {
                _speakCompletedSubscription = _speechSynthesizer.Events().SpeakCompleted.Subscribe(speakCompletedObserver);
            }

            if (speakProgressObserver != null)
            {
                _speakProgressSubscription = _speechSynthesizer.Events().SpeakProgress.Subscribe(speakProgressObserver);
            }

            if (speakStartedObserver != null)
            {
                _speakStartedSubscription = _speechSynthesizer.Events().SpeakStarted.Subscribe(speakStartedObserver);
            }

            if (stateChangedObserver != null)
            {
                _stateChangedSubscription = _speechSynthesizer.Events().StateChanged.Subscribe(stateChangedObserver);
            }

            if (visemeReachedObserver != null)
            {
                _visemeReachedSubscription = _speechSynthesizer.Events().VisemeReached.Subscribe(visemeReachedObserver);
            }

            if (voiceChangeObserver != null)
            {
                _voiceChangeSubscription = _speechSynthesizer.Events().VoiceChange.Subscribe(voiceChangeObserver);
            }
        }

        /// <summary>
        /// Uses Text To Speech to make an announcement.
        /// </summary>
        /// <param name="text">Message to convert to speech.</param>
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
            _speechSynthesizer.Dispose();
            _bookmarkReachedSubscription?.Dispose();
            _phonemeReachedSubscription?.Dispose();
            _speakCompletedSubscription?.Dispose();
            _speakProgressSubscription?.Dispose();
            _speakStartedSubscription?.Dispose();
            _stateChangedSubscription?.Dispose();
            _visemeReachedSubscription?.Dispose();
            _voiceChangeSubscription?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
