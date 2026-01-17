using System;
using UnityEngine;
using YG;

public class LevelsPanel : PanelBase
{
    [SerializeField] private LevelBoxContainer _levelBoxContainer;
    [SerializeField] private LevelsPreviousPageButton _previousPageButton;
    [SerializeField] private LevelsNextPageButton _nextPageButton;

    public event Action<int> LevelClicked;

    private void OnEnable()
    {        
        _previousPageButton.Clicked += OnPreviousPageClicked;
        _nextPageButton.Clicked += OnNextPageClicked;
    }

    private void OnDisable()
    {        
        _previousPageButton.Clicked -= OnPreviousPageClicked;
        _nextPageButton.Clicked -= OnNextPageClicked;
    }

    private void OnDestroy()
    {
        if (_levelBoxContainer != null)
        {
            _levelBoxContainer.ClickUnlockedLevel -= OnUnlockedLevelClicked;
            _levelBoxContainer.PageChanged -= OnPageChanged;
        }        
    }

    public void Initialize(Saver saver)
    {
        Initialize();
        _levelBoxContainer.Initialize(new LevelBoxContainerConfig(saver));
        _levelBoxContainer.ClickUnlockedLevel += OnUnlockedLevelClicked;

        OnPageChanged();
        _levelBoxContainer.PageChanged += OnPageChanged;

        _previousPageButton.Initialize();
        _nextPageButton.Initialize();
    }

    private void OnUnlockedLevelClicked(int levelIndex)
    {
        if (YG2.saves.SavesData.IsNoAds == false)
            YG2.InterstitialAdvShow();

        LevelClicked?.Invoke(levelIndex);
    }

    protected override void OnShow()
    {
        base.OnShow();
        _levelBoxContainer.ShowCurrentProgressPage();
    }

    private void OnPageChanged()
    {
        _previousPageButton.SetActive(_levelBoxContainer.IsFirstPage == false);
        _nextPageButton.SetActive(_levelBoxContainer.IsLastPage == false);
    }

    private void OnPreviousPageClicked(LevelsPreviousPageButton _) =>
        _levelBoxContainer.PreviousPage();

    private void OnNextPageClicked(LevelsNextPageButton _) =>
        _levelBoxContainer.NextPage();
}