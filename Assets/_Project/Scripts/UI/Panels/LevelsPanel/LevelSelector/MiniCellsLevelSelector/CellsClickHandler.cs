using System;

namespace EmojiChaos.UI.Panels.LevelsPanel.LevelSelector.MiniCellsLevelSelector
{
    using Audio;
    using MiniCell;

    public class CellsClickHandler
    {
        private readonly Action<int> _levelClicked;

        public CellsClickHandler(Action<int> levelClicked)
        {
            _levelClicked = levelClicked ?? throw new ArgumentNullException(nameof(levelClicked));
        }

        public void OnClickCell(LevelCell cell)
        {
            if (cell.IsLock)
            {
                cell.Shake();
                Audio.Sfx.PlayLevelClosed();
            }
            else
            {
                _levelClicked?.Invoke(cell.LevelNumber);
                Audio.Sfx.PlayLevelSelected();
            }
        }
    }
}