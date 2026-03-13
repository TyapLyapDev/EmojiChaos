using System;
using EmojiChaos.Core.Abstract.Interface;

namespace EmojiChaos.Services.Input
{
    public interface ISwipeStrategy : IDisposable
    {
        event Action<ISwipeable, int> HasSwipe;

        void Pause();

        void Resume();
    }
}