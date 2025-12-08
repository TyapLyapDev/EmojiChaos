using System;

public readonly struct MenuUiConfig : IConfig
{
    private readonly Saver _saver;

    public MenuUiConfig(Saver saver)
    {
        _saver = saver ?? throw new ArgumentNullException(nameof(saver));
    }

    public Saver Saver => _saver;
}