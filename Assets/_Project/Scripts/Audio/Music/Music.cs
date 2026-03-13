using System.Collections.Generic;
using EmojiChaos.Core.Abstract.MonoBehaviourWrapper;
using UnityEngine;

namespace EmojiChaos.AudioSpace
{
    public class Music : InitializingBehaviour
    {
        [SerializeField] private AudioSource _source;
        [SerializeField] private AudioClip _menuMusic;
        [SerializeField] private AudioClip _levelMusic;

        private MusicMode _currentMode = MusicMode.None;
        private MusicState _currentState = MusicState.Playing;

        private Dictionary<(MusicState, MusicCommand), Transition> _transitions;

        public void SetMute() =>
            ApplyCommand(MusicCommand.Mute);

        public void ResetMute() =>
            ApplyCommand(MusicCommand.Unmute);

        public void Pause() =>
            ApplyCommand(MusicCommand.Pause);

        public void UnPause() =>
            ApplyCommand(MusicCommand.Resume);

        public void PlayMenu() =>
            SetMode(MusicMode.Menu);

        public void PlayLevel() =>
            SetMode(MusicMode.Level);

        protected override void OnInitialize()
        {
            _source.loop = true;
            _source.playOnAwake = false;

            _transitions = new ()
            {
                [(MusicState.Playing, MusicCommand.Mute)] = new Transition(MusicState.Muted, _source.Pause),
                [(MusicState.Playing, MusicCommand.Pause)] = new Transition(MusicState.Paused, _source.Pause),
                [(MusicState.Paused, MusicCommand.Resume)] = new Transition(MusicState.Playing, _source.UnPause),
                [(MusicState.Paused, MusicCommand.Mute)] = new Transition(MusicState.PausedAndMuted),
                [(MusicState.Muted, MusicCommand.Unmute)] = new Transition(MusicState.Playing, PlayCurrentModeClip),
                [(MusicState.Muted, MusicCommand.Pause)] = new Transition(MusicState.PausedAndMuted),
                [(MusicState.PausedAndMuted, MusicCommand.Unmute)] = new Transition(MusicState.Paused),
                [(MusicState.PausedAndMuted, MusicCommand.Resume)] = new Transition(MusicState.Muted),
            };
        }

        private void ApplyCommand(MusicCommand cmd)
        {
            var key = (_currentState, cmd);

            if (_transitions.TryGetValue(key, out Transition transition))
            {
                _currentState = transition.NewState;
                transition.Action?.Invoke();
            }
        }

        private void SetMode(MusicMode newMode)
        {
            if (_currentMode == newMode)
                return;

            _currentMode = newMode;
            PlayCurrentModeClip();
        }

        private void UpdateClip()
        {
            AudioClip clip = _currentMode == MusicMode.Menu ? _menuMusic : _levelMusic;

            if (_source.clip != clip)
            {
                _source.clip = clip;
                _source.time = 0f;
            }
        }

        private void PlayCurrentModeClip()
        {
            UpdateClip();

            if (_currentState == MusicState.Playing)
                _source.Play();
        }
    }
}