using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class EnemyService : IDisposable
{
    private readonly EnemySpawner _spawner;
    private readonly EnemySpeedDirector _enemySpeedDirector;
    private readonly SplineContainer _splineConteiner;
    private readonly TypeColorRandomizer _colorRandomizer;

    public EnemyService(MonoBehaviour runner,
        Enemy prefab,
        List<Crowd> crowds,
        SplineContainer splineContainer,
        TypeColorRandomizer colorRandomizer,
        float gameSpeed)
    {
        if (splineContainer == null)
            throw new ArgumentNullException(nameof(splineContainer));

        _splineConteiner = splineContainer;
        _spawner = new(runner, prefab, crowds, splineContainer.transform, gameSpeed);
        _enemySpeedDirector = new(runner, gameSpeed);
        _colorRandomizer = colorRandomizer;

        _spawner.Spawned += OnEnemySpawned;
        _spawner.Created += OnEnemyCreated;
    }

    public event Action<Enemy> EnemySpawned;

    public void Dispose()
    {
        _spawner.Spawned -= OnEnemySpawned;
        _spawner.Created -= OnEnemyCreated;

        _spawner?.Dispose();
        _enemySpeedDirector?.Dispose();
    }

    public void Run()
    {
        _enemySpeedDirector.Run();
        _spawner.Run();
    }

    private void OnEnemySpawned(Enemy enemy)
    {
        if (_colorRandomizer.TryGetColor(enemy.Id, out Color color))
        {
            enemy.SetColor(color);
            _enemySpeedDirector.AddEnemy(enemy);

            EnemySpawned?.Invoke(enemy);
        }
    }

    private void OnEnemyCreated(Enemy enemy) =>
        enemy.Initialize(_splineConteiner);
}