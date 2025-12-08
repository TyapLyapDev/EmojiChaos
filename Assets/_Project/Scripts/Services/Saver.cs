using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YG;

namespace YG
{
    public partial class SavesYG
    {
        public SavesData SavesData = new();
    }
}

public class Saver
{
    private readonly int _totalLevelsCount;
    private readonly SavesData _data;

    public Saver(int totalLevelsCount)
    {
        if (totalLevelsCount < 0)
            throw new ArgumentOutOfRangeException(nameof(totalLevelsCount));

        _totalLevelsCount = totalLevelsCount;
        _data = YG2.saves.SavesData;
        EnterMissingData();
    }

    public int TotalLevelsCount => _totalLevelsCount;

    public int SelectedLevel => _data.SelectedLevel;

    public int LevelProgress => _data.LevelProgress;

    public float MusicVolume => _data.MusicVolume;

    public float SfxVolume => _data.SfxVolume;

    public bool CanLoadNextLevel => _data.SelectedLevel < _data.LevelProgress;


    public List<int> GetStarsInfo() =>
        _data.Levels.Select(v => v.CountStars).ToList();

    public bool TryIncreaseLevelProgress()
    {
        if (SelectedLevel < LevelProgress)
            return false;

        if (TotalLevelsCount <= LevelProgress + 1)
            return false;

        _data.LevelProgress++;

        return true;
    }

    public void SetSelectedLevel(int level)
    {
        if (_data.SelectedLevel < 0)
            throw new ArgumentOutOfRangeException(nameof(_data.SelectedLevel), "Выбранный уровень не может быть меньше нуля");

        if (_data.SelectedLevel > _data.LevelProgress)
            throw new ArgumentOutOfRangeException(nameof(SelectedLevel), "Выбранный уровень не может быть больше пройденного прогресса");

        _data.SelectedLevel = level;
    }

    public void SetMusicVolume(float volume) =>
        _data.MusicVolume = volume;

    public float SetSfxVolume(float volume) =>
        _data.SfxVolume = volume;

    public void SetWhereMoreStarCount(int value)
    {
        LevelDataInfo selectedLevel = _data.Levels[SelectedLevel];
        selectedLevel.CountStars = Mathf.Max(selectedLevel.CountStars, value);
    }

    public void Save() =>
        YG2.SaveProgress();

    public void ResetProgress()
    {
        _data.Reset();
        EnterMissingData();
    }

    private void EnterMissingData()
    {
        if (_data.Levels.Count == _totalLevelsCount)
            return;

        while (_data.Levels.Count < _totalLevelsCount)
            _data.Levels.Add(new LevelDataInfo());

        Save();
    }
}