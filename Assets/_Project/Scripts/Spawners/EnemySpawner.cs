using System;
using UnityEngine;
using UnityEngine.Splines;

public class EnemySpawner : IDisposable
{
    private readonly SplineContainer _splineContainer;
    private readonly EnemyPoolRegistry _poolRegistry;
    private readonly WaveSequencer _waveSequencer;
    private readonly SpawnCoordinator _spawnCoordinator;

    public event Action<Enemy> Spawned;

    public EnemySpawner(MonoBehaviour coroutineRunner, LevelConfigurations levelConfig, SplineContainer splineContainer)
    {
        if (levelConfig == null)
            throw new ArgumentNullException(nameof(levelConfig));

        if (splineContainer == null)
            throw new ArgumentNullException(nameof(splineContainer));

        _splineContainer = splineContainer;
        _poolRegistry = new(levelConfig.EnemyPrefabs, _splineContainer.transform, levelConfig.GetUniqueCrowdIds());
        _waveSequencer = new(levelConfig.GetCrowdSequence());
        _spawnCoordinator = new(coroutineRunner);

        _poolRegistry.EnemyCreated += OnEnemyCreated;
        _spawnCoordinator.EnemySpawned += OnEnemySpawned;
    }

    public void Dispose()
    {
        StopSpawning();

        if (_spawnCoordinator != null)
            _spawnCoordinator.EnemySpawned -= OnEnemySpawned;

        if (_poolRegistry != null)
            _poolRegistry.EnemyCreated -= OnEnemyCreated;
    }

    public void StartSpawning(float gameSpeed) =>
        _spawnCoordinator.StartSpawningSequence(_waveSequencer, _poolRegistry, gameSpeed);

    public void StopSpawning() =>
        _spawnCoordinator.StopSpawningSequence();

    private void OnEnemySpawned(Enemy enemy) =>
        Spawned?.Invoke(enemy);

    private void OnEnemyCreated(Enemy enemy) =>
        enemy.Initialize(_splineContainer);
}