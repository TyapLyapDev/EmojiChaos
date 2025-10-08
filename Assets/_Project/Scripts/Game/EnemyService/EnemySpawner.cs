using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : IDisposable
{
    private readonly EnemySpawnCoordinator _spawnCoordinator;
    private readonly Pool<Enemy> _pool;
    private readonly WaveSequencer _waveSequencer;
    private readonly float _gameSpeed;

    public event Action<Enemy> Spawned;
    public event Action<Enemy> Created;

    public EnemySpawner(MonoBehaviour runner, Enemy prefab, List<Crowd> crowdSequence, Transform parent, float gameSpeed)
    {
        if (gameSpeed <= 0)
            throw new ArgumentOutOfRangeException(nameof(gameSpeed), "Значение должно быть больше нуля");

        _spawnCoordinator = new(runner);
        _pool = new(prefab, OnEnemyCreated, parent);
        _waveSequencer = new(crowdSequence);
        _gameSpeed = gameSpeed;

        _spawnCoordinator.Spawned += OnEnemySpawned;
    }

    public void Dispose()
    {
        StopSpawning();

        if (_spawnCoordinator != null)
            _spawnCoordinator.Spawned -= OnEnemySpawned;
    }

    public void Run() =>
        _spawnCoordinator.StartSpawningSequence(_waveSequencer, _pool, _gameSpeed);

    public void StopSpawning() =>
        _spawnCoordinator.StopSpawningSequence();

    private void OnEnemySpawned(Enemy enemy) =>
        Spawned?.Invoke(enemy);

    private void OnEnemyCreated(Enemy enemy) =>
        Created?.Invoke(enemy);
}