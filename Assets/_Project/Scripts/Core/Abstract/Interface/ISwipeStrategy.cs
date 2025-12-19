using System;

public interface ISwipeStrategy : IDisposable
{
    event Action<ISwipeable, int> HasSwipe;

    void Pause();
    void Resume();
}