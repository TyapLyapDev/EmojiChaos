using System;
using UnityEngine;

public abstract class BaseFactory<T> : IFactory<T> where T : MonoBehaviour
{
    private readonly T _prefab;

    protected BaseFactory(T prefab)
    {
        _prefab = prefab ?? throw new ArgumentNullException(nameof(prefab));
    }

    public T Create()
    {
        T element = UnityEngine.Object.Instantiate(_prefab);
        OnCreate(element);

        return element;
    }

    protected abstract void OnCreate(T element);
}