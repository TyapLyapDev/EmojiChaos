using System;

public readonly struct LevelBoxContainerConfig : IConfig
{
    private readonly Saver _saver;
    
    public LevelBoxContainerConfig(Saver saver)
    {
        _saver = saver ?? throw new ArgumentOutOfRangeException(nameof(saver));
    }

    public Saver Saver => _saver;
}