using System;
using System.Collections.Generic;

[Serializable]
public class SavesData
{
    public int SelectedLevel = 0;
    public int LevelProgress = 0;
    public int Score = 0;
    public float MusicVolume = 0.3f;
    public float SfxVolume = 0.7f;
    public List<LevelDataInfo> Levels = new();
    public bool IsPurchsingRack = false;
    public bool IsNoAds = false;

    public void Reset()
    {
        SelectedLevel = 0;
        LevelProgress = 0;
        Score = 0;
        Levels.Clear();
    }
}