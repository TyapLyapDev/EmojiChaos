using System;
using System.Collections.Generic;
using System.Linq;

public class EnemyRegistryToAttack : IDisposable
{
    private readonly Dictionary<int, List<Enemy>> _availableEnemiesByType = new();
    private readonly EnemySpawner _spawner;

    public EnemyRegistryToAttack(EnemySpawner enemySpawner)
    {
        _spawner = enemySpawner ?? throw new ArgumentNullException(nameof(enemySpawner));

        _spawner.Spawned += OnEnemySpawned;
    }

    public void Dispose()
    {
        if (_spawner != null)
            _spawner.Spawned -= OnEnemySpawned;

        ClearAllEnemies();
    }

    public bool TryGiveEnemy(int enemyType, out Enemy enemy)
    {
        if (TryGetEnemiesList(enemyType, out List<Enemy> enemies) == false || enemies.Count == 0)
        {
            enemy = null;

            return false;
        }

        enemy = enemies.First();
        RemoveEnemy(enemy);

        return enemy.IsActive;
    }

    public void Scare(int enemyType, int count)
    {
        if (TryGetEnemiesList(enemyType, out List<Enemy> enemies) == false)
            return;

        List<Enemy> enemiesToScare = enemies.Take(count).ToList();

        foreach (var enemy in enemiesToScare)
            enemy.Scare();
    }

    private void ClearAllEnemies()
    {
        foreach (List<Enemy> enemies in _availableEnemiesByType.Values)
        {
            foreach (Enemy enemy in enemies)
                if (enemy != null)
                    enemy.Deactivated -= OnEnemyDeactivated;

            enemies.Clear();
        }

        _availableEnemiesByType.Clear();
    }

    private void RegisterEnemy(Enemy enemy)
    {
        if (enemy == null)
            throw new ArgumentNullException(nameof(enemy));

        if (enemy.IsActive == false)
            throw new InvalidOperationException($"{nameof(enemy)} должен быть активным при регистрации");

        List<Enemy> enemies = GetOrCreateEnemiesList(enemy.Type);

        if (enemies.Contains(enemy))
            throw new InvalidOperationException($"{nameof(enemy)} уже был зарегистрирован");

        enemies.Add(enemy);
        enemy.Deactivated += OnEnemyDeactivated;
    }

    private void RemoveEnemy(Enemy enemy)
    {
        if (enemy == null) 
            return;

        if (TryGetEnemiesList(enemy.Type, out List<Enemy> enemies) == false)
            return;

        enemy.Deactivated -= OnEnemyDeactivated;
        enemies.Remove(enemy);

        if (enemies.Count == 0)
            _availableEnemiesByType.Remove(enemy.Type);
    }

    private List<Enemy> GetOrCreateEnemiesList(int enemyType)
    {
        if (_availableEnemiesByType.TryGetValue(enemyType, out List<Enemy> enemies) == false)
        {
            enemies = new();
            _availableEnemiesByType[enemyType] = enemies;
        }

        return enemies;
    }

    private bool TryGetEnemiesList(int enemyType, out List<Enemy> enemies) =>
        _availableEnemiesByType.TryGetValue(enemyType, out enemies);

    private void OnEnemySpawned(Enemy enemy) =>
        RegisterEnemy(enemy);

    private void OnEnemyDeactivated(Enemy enemy) =>
        RemoveEnemy(enemy);
}