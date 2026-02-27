using System;
using System.Collections.Generic;
using System.Linq;
using UI.CustomMiniCellsLevelSelector;
using UI.LevelCards;
using UnityEngine;

public class LevelsPanel : PanelBase
{
    [SerializeField] private MiniCellsLevelSelector _levelBoxContainer;
    [SerializeField] private CardsLevelSelector _cardsLevelSelector;
    [SerializeField] private LevelRepresentationSwitcherButton _levelRepresentationSwitcherButton;

    private readonly LevelFinder _levelFinder = new();
    private List<ILevelSelector> _selectors;
    private ILevelSelector _currentSelector;
    private int _levelProgress;

    public event Action<int> LevelClicked;

    private void OnDestroy()
    {
        if(_levelRepresentationSwitcherButton != null)
            _levelRepresentationSwitcherButton.Clicked -= OnClickSwitchRepresentation;

        UnsubscribeSelector();
    }

    public void Initialize(int levelProgress, List<int> starInfos)
    {
        Initialize();
        _levelRepresentationSwitcherButton.Initialize();
        _levelProgress = levelProgress;

        IReadOnlyList<Level> levelPrefabs = _levelFinder.GetLevelPrefabs();
        List<LevelInfo> levelInfos = new();

        for (int i = 0; i < levelPrefabs.Count; i++)
        {
            Level level = levelPrefabs[i];
            CrowdSequenceType crowdSequenceType = level.IsRandomSequence ? CrowdSequenceType.Random : CrowdSequenceType.Deterministic;

            LevelInfo levelInfo = new(
                levelNumber: i + 1,
                starCount: starInfos[i],
                difficulty: level.Difficulty,
                isLock: i > _levelProgress,
                crowdSequenceType: crowdSequenceType,
                preview: level.Preview);

            levelInfos.Add(levelInfo);
        }

        _levelFinder.UnloadUnusedLevels();
        _levelProgress = levelProgress;InitSelectors(levelInfos);

        _levelRepresentationSwitcherButton.Clicked += OnClickSwitchRepresentation;
    }

    private void InitSelectors(List<LevelInfo> levelInfos)
    {
        _levelBoxContainer.Init(levelInfos.Cast<IMiniCellInfo>().ToList());
        _cardsLevelSelector.Init(levelInfos.Cast<ICardInfo>().ToList());

        _selectors = new()
        {
            _cardsLevelSelector,
            _levelBoxContainer
        };

        SetSelector(_selectors[0]);
    }

    private void SetSelector(ILevelSelector selector)
    {
        UnsubscribeSelector();
        _currentSelector = selector;
        SubscribeSelector();

        foreach (ILevelSelector levelSelector in _selectors)
            levelSelector.Hide();

        if (_currentSelector is MiniCellsLevelSelector _)
            _levelRepresentationSwitcherButton.ShowCardIcon();
        else
            _levelRepresentationSwitcherButton.ShowCellIcon();

            _currentSelector.Show();
        _currentSelector.AlignByLevel(_levelProgress);
    }

    private void SubscribeSelector()
    {
        if (_currentSelector != null)
            _currentSelector.LevelClicked += OnLevelClicked;
    }

    private void UnsubscribeSelector()
    {
        if (_currentSelector != null)
            _currentSelector.LevelClicked -= OnLevelClicked;
    }

    private void OnLevelClicked(int levelIndex) =>
        LevelClicked?.Invoke(levelIndex);

    protected override void OnShow()
    {
        base.OnShow();
        _currentSelector.AlignByLevel(_levelProgress);
    }

    private void OnClickSwitchRepresentation(LevelRepresentationSwitcherButton button)
    {
        int currentIndex = _selectors.IndexOf(_currentSelector);
        int nextIndex = (currentIndex + 1) % _selectors.Count;
        ILevelSelector selector = _selectors[nextIndex];
        SetSelector(selector);        
    }
}
