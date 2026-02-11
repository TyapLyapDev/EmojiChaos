using System;

public interface ILevelSelector
{
    event Action<int> LevelClicked;

    void Hide();

    void Show();

    void AlignByLevel(int levelIndex);
}