using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinder
{ 
    public IReadOnlyList<Level> GetLevelPrefabs()
    {
        IReadOnlyList<Level> levels = Resources.LoadAll<Level>(Constants.LevelsPath);
        
        return levels;
    }

    public void UnloadUnusedLevels()
    {
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }
}