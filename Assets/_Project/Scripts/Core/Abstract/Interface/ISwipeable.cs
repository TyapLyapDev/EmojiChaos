using UnityEngine;

namespace EmojiChaos.Core.Abstract.Interface
{
    public interface ISwipeable : IClickable
    {
        Transform Transform { get; }
    }
}