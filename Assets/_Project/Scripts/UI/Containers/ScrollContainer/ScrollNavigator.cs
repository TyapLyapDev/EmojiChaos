using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CustomScrollContainer
{
    public class ScrollNavigator
    {
        private readonly ScrollRect _scroll;
        private readonly RectTransform _viewport;
        private readonly RectTransform _content;
        private readonly ItemContainer _container;

        public ScrollNavigator(ScrollRect scroll, RectTransform viewport, RectTransform content, ItemContainer itemContainer)
        {
            _scroll = scroll != null ? scroll : throw new ArgumentNullException(nameof(scroll));
            _viewport = viewport != null ? viewport : throw new ArgumentNullException(nameof(viewport));
            _content = content != null ? content : throw new ArgumentNullException(nameof(content));
            _container = itemContainer ?? throw new ArgumentNullException(nameof(itemContainer));
        }

        public void AlignByItem(int index)
        {
            IReadOnlyList<IScrolledItem> items = _container.Items;
            index = Mathf.Clamp(index, 0, items.Count - 1);

            LayoutRebuilder.ForceRebuildLayoutImmediate(_content);
            Canvas.ForceUpdateCanvases();
            RectTransform elementRect = items[index].Center;
            float normalizedPosition = GetNormalizedHorizontalPositionOnLayout(elementRect);
            _scroll.horizontalNormalizedPosition = normalizedPosition;
            _scroll.velocity = Vector2.zero;
        }

        private float GetNormalizedHorizontalPositionOnLayout(RectTransform element)
        {
            if (_content == null || _viewport == null)
                return 0f;

            Bounds elementBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(_content, element);
            float viewportWidth = _viewport.rect.width;
            float contentWidth = _content.rect.width;
            float targetViewportCenterX = elementBounds.center.x;
            float targetContentPosX = targetViewportCenterX - viewportWidth / 2f;
            float maxScrollDistance = Mathf.Max(0f, contentWidth - viewportWidth);
            float normalized = Mathf.Clamp01(targetContentPosX / maxScrollDistance);

            return normalized;
        }
    }
}