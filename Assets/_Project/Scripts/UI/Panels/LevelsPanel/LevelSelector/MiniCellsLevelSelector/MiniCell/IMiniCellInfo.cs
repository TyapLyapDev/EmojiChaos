namespace EmojiChaos.UI.Panels.LevelsPanel.LevelSelector.MiniCellsLevelSelector.MiniCell
{
    public interface IMiniCellInfo
    {
        public int LevelNumber { get; }

        public int StarCount { get; }

        bool IsLock { get; }
    }
}