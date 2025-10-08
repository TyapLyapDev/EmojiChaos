using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : MonoBehaviour, IPoolable<T>
{
    private const int MaximumSize = 300;

    private readonly T _prefab;
    private readonly System.Action<T> _created;
    private readonly Transform _parent;
    private readonly Stack<T> _elements = new();
    private readonly int _size;

    private int _count;

    public Pool(T prefab, System.Action<T> created, Transform parent, int size = MaximumSize)
    {
        if(prefab == null)
            throw new System.ArgumentNullException(nameof(prefab));

        _prefab = prefab;
        _created = created;
        _parent = parent;
        _size = size;
    }

    public bool TryGet(out T element)
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

    private void Return(T element)
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

        T element = Object.Instantiate(_prefab, _parent);
        _created?.Invoke(element);

        return element;
    }
}