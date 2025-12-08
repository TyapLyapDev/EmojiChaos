using System;
using UnityEngine;

public class LevelsPanel : PanelBase 
{
    [SerializeField] private LevelBoxContainer _levelBoxContainer;    

    public event Action<int> LevelClicked;

    private void OnDestroy()
    {
        if (_levelBoxContainer != null)
            _levelBoxContainer.ClickUnlockedLevel -= OnUnlockedLevelClicked;
    }

    public void Initialize(Saver saver)
    {
        Initialize();
        _levelBoxContainer.Initialize(new LevelBoxContainerConfig(saver));
        _levelBoxContainer.ClickUnlockedLevel += OnUnlockedLevelClicked;
    }

    private void OnUnlockedLevelClicked(int levelIndex) =>
        LevelClicked?.Invoke(levelIndex);

    protected override void OnShow()
    {
        base.OnShow();
        _levelBoxContainer.ShowCurrentProgressPage();
    }
}