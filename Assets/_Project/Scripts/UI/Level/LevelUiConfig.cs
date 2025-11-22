using System;

public readonly struct LevelUiConfig : IConfig
{
    private readonly PauseSwitcher _pauseSwitcher;
    private readonly SceneLoader _sceneLoader;

    public LevelUiConfig(PauseSwitcher pauseSwitcher, SceneLoader sceneLoader)
    {
        _pauseSwitcher = pauseSwitcher ?? throw new ArgumentNullException(nameof(pauseSwitcher));
        _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
    }

    public PauseSwitcher PauseSwitcher => _pauseSwitcher;

    public SceneLoader SceneLoader => _sceneLoader;
}