using UnityEngine;

namespace EmojiChaos.UI.Containers.ScrollContainer
{
    public interface IScrolledItem
    {
        Transform Transform { get; }

        RectTransform Center { get; }
    }
}