using System;
using System.Collections.Generic;

[Serializable]
public class SavesData
{
    public int SelectedLevel = 0;
    public int LevelProgress = 0;
    public int Score = 0;
    public float MusicVolume = 0.3f;
    public float SfxVolume = 0.7f;
    public List<LevelDataInfo> Levels = new();
    public bool IsPurchsingRack = false;
    public bool IsNoAds = false;
    public bool ShowedAuthDialog = false;
    public List<ShopData> ShopDatas = new();
}

[Serializable]
public class ShopData
{
    public ShopEntityItemType EntityType;
    public List<ShopCardItemButtonType> ButtonTypes = new();
}