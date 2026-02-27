using System;
using UnityEngine;

public class PoolBuilder
{
    private const string PoolParentName = "Pools";
    private const string SecondaryPoolSuffix = "Pool";

    private readonly Transform _registryParent;

    public PoolBuilder()
    {
        _registryParent = new GameObject(PoolParentName).transform;
    }

    public Pool<TBehaviour> Build<TBehaviour>(TBehaviour prefab) where TBehaviour : InitializingBehaviour, IPoolable<TBehaviour>
    {
        Factory<TBehaviour> factory = new(prefab);
        Pool<TBehaviour> pool = Build(factory);

        return pool;
    }

    public Pool<TBehaviour> Build<TBehaviour, TConfig>(TBehaviour prefab, TConfig config) 
        where TBehaviour : InitializingWithConfigBehaviour<TConfig>, IPoolable<TBehaviour>
        where TConfig : IParam
    {
        FactoryWithParam<TBehaviour, TConfig> factory = new(prefab, config);
        Pool<TBehaviour> pool = Build(factory);

        return pool;
    }

    private Pool<T> Build<T>(IFactory<T> factory) where T : MonoBehaviour, IPoolable<T>
    {
        if (factory == null)
            throw new ArgumentNullException(nameof(factory));

        string poolName = $"{typeof(T).Name}{SecondaryPoolSuffix}";
        Transform poolParent = new GameObject($"{poolName}{SecondaryPoolSuffix}").transform;
        poolParent.SetParent(_registryParent);
        Pool<T> pool = new(factory, poolParent);

        return pool;
    }
}