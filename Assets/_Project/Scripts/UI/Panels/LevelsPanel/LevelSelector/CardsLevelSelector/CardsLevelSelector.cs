using System;
using System.Collections.Generic;
using UI.CustomScrollContainer;
using UnityEngine;
using UnityEngine.UI;

namespace UI.LevelCards
{
    public class CardsLevelSelector : MonoBehaviour, ILevelSelector
    {
        [SerializeField] private ScrollRect _scroll;
        [SerializeField] private RectTransform _viewport;
        [SerializeField] private Transform _content;
        [SerializeField] private LevelCard _prefab;

        private ItemContainer _itemContainer;
        private ScrollNavigator _scroller;
        private CardFactory _cardFactory;
        private CardClickHandler _cardClickHandler;

        public event Action<int> LevelClicked;

        private void OnDestroy() =>
            _cardFactory.Dispose();

        public void Init(IReadOnlyList<ICardInfo> cardInfos)
        {
            _itemContainer = new(_content);
            _scroller = new(_scroll, _viewport, _content as RectTransform, _itemContainer);
            _cardClickHandler = new(OnClickLevel);
            _cardFactory = new(_prefab, _itemContainer, cardInfos, _cardClickHandler.OnClickCard);
            _cardFactory.CreateCards();
        }

        public void Hide() =>
            gameObject.SetActive(false);

        public void Show() =>
            gameObject.SetActive(true);

        public void AlignByLevel(int levelIndex) =>
            _scroller.AlignByItem(levelIndex);

        private void OnClickLevel(int level)
        {
            int levelIndex = level - 1;
            LevelClicked?.Invoke(levelIndex);
        }
    }
}