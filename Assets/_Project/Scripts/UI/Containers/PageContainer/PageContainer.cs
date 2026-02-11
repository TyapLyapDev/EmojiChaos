using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.CustomPageContainer
{
    public class PageContainer
    {
        private readonly List<IPagedItem> _items = new();
        private readonly Transform _content;

        public PageContainer(Transform content)
        {
            _content = content != null ? content : throw new ArgumentNullException(nameof(content));
        }

        public int ItemsCount => _items.Count;

        public IReadOnlyList<IPagedItem> Items => _items;

        public void ClearContent()
        {
            _items.Clear();

            foreach (Transform child in _content)
                UnityEngine.Object.Destroy(child.gameObject);
        }

        public void AddItem(IPagedItem item)
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