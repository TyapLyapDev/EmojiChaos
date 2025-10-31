using System;

public interface ISwipeStrategy
{
    event Action<ISwipeable, int> HasSwipe;
}