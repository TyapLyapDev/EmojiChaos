using System;
using System.Collections.Generic;

public class ServicesRegistry
{
    private readonly Dictionary<Type, object> _services = new();
    private readonly List<IDisposable> _disposables = new();

    public void Add<T>(T service) where T : class
    {
        Type type = typeof(T);

        if (_services.ContainsKey(type))
            throw new InvalidOperationException($"Service {type.Name} already registered");

        _services[typeof(T)] = service;

        if (service is IDisposable disposable)
            _disposables.Add(disposable);
    }

    public T Get<T>() where T : class
    {
        if (_services.TryGetValue(typeof(T), out var service))
            return (T)service;

        throw new InvalidOperationException($"Service {typeof(T).Name} not registered");
    }

    public void Dispose()
    {
        for (int i = _disposables.Count - 1; i >= 0; i--)
            _disposables[i]?.Dispose();

        _disposables.Clear();
        _services.Clear();
    }
}