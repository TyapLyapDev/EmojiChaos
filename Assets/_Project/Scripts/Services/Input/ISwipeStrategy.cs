using System;

namespace EmojiChaos.Services.Input
{
    using EmojiChaos.Core.Abstract.Interface;

    public interface ISwipeStrategy : IDisposable
    {
        event Action<ISwipeable, int> HasSwipe;

        void Pause();

        void Resume();
    }
}