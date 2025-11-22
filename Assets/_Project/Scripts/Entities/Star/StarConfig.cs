using System;

public readonly struct StarConfig : IConfig
{
    private readonly EnemySpeedDirector _enemySpeedDirector;
    private readonly ParticleShower _particleShower;
    private readonly CameraShaker _cameraShaker;

    public StarConfig(EnemySpeedDirector enemySpeedDirector, ParticleShower particleShower, CameraShaker cameraShaker)
    {
        _enemySpeedDirector = enemySpeedDirector != null ? enemySpeedDirector : throw new ArgumentNullException(nameof(enemySpeedDirector));
        _particleShower = particleShower ?? throw new ArgumentNullException(nameof(particleShower));
        _cameraShaker = cameraShaker ?? throw new ArgumentNullException(nameof(cameraShaker));
    }

    public EnemySpeedDirector EnemySpeedDirector => _enemySpeedDirector;

    public ParticleShower ParticleShower => _particleShower;

    public CameraShaker CameraShaker => _cameraShaker;
}