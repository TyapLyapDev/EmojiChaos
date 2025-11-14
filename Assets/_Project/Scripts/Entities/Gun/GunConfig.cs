using System;

public readonly struct GunConfig : IConfig
{
    private readonly Shooter _shooter;
    private readonly ParticleShower _particleShower;

    public GunConfig(Shooter shooter, ParticleShower particleShower)
    {
        _shooter = shooter ?? throw new ArgumentNullException(nameof(shooter));
        _particleShower = particleShower ?? throw new ArgumentNullException(nameof(particleShower));
    }

    public Shooter Shooter => _shooter;

    public ParticleShower ParticleShower => _particleShower;
}