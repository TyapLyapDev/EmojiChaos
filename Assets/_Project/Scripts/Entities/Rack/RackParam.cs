using System;

public readonly struct RackParam : IParam
{
    private readonly Gun _gun;
    private readonly Saver _saver;

    public RackParam(Gun gun, Saver saver)
    {
        _gun = gun != null ? gun : throw new ArgumentNullException(nameof(gun));
        _saver = saver ?? throw new ArgumentNullException(nameof(saver));
    }

    public Gun Gun => _gun;

    public Saver Saver => _saver;
}