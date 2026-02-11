using UI.LevelCards;
using UI.CustomMiniCellsLevelSelector;
using UnityEngine;

public readonly struct LevelInfo : ICardInfo, IMiniCellInfo
{
    public LevelInfo(int levelNumber, int starCount, int difficulty, bool isLock, CrowdSequenceType crowdSequenceType, Sprite preview)
    {
        LevelNumber = levelNumber;
        StarCount = starCount;
        CrowdSequenceType = crowdSequenceType;
        Difficulty = difficulty;
        IsLock = isLock;
        Preview = preview;
    }

    public int LevelNumber { get; }

    public int StarCount { get; }

    public int Difficulty { get; }

    public bool IsLock { get; }

    public CrowdSequenceType CrowdSequenceType { get; }

    public Sprite Preview { get; }
}