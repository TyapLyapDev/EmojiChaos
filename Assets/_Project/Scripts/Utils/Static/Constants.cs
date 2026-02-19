public static class Constants
{
    public const string LevelsPath = "Levels";
    public const string TutorialPath = "Tutorial";
    public const string LevelSceneName = "Level";
    public const string MenuSceneName = "Menu";
    public const string MusicGroup = nameof(MusicGroup);
    public const string SfxGroup = nameof(SfxGroup);

    public const string LangRu = "ru";
    public const string LangTr = "tr";
    public const string LangEn = "en";

    public const string RewardRack = nameof(RewardRack);

#if UNITY_EDITOR
    public const string LeaderboardTechnoName = "test";    
#else
    public const string LeaderboardTechnoName = "Score";
#endif
}