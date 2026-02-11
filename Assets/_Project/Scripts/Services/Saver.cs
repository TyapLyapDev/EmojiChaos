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

    public Saver(int totalLevelsCount)
    {
        if (totalLevelsCount < 0)
            throw new ArgumentOutOfRangeException(nameof(totalLevelsCount));

        _totalLevelsCount = totalLevelsCount;
        EnterMissingData();
    }

    public int TotalLevelsCount => _totalLevelsCount;

    public int SelectedLevel => Data.SelectedLevel;

    public int LevelProgress => Data.LevelProgress;

    public float MusicVolume => Data.MusicVolume;

    public float SfxVolume => Data.SfxVolume;

    public bool NextLevelExists => Data.SelectedLevel + 1 < TotalLevelsCount;

    public bool IsNoAds => Data.IsNoAds;

    private SavesData Data => YG2.saves.SavesData;

    public List<int> GetStarInfos() =>
        Data.Levels.Select(v => v.CountStars).ToList();

    public bool TryIncreaseLevelProgress()
    {
        if (SelectedLevel < LevelProgress)
            return false;

        if (TotalLevelsCount <= LevelProgress)
            return false;

        Data.LevelProgress++;

        return true;
    }

    public void SetSelectedLevel(int level)
    {
        if (Data.SelectedLevel < 0)
            throw new ArgumentOutOfRangeException(nameof(Data.SelectedLevel), level, "Выбранный уровень не может быть меньше нуля");

        if (level > Data.LevelProgress)
            throw new ArgumentOutOfRangeException(nameof(SelectedLevel), level, "Выбранный уровень не может быть больше пройденного прогресса");

        Data.SelectedLevel = level;
    }

    public void SetMusicVolume(float volume) =>
        Data.MusicVolume = volume;

    public float SetSfxVolume(float volume) =>
        Data.SfxVolume = volume;

    public void SetWhereMoreStarCount(int value)
    {
        LevelDataInfo selectedLevel = Data.Levels[SelectedLevel];
        selectedLevel.CountStars = Mathf.Max(selectedLevel.CountStars, value);
    }

    public void IncreaseScore(int score)
    {
        Data.Score += score;

        if (YG2.player.auth)
            YG2.SetLeaderboard(Constants.LeaderboardTechnoName, Data.Score);

        if (LeaderboardYGMediator.Instance != null)
            LeaderboardYGMediator.Instance.RequestAnUpdate();
    }

    public void DisableAds()
    {
        Data.IsNoAds = true;
        Save();
    }

    public void AddShopCardInfos(ShopCardInfos[] infos)
    {
        foreach (ShopCardInfos info in infos)
        {
            ShopData existingData = Data.ShopDatas.FirstOrDefault(shopData => shopData.EntityType == info.EntityType);

            if (existingData == null)
            {
                List<ShopCardItemButtonType> buttonTypes = info.CardInfos
                    .Select(shopCardInfo => shopCardInfo.Type)
                    .ToList();

                ShopData shopData = new()
                {
                    EntityType = info.EntityType,
                    ButtonTypes = buttonTypes,
                };

                Data.ShopDatas.Add(shopData);
            }
            else
            {
                existingData.ButtonTypes = info.CardInfos
                    .Select(shopCardInfo => shopCardInfo.Type)
                    .ToList();
            }
        }
    }

    public void SetShopCards(ShopEntityItemType entityType, IReadOnlyList<ShopCardItemButtonType> buttonTypes)
    {
        ShopData shopData = Data.ShopDatas.FirstOrDefault(shopData => shopData.EntityType == entityType);

        if (shopData == null)
        {
            shopData = new ShopData
            {
                EntityType = entityType,
                ButtonTypes = new List<ShopCardItemButtonType>(buttonTypes)
            };
            Data.ShopDatas.Add(shopData);
        }
        else
        {
            shopData.ButtonTypes = new List<ShopCardItemButtonType>(buttonTypes);
        }
    }

    public IReadOnlyList<ShopCardItemButtonType> GetShopButtonTypes(ShopEntityItemType entityType)
    {
        ShopData shopData = Data.ShopDatas.FirstOrDefault(shopData => shopData.EntityType == entityType);

        return shopData?.ButtonTypes ?? new List<ShopCardItemButtonType>();
    }

    public void Save() =>
        YG2.SaveProgress();

    public void ResetProgress()
    {
        Data.SelectedLevel = 0;
        Data.LevelProgress = 0;
        Data.Score = 0;
        Data.ShopDatas = new();

        foreach (LevelDataInfo level in Data.Levels)
            level.CountStars = 0;
    }

    public void ResetInUp()
    {
        Data.IsPurchsingRack = false;
        Data.IsNoAds = false;
    }

    private void EnterMissingData()
    {
        if (Data.Levels.Count == _totalLevelsCount)
            return;

        while (Data.Levels.Count < _totalLevelsCount)
            Data.Levels.Add(new LevelDataInfo());

        Save();
    }
}