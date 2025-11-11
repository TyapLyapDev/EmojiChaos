using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolRegistry : IDisposable
{
    private readonly Dictionary<Type, object> _pools = new();
    private readonly PoolBuilder _poolBuilder;

    public PoolRegistry(PoolBuilder poolBuilder)
    {
        _poolBuilder = poolBuilder ?? throw new ArgumentNullException(nameof(poolBuilder));
    }

    public void Dispose()
    {
        foreach (object pool in _pools.Values)
            if (pool is IDisposable disposablePool)
                disposablePool.Dispose();

        _pools.Clear();
    }

    public Pool<T> CreatePool<T>(T prefab, string poolName) where T : MonoBehaviour, IPoolable<T>
    {
        Pool<T> pool = _poolBuilder.Build(prefab, poolName);
        RegisterPool(pool);

        return pool;
    }

    public void RegisterPool<T>(Pool<T> pool) where T : MonoBehaviour, IPoolable<T>
    {
        Type poolType = typeof(T);

        if (_pools.ContainsKey(poolType))
            Debug.LogWarning($"Pool for type {poolType.Name} is already registered. Overwriting...");

        _pools[poolType] = pool ?? throw new ArgumentNullException(nameof(pool));
    }

    public Pool<T> GetPool<T>() where T : MonoBehaviour, IPoolable<T>
    {
        Type poolType = typeof(T);

        if (_pools.TryGetValue(poolType, out object pool))
            return (Pool<T>)pool;

        throw new InvalidOperationException($"Pool for type {poolType.Name} is not registered!");
    }
}