using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI.Shop
{
    public class TabPanel : MonoBehaviour
    {
        [SerializeField] private Transform _content;
        [SerializeField] private Card _prefab;

        private readonly List<Card> _cards = new();

        public event Action<TabPanel, Card> CardClicked;

        public ShopEntityItemType EntityType { get; private set; }

        public IReadOnlyList<ShopCardItemButtonType> ButtonTypes => _cards.Select(card => card.Type).ToList();

        public void Init(IReadOnlyList<ShopCardInfo> cardInfos, ShopEntityItemType entityType)
        {
            ClearContent();

            EntityType = entityType;

            foreach (ShopCardInfo info in cardInfos)
            {
                Card card = Instantiate(_prefab, _content);
                card.Initialize();
                card.SetInfo(info);
                card.Clicked += OnClickCard;
                _cards.Add(card);
            }
        }

        public void ClearContent()
        {
            foreach (Card card in _cards)
                if (card != null)
                    card.Clicked -= OnClickCard;

            _cards.Clear();

            foreach (Transform child in _content)
                Destroy(child.gameObject);
        }

        public bool TryGetCard(string revardedAdvId, out Card card)
        {
            foreach(Card tempCard in _cards)
            {
                if(tempCard.RevardedAdvId == revardedAdvId)
                {
                    card = tempCard;

                    return true;
                }
            }

            card = null;

            return false;
        }

        public void DisableAds()
        {
            foreach(Card card in _cards)
                if (card.Type == ShopCardItemButtonType.NeedViewAds)
                    card.SetType(ShopCardItemButtonType.Opened);
        }

        public void SelectCard(Card card)
        {
            foreach (Card tempCard in _cards)
            {
                if(tempCard != card)
                {
                    if(tempCard.Type == ShopCardItemButtonType.Selected)
                        tempCard.SetType(ShopCardItemButtonType.Opened);

                    continue;
                }

                card.SetType(ShopCardItemButtonType.Selected);
            }
        }

        public void SelCardType(int cardIndex, ShopCardItemButtonType type)
        {
            if (cardIndex < 0 || cardIndex >= _cards.Count)
                return;

            Card card = _cards[cardIndex];
            card.SetType(type);
        }

        public void Show() =>
            gameObject.SetActive(true);

        public void Hide() =>
            gameObject.SetActive(false);

        private void OnClickCard(Card card) =>
            CardClicked?.Invoke(this, card);
    }
}