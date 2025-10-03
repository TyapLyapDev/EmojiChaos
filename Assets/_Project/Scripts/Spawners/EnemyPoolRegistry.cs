using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolRegistry
{
    private const int DefaultPoolSize = 300;

    private readonly Transform _poolParent;

    private readonly Dictionary<int, Pool<Enemy>> _crowdPools = new();

    public EnemyPoolRegistry(List<Enemy> enemyPrefabs, Transform parent, List<int> uniqueCrowdIds)
    {
        if (enemyPrefabs == null || enemyPrefabs.Count == 0)
            throw new ArgumentException("Список вражеских префабов не может быть пустым", nameof(enemyPrefabs));

        if (uniqueCrowdIds == null || uniqueCrowdIds.Count == 0)
            throw new ArgumentException("Список уникальных идентификаторов толп не может быть пустым", nameof(uniqueCrowdIds));

        if (enemyPrefabs.Count < uniqueCrowdIds.Count)
            throw new ArgumentException($"Количество вражеских префабов ({enemyPrefabs.Count}) должно быть не меньше количества уникальных идентификаторов толпы ({uniqueCrowdIds.Count})");

        _poolParent = parent;
        AssignPoolsToCrowds(enemyPrefabs, uniqueCrowdIds);
    }

    public event Action<Enemy> EnemyCreated;

    public bool TryGetEnemy(int crowdId, out Enemy enemy)
    {
        enemy = null;

        return _crowdPools.TryGetValue(crowdId, out Pool<Enemy> pool) && pool.TryGet(out enemy);
    }

    private void AssignPoolsToCrowds(List<Enemy> enemyPrefabs, List<int> uniqueCrowdIds)
    {
        List<Enemy> availablePrefabs = new(enemyPrefabs);

        foreach (int crowdId in uniqueCrowdIds)
        {
            if (availablePrefabs.Count == 0)
                availablePrefabs = new(enemyPrefabs);

            int randomIndex = UnityEngine.Random.Range(0, availablePrefabs.Count);
            Enemy selectedPrefab = availablePrefabs[randomIndex];

            _crowdPools[crowdId] = new Pool<Enemy>(selectedPrefab, OnEnemyCreated, _poolParent, DefaultPoolSize);

            availablePrefabs.RemoveAt(randomIndex);
        }
    }

    private void OnEnemyCreated(Enemy enemy) =>
        EnemyCreated?.Invoke(enemy);
}