using System;
using UnityEngine.Splines;

public class EnemyInitializer : IDisposable
{
    private readonly Pool<Enemy> _pool;
    private readonly SplineContainer _splineContainer;
    private readonly ParticleShower _particleShower;

    public EnemyInitializer(Pool<Enemy> pool, SplineContainer splineContainer, ParticleShower particleShower)
    {
        _pool = pool ?? throw new ArgumentNullException(nameof(pool));
        _splineContainer = splineContainer != null ? splineContainer : throw new ArgumentNullException(nameof(splineContainer));
        _particleShower = particleShower ?? throw new ArgumentNullException(nameof(particleShower));

        _pool.Created += OnEnemyCreated;
    }

    public void Dispose()
    {
        if (_pool != null)
            _pool.Created -= OnEnemyCreated;
    }

    private void OnEnemyCreated(Enemy enemy)
    {
        if (enemy == null)
            throw new ArgumentNullException(nameof(enemy));

        enemy.Initialize(_splineContainer, _particleShower);
    }
}