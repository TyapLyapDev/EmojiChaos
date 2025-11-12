using System;
using UnityEngine.Splines;

public class EnemyConfig : IConfig
{
    private readonly SplineContainer _splineContainer;
    private readonly ParticleShower _particleShower;

    public EnemyConfig(SplineContainer splineContainer, ParticleShower particleShower)
    {
        _splineContainer = splineContainer != null ? splineContainer : throw new ArgumentNullException(nameof(splineContainer));
        _particleShower = particleShower ?? throw new ArgumentNullException(nameof(particleShower));
    }

    public SplineContainer SplineContainer => _splineContainer;

    public ParticleShower ParticleShower => _particleShower;
}