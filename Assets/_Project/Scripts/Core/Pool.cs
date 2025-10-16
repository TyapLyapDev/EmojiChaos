using System.Collections.Generic;
using System;
using UnityEngine;

public class Pool<T> where T : MonoBehaviour, IPoolable<T>
{
    private const int MaximumSize = 300;

    private readonly T _prefab;
    private readonly Transform _parent;
    private readonly Action<T> _created;
    private readonly Stack<T> _elements = new();
    private readonly int _size;

    private int _count;

    public Pool(T prefab, Action<T> created, Transform parent, int size = MaximumSize)
    {
        _prefab = prefab ?? throw new ArgumentNullException(nameof(prefab));

        _created = created;
        _parent = parent;
        _size = size;
    }

    public bool TryGive(out T element)
    {
        element = null;

        if (_elements.Count == 0 && _count >= _size)
            return false;

        if (_elements.Count > 0)
            element = _elements.Pop();
        else
            element = Create();

        element.Deactivated += Return;

        return true;
    }

    public void Return(T element)
    {
        if (element == null)
            return;

        element.Deactivated -= Return;
        element.gameObject.SetActive(false);
        _elements.Push(element);
    }

    private T Create()
    {
        _count++;

        T element = UnityEngine.Object.Instantiate(_prefab, _parent);
        _created?.Invoke(element);

        return element;
    }
}