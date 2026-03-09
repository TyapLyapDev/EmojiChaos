using EmojiChaos.Core.Abstract.Interface;
using System;

namespace EmojiChaos.Services.Input
{

public interface ISwipeStrategy : IDisposable
{
    event Action<ISwipeable, int> HasSwipe;

    void Pause();

    void Resume();
}
}
