using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelBoxContainer : InitializingWithConfigBehaviour<LevelBoxContainerConfig>
{
    public const int CellCapacity = 20;

    [SerializeField] private LevelBox _levelBoxPrefab;

    private readonly List<LevelBox> _levelBoxes = new();
    private Transform _context;
    private int _currentPage;
    private int _currentLevelProgress;
    private LevelBoxContainerConfig _config;

    public event Action<int> ClickLockedLevel;
    public event Action<int> ClickUnlockedLevel;

    public void ShowCurrentProgressPage()
    {
        _currentLevelProgress = _config.Saver.LevelProgress;
        _currentPage = (_currentLevelProgress - 1) / CellCapacity;
        UpdateCountEarnedStars();
        ShowPage(_currentPage);
    }

    private void UpdateCountEarnedStars()
    {
        List<int> starsInfo = _config.Saver.GetStarsInfo();

        int starsInfoCount = starsInfo.Count;

        for (int i = 0; i < CellCapacity; i++)
        {
            int level = (i + 1) % CellCapacity;

            if (starsInfoCount < level)
                return;

            _levelBoxes[i].SetCountEarnedStars(starsInfo[level - 1]);
        }
    }

    private void ShowPage(int pageIndex)
    {
        int levelCount = _config.Saver.TotalLevelsCount;
        int startLevel = pageIndex * CellCapacity + 1;

        for (int i = 0; i < CellCapacity; i++)
        {
            int levelNumber = startLevel + i;
            LevelBox levelBox = _levelBoxes[i];

            if (levelNumber <= levelCount)
            {
                levelBox.ShowCell();
                levelBox.SetNumber(levelNumber);

                if (levelNumber <= _currentLevelProgress + 1)
                    levelBox.Unlock();
                else
                    levelBox.Lock();

                continue;
            }

            levelBox.HideCell();
        }
    }

    public void NextPage()
    {
        int maxPage = (_config.Saver.TotalLevelsCount - 1) / CellCapacity;

        if (_currentPage < maxPage)
        {
            _currentPage++;
            ShowPage(_currentPage);
        }
    }

    public void PreviousPage()
    {
        if (_currentPage > 0)
        {
            _currentPage--;
            ShowPage(_currentPage);
        }
    }

    protected override void OnInitialize(LevelBoxContainerConfig config)
    {
        _config = config;
        _context = transform;
        ClearContext();
        CreateCells();
    }

    private void ClearContext()
    {
        foreach (Transform child in _context)
            Destroy(child.gameObject);

        _levelBoxes.Clear();
    }

    private void CreateCells()
    {
        for (int i = 0; i < CellCapacity; i++)
        {
            LevelBox levelBox = Instantiate(_levelBoxPrefab, _context);
            levelBox.Lock();
            levelBox.HideCell();
            levelBox.Initialize();
            levelBox.Clicked += OnLevelClick;
            _levelBoxes.Add(levelBox);
        }
    }

    private void OnLevelClick(LevelBox levelBbox)
    {
        if (levelBbox.IsLock)
        {
            ClickLockedLevel?.Invoke(levelBbox.Number);

            return;
        }

        ClickUnlockedLevel?.Invoke(levelBbox.Number - 1);
    }
}