using System;

public readonly struct MenuUiConfig : IConfig
{
    private readonly Saver _saver;
    private readonly SceneLoader _sceneLoader;

    public MenuUiConfig(Saver saver, SceneLoader sceneLoader)
    {
        _saver = saver ?? throw new ArgumentNullException(nameof(saver));
        _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
    }

    public Saver Saver => _saver;

    public SceneLoader SceneLoader => _sceneLoader;
}