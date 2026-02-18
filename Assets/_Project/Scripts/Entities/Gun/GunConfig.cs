using System;

public readonly struct GunConfig : IConfig
{
    private readonly Shooter _shooter;
    private readonly ParticleShower _particleShower;
    private readonly float _timeReload;

    public GunConfig(Shooter shooter, ParticleShower particleShower, float timeReload)
    {
        _shooter = shooter ?? throw new ArgumentNullException(nameof(shooter));
        _particleShower = particleShower ?? throw new ArgumentNullException(nameof(particleShower));

        if (timeReload <= 0)
            throw new ArgumentOutOfRangeException(nameof(timeReload), timeReload, "Значение должно быть больше нуля");

        _timeReload = timeReload;
    }

    public Shooter Shooter => _shooter;

    public ParticleShower ParticleShower => _particleShower;

    public float TimeReload => _timeReload;
}