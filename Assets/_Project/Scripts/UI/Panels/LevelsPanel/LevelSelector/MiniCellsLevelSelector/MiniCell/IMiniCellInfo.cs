namespace UI.CustomMiniCellsLevelSelector
{
    public interface IMiniCellInfo
    {
        public int LevelNumber { get; }

        public int StarCount { get; }

        bool IsLock { get; }
    }
}