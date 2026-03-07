using System;

namespace EmojiChaos.Core.Abstract.Interface
{
    public interface IDeactivatable<T>
    {
        public event Action<T> Deactivated;

        void Deactivate();
    }
}