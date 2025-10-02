using System;

public interface IPoolable<T>
{
    public event Action<T> Deactivated;

    void Deactivate();
}