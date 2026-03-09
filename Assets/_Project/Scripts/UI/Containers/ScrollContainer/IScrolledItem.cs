using UnityEngine;

namespace EmojiChaos.UI.Containers.ScrollContainer
{

namespace UI.CustomScrollContainer
{
    public interface IScrolledItem 
    { 
        Transform Transform { get; }

        RectTransform Center { get; }
    }
}
}
