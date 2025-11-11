using System;
using UnityEngine;

public class PoolBuilder
{
    private const string PoolSuffix = "Pool";

    private readonly Transform _registryParent;

    public PoolBuilder(Transform registryParent)
    {
        if (registryParent == null)
            throw new ArgumentNullException(nameof(registryParent));

        _registryParent = registryParent;
    }

    public Pool<T> Build<T>(T prefab, string poolName) where T : MonoBehaviour, IPoolable<T>
    {
        if (prefab == null)
            throw new ArgumentNullException(nameof(prefab));

        Transform poolParent = new GameObject($"{poolName}{PoolSuffix}").transform;
        poolParent.SetParent(_registryParent);
        Factory<T> factory = new(prefab);
        Pool<T> pool = new(factory, poolParent);

        return pool;
    }
}