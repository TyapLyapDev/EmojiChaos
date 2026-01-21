using System;
using System.Collections.Generic;

public class EnemiesCounter : IDisposable
{
    private readonly CrowdSpawnCoordinator _crowdSpawnCoordinator;
    private readonly List<Enemy> _enemies = new();
    private bool _isDisposed;

    public EnemiesCounter(CrowdSpawnCoordinator crowdSpawnCoordinator)
    {
        _crowdSpawnCoordinator = crowdSpawnCoordinator ?? throw new ArgumentNullException(nameof(crowdSpawnCoordinator));

        Subscribe();
    }

    public event Action<Enemy> Killed;
    public event Action<int> EnemyCountChanged;
    public event Action AllEnemiesDefeated;

    public int EnemyCount => _enemies.Count;

    public bool HasEnemies => _enemies.Count > 0;

    public bool IsSpawningComplete => _crowdSpawnCoordinator.IsSpawning == false;

    public void Dispose()
    {
        if (_isDisposed)
            return;

        Unsubscribe();
        _enemies.Clear();
        _isDisposed = true;
    }

    private void Subscribe()
    {
        _crowdSpawnCoordinator.EnemySpawned += OnEnemySpawned;
        _crowdSpawnCoordinator.Completed += OnSpawnCompleted;
    }

    private void Unsubscribe()
    {
        if (_crowdSpawnCoordinator != null)
        {
            _crowdSpawnCoordinator.EnemySpawned -= OnEnemySpawned;
            _crowdSpawnCoordinator.Completed -= OnSpawnCompleted;
        }

        foreach (Enemy enemy in _enemies)
        {
            if (enemy != null)
                enemy.Deactivated -= OnEnemyDeactivated;
        }
    }

    private void ProcessDiedEnemy(Enemy enemy)
    {
        if (enemy == null)
            throw new ArgumentNullException(nameof(enemy));

        enemy.Deactivated -= OnEnemyDeactivated;
        enemy.Killed -= OnEnemyKilled;
        _enemies.Remove(enemy);

        EnemyCountChanged?.Invoke(_enemies.Count);

        if (_enemies.Count == 0 && IsSpawningComplete)
            AllEnemiesDefeated?.Invoke();
    }

    private void OnEnemySpawned(Enemy enemy)
    {
        if (enemy == null)
            throw new ArgumentNullException(nameof(enemy));

        _enemies.Add(enemy);
        enemy.Deactivated += OnEnemyDeactivated;
        enemy.Killed += OnEnemyKilled;

        EnemyCountChanged?.Invoke(_enemies.Count);
    }

    private void OnEnemyDeactivated(Enemy enemy) =>
        ProcessDiedEnemy(enemy);

    private void OnSpawnCompleted()
    {
        if (_enemies.Count == 0)
            AllEnemiesDefeated?.Invoke();
    }

    private void OnEnemyKilled(Enemy enemy)
    {
        if (enemy == null)
            throw new ArgumentNullException(nameof(enemy));

        ProcessDiedEnemy(enemy);

        Killed?.Invoke(enemy);
    }
}