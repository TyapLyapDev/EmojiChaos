using System;
using System.Collections.Generic;

public class StarsCounter : IDisposable
{
    private readonly List<Star> _stars = new();

    public StarsCounter(IReadOnlyList<Star> stars)
    {
        _stars = new List<Star>(stars) ?? throw new ArgumentNullException(nameof(stars));

        foreach (Star star in _stars)
            if(star == null)
                throw new ArgumentNullException(nameof(star));

        Subscribe();
    }

    public event Action<int> StarCountChanged;
    public event Action StarsLeft;

    public int StarCount => _stars.Count;

    public IReadOnlyList<Star> Stars => _stars;

    public void Dispose()
    {
        Unsubscribe();
        _stars.Clear();
    }

    private void Subscribe()
    {
        foreach (Star star in _stars)
            if (star != null)
                star.Destroyed += OnStarDestroyed;
    }

    private void Unsubscribe()
    {
        foreach (Star star in _stars)
            if (star != null)
                star.Destroyed -= OnStarDestroyed;
    }

    private void OnStarDestroyed(Star star)
    {
        if (star == null) 
            return;

        star.Destroyed -= OnStarDestroyed;
        _stars.Remove(star);

        StarCountChanged?.Invoke(_stars.Count);

        if (_stars.Count == 0)
            StarsLeft?.Invoke();
    }
}
