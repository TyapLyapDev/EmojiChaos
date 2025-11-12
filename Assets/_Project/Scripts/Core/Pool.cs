using System;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : MonoBehaviour, IPoolable<T>
{
    private const int MaximumSize = 500;

    private readonly IFactory<T> _factory;
    private readonly Transform _parent;
    private readonly Stack<T> _elements = new();
    private readonly int _size;

    private int _count;

    public Pool(IFactory<T> factory, Transform parent, int size = MaximumSize)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));

        _parent = parent;
        _size = size;
    }

    public bool TryGive(out T element)
    {
        element = null;

        if (_elements.Count == 0 && _count >= _size)
            return false;

        element = _elements.Count > 0 ? _elements.Pop() : Create();
        element.Deactivated += Return;

        return true;
    }

    public void Return(T element)
    {
        if (element == null)
            return;

        element.Deactivated -= Return;
        element.SetActive(false);
        _elements.Push(element);
    }

    private T Create()
    {
        _count++;

        T element = _factory.Create();
        element.SetParent(_parent);

        return element;
    }
}