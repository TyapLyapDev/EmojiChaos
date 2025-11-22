using UnityEngine;

public class PauseSwitcher
{
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

    private void ProcessPause()
    {
        if (_pauseCounter <= 0)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
    }
}
