using System;

public readonly struct LevelUiConfig : IConfig
{
    private readonly PauseSwitcher _pauseSwitcher;
    private readonly Saver _saver;
    private readonly LevelStatsHandler _levelStatsHandler;
    private readonly SceneLoader _sceneLoader;

    public LevelUiConfig(PauseSwitcher pauseSwitcher, 
        Saver saver,
        LevelStatsHandler levelStatsHandler, 
        SceneLoader sceneLoader)
    {
        _pauseSwitcher = pauseSwitcher ?? throw new ArgumentNullException(nameof(pauseSwitcher));
        _saver = saver ?? throw new ArgumentNullException(nameof(saver));
        _levelStatsHandler = levelStatsHandler ?? throw new ArgumentNullException(nameof(levelStatsHandler));
        _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
    }

    public PauseSwitcher PauseSwitcher => _pauseSwitcher;

    public Saver Saver => _saver;

    public LevelStatsHandler LevelStatsHandler => _levelStatsHandler;

    public SceneLoader SceneLoader => _sceneLoader;
}