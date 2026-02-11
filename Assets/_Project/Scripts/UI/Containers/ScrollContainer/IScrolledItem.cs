using UnityEngine;

namespace UI.CustomScrollContainer
{
    public interface IScrolledItem 
    { 
        Transform Transform { get; }

        RectTransform Center { get; }
    }
}