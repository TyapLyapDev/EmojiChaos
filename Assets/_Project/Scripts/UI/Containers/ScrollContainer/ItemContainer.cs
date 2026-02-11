using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.CustomScrollContainer
{
    public class ItemContainer
    {
        private readonly List<IScrolledItem> _items = new();
        private readonly Transform _content;

        public ItemContainer(Transform content)
        {
            _content = content != null ? content : throw new ArgumentNullException(nameof(content));
        }

        public IReadOnlyList<IScrolledItem> Items => _items;

        public void ClearContent()
        {
            _items.Clear();

            foreach (Transform child in _content)
                UnityEngine.Object.Destroy(child.gameObject);
        }

        public void AddItem(IScrolledItem item)
        {
            Transform itemTransform = item.Transform;
            Vector3 scale = itemTransform.localScale;
            bool contentWasActive = _content.gameObject.activeSelf;

            if (contentWasActive == false)
                _content.gameObject.SetActive(true);

            itemTransform.SetParent(_content, false);
            itemTransform.localScale = scale;
            _items.Add(item);

            if (contentWasActive == false)
                _content.gameObject.SetActive(false);
        }
    }
}