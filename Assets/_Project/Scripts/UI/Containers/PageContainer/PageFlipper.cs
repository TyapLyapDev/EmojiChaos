using System;
using UnityEngine;

namespace UI.CustomPageContainer
{
    public class PageFlipper : IPageFlipper
    {
        private readonly int _levelsCount;
        private readonly int _cellsCount;
        private readonly Action _pageChanged;

        private int _currentPage;

        public PageFlipper(int levelsCount, int cellsCount, Action pageChanged)
        {
            if (levelsCount < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(levelsCount), 
                    levelsCount, 
                    " оличество уровней не может быть отрицательным");

            _levelsCount = levelsCount;

            if(cellsCount <= 0) 
                throw new ArgumentOutOfRangeException(
                    nameof(cellsCount), 
                    cellsCount, 
                    " оличество €чеек не может быть меньше единицы");

            _cellsCount = cellsCount;

            _pageChanged = pageChanged;
        }

        public bool IsFirstPage => _currentPage == 0;

        public bool IsLastPage => _currentPage == MaxPage;

        public int CurrentPage => _currentPage;

        private int MaxPage => (_levelsCount - 1) / _cellsCount;

        public void SelectPageByItem(int levelIndex)
        {
            levelIndex = Mathf.Clamp(levelIndex, 0, _levelsCount - 1);
            _currentPage = (Mathf.Min(levelIndex, _levelsCount - 1)) / _cellsCount;
            _pageChanged?.Invoke();
        }

        public void ShowNextPage()
        {
            _currentPage = (_currentPage + 1) % (MaxPage + 1);
            _pageChanged?.Invoke();
        }

        public void ShowPreviousPage()
        {
            _currentPage = (_currentPage - 1 + (MaxPage + 1)) % (MaxPage + 1);
            _pageChanged?.Invoke();
        }
    }
}