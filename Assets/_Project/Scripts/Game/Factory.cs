using System;
using UnityEngine;

public class Factory<T> : IFactory<T> where T : MonoBehaviour
{
    private readonly T _prefab;

    public Factory(T prefab)
    {
        _prefab = prefab ?? throw new ArgumentNullException(nameof(prefab));
    }

    public T Create() =>
        UnityEngine.Object.Instantiate(_prefab);
}