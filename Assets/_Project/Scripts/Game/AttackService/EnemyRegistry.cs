using System;
using System.Collections.Generic;
using System.Linq;

public class EnemyRegistry : IDisposable
{
    private readonly Dictionary<int, List<Enemy>> _availableEnemiesByType = new();

    public void Dispose()
    {
        Dictionary<int, List<Enemy>> enemiesByTypeCopy = new(_availableEnemiesByType);

        foreach (var kvp in enemiesByTypeCopy)
        {
            foreach (Enemy enemy in kvp.Value)
                if (enemy != null)
                    enemy.Deactivated -= OnEnemyDeactivated;

            kvp.Value.Clear();
        }

        _availableEnemiesByType.Clear();
    }

    public void RegisterEnemy(Enemy enemy)
    {
        if (enemy == null)
            throw new ArgumentNullException(nameof(enemy));

        int enemyType = enemy.Type;

        if (_availableEnemiesByType.ContainsKey(enemyType) == false)
            _availableEnemiesByType[enemyType] = new();

        if (enemy.IsActive == false)
            throw new InvalidOperationException($"{nameof(enemy)} должен быть активным при регистрации");

        if (_availableEnemiesByType[enemyType].Contains(enemy))
            throw new InvalidOperationException($"{nameof(enemy)} уже был зарегистрирован");

        _availableEnemiesByType[enemyType].Add(enemy);
        enemy.Deactivated += OnEnemyDeactivated;
    }

    public bool TryGiveEnemy(int enemyType, out Enemy enemy)
    {
        enemy = null;

        if (_availableEnemiesByType.ContainsKey(enemyType) == false)
            return false;

        List<Enemy> enemies = _availableEnemiesByType[enemyType];

        if (enemies.Count == 0)
            return false;

        enemy = enemies.First();
        enemies.RemoveAt(0);

        if (enemies.Count == 0)
            _availableEnemiesByType.Remove(enemyType);

        if (enemy != null)
            enemy.Deactivated -= OnEnemyDeactivated;

        return enemy != null && enemy.IsActive;
    }

    private void OnEnemyDeactivated(Enemy enemy)
    {
        if (enemy == null)
            throw new ArgumentNullException(nameof(enemy));

        int enemyType = enemy.Type;

        if (_availableEnemiesByType.ContainsKey(enemyType) == false)
            return;

        List<Enemy> enemies = _availableEnemiesByType[enemyType];
        enemy.Deactivated -= OnEnemyDeactivated;
        enemies.Remove(enemy);

        if (enemies.Count == 0)
            _availableEnemiesByType.Remove(enemyType);
    }
}