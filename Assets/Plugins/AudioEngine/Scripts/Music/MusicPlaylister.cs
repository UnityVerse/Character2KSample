using System;
using System.Collections.Generic;
using UnityEngine;

namespace AudioEngine
{
    public sealed class MusicPlaylister
    {
        public event Action OnStarted;
        public event Action OnStopped;
        public event Action OnFinished;
        public event Action<AudioClip> OnTrackChanged;

        public bool IsPlaying => _isPlaying;
        public AudioClip CurrentTrack => _playlist.Current;
        public IEnumerator<AudioClip> CurrentList => _playlist;

        private MusicPlayer musicPlayer;
        
        private IEnumerator<AudioClip> _playlist;
        private bool _isPlaying;

        public void Play(IEnumerator<AudioClip> playlist)
        {
            if (_isPlaying)
            {
                Debug.LogWarning("Playlist is already started!");
                return;
            }

            _playlist = playlist;
            _playlist.Reset();

            if (!_playlist.MoveNext())
            {
                Debug.LogWarning("Track list is empty");
                return;
            }

            _isPlaying = true;
            this.OnStarted?.Invoke();

            this.musicPlayer.OnTrackFinsihed += this.OnTrackFinished;
            this.musicPlayer.Stop();
            this.musicPlayer.Play(_playlist.Current);
            
            this.OnTrackChanged?.Invoke(_playlist.Current);
        }

        public void Stop()
        {
            if (!_isPlaying)
            {
                Debug.LogWarning("Playlist is not started!");
                return;
            }

            _isPlaying = false;
            _playlist.Dispose();

            this.musicPlayer.Stop();
            this.musicPlayer.OnTrackFinsihed -= this.OnTrackFinished;
            this.OnStopped?.Invoke();
        }

        public void Restart(IEnumerator<AudioClip> enumerator)
        {
            this.Stop();
            this.Play(enumerator);
        }

        public void SetPlayer(MusicPlayer player)
        {
            this.musicPlayer = player;
        }

        private void OnTrackFinished(AudioClip track)
        {
            if (_playlist.MoveNext())
            {
                this.musicPlayer.Play(_playlist.Current);
                this.OnTrackChanged?.Invoke(_playlist.Current);
            }
            else
            {
                _isPlaying = false;
                this.OnFinished?.Invoke();
            }
        }
    }
}