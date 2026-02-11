using System;
using System.Collections.Generic;
using UI.CustomScrollContainer;

namespace UI.LevelCards
{
    public class CardFactory : IDisposable
    {
        private readonly LevelCard _prefab;
        private readonly ItemContainer _container;
        private readonly IReadOnlyList<ICardInfo> _cardInfos;
        private readonly Action<LevelCard> _cardClicked;
        private readonly List<LevelCard> _cards = new();

        public CardFactory(LevelCard prefab, ItemContainer container, IReadOnlyList<ICardInfo> cardInfos, Action<LevelCard> onCardClicked)
        {
            _prefab = prefab != null ? prefab : throw new ArgumentNullException(nameof(prefab));
            _container = container != null ? container : throw new ArgumentNullException(nameof(container));
            _cardInfos = cardInfos ?? throw new ArgumentNullException(nameof(cardInfos));
            _cardClicked = onCardClicked ?? throw new ArgumentNullException(nameof(onCardClicked));
        }

        public void Dispose() =>
            ClearCards();

        public void CreateCards()
        {
            ClearCards();

            foreach (ICardInfo cardInfo in _cardInfos)
                if (cardInfo != null)
                    Create(cardInfo);
        }

        private void Create(ICardInfo cardInfo)
        {
            LevelCard card = UnityEngine.Object.Instantiate(_prefab);
            card.Initialize();
            card.SetInfo(cardInfo);
            card.Clicked += _cardClicked;
            _container.AddItem(card);
            _cards.Add(card);
        }

        private void ClearCards()
        {
            for (int i = _cards.Count - 1; i >= 0; i--)
            {
                LevelCard card = _cards[i];

                if (card != null)
                    card.Clicked -= _cardClicked;
            }

            _cards.Clear();
            _container.ClearContent();
        }
    }
}