using System;
using System.Collections.Generic;
using UI.CustomPageContainer;
using UnityEngine;

namespace UI.CustomMiniCellsLevelSelector
{
    public class MiniCellsLevelSelector : MonoBehaviour, ILevelSelector
    {
        [SerializeField] private Transform _content;
        [SerializeField] private LevelCell _prefab;
        [SerializeField] private int _cellCapacity = 20;

        private PageContainer _container;
        private PageFlipper _pageFlipper;
        private CellFactory _cellFactory;
        private CellsClickHandler _cellClickHandler;
        private PageInfoUpdater _infoUpdater;

        public event Action<int> LevelClicked;
        public event Action PageChanged;

        public IPageFlipper PageFlipper => _pageFlipper;

        public void Init(List<IMiniCellInfo> infos)
        {
            _container = new(_content);
            _cellClickHandler = new(OnLevelClick);
            _pageFlipper = new(infos.Count, _cellCapacity, OnPageChanged);
            _cellFactory = new(_container, _prefab, _cellClickHandler.OnClickCell);
            _infoUpdater = new(_container, _pageFlipper, infos);

            _container.ClearContent();
            _cellFactory.CreateCells(_cellCapacity);
            _pageFlipper.SelectPageByItem(0);
        }

        public void Hide() =>
            gameObject.SetActive(false);

        public void Show() =>
            gameObject.SetActive(true);

        public void AlignByLevel(int levelIndex) =>
            _pageFlipper.SelectPageByItem(levelIndex);

        private void OnLevelClick(int levelNumber)
        {
            int levelIndex = levelNumber - 1;
            LevelClicked?.Invoke(levelIndex);
        }

        private void OnPageChanged()
        {
            _infoUpdater.DisplayInfo();
            PageChanged?.Invoke();
        }
    }
}