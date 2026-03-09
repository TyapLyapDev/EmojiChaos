using System;

namespace EmojiChaos.UI.Panels.LevelsPanel.LevelSelector
{

public interface ILevelSelector
{
    event Action<int> LevelClicked;

    void Hide();

    void Show();

    void AlignByLevel(int levelIndex);
}
}
