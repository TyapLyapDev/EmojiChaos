using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class EnemyService : IDisposable
{
    private readonly CrowdSpawnCoordinator _crowdSpawnCoordinator;
    private readonly EnemySpawner _enemySpawner;
    private readonly EnemySpeedDirector _enemySpeedDirector;
    private readonly SplineContainer _splineConteiner;

    public EnemyService(MonoBehaviour runner,
        Enemy prefab,
        List<Crowd> crowds,
        SplineContainer splineContainer,
        TypeColorRandomizer colorRandomizer,
        float gameSpeed)
    {
        if (splineContainer == null)
            throw new ArgumentNullException(nameof(splineContainer));

        Pool<Enemy> pool = new(prefab, OnEnemyCreated, splineContainer.transform);
        _enemySpawner = new(pool, colorRandomizer, gameSpeed);
        _splineConteiner = splineContainer;
        _crowdSpawnCoordinator = new(runner, _enemySpawner, crowds);
        _enemySpeedDirector = new(gameSpeed);

        _enemySpawner.Spawned += OnEnemySpawned;
    }

    public event Action<Enemy> EnemySpawned;

    public void Dispose()
    {
        _enemySpawner.Spawned -= OnEnemySpawned;
        _crowdSpawnCoordinator?.Dispose();
        _enemySpeedDirector?.Dispose();
    }

    public void StartRunning()
    {
        _enemySpeedDirector.StartRun();
        _crowdSpawnCoordinator.StartRunning();
    }

    private void OnEnemySpawned(Enemy enemy)
    {
        _enemySpeedDirector.RegisterEnemy(enemy);
        EnemySpawned?.Invoke(enemy);
    }

    private void OnEnemyCreated(Enemy enemy) =>
        enemy.Initialize(_splineConteiner);
}