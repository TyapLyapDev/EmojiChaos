using System;
using UnityEngine;

namespace EmojiChaos.Services.Core
{
    public class PauseSwitcher
    {
        private float _timeScale = 1f;
        private int _pauseCounter = 0;

        public void SetPause()
        {
            _pauseCounter++;
            ProcessPause();
        }

        public void SetResume()
        {
            _pauseCounter--;
            ProcessPause();
        }

        public void SetTimeScale(float timeScale)
        {
            if (timeScale < 0)
                throw new ArgumentOutOfRangeException(nameof(timeScale), timeScale, "The value must be positive");

            _timeScale = timeScale;

            ProcessPause();
        }

        private void ProcessPause()
        {
            if (_pauseCounter <= 0)
                Time.timeScale = _timeScale;
            else
                Time.timeScale = 0;
        }
    }
}