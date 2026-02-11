using System;
using System.Collections.Generic;
using UI.CustomPageContainer;

namespace UI.CustomMiniCellsLevelSelector
{
    public class CellFactory : IDisposable
    {
        private readonly PageContainer _container;
        private readonly LevelCell _prefab;
        private readonly Action<LevelCell> _cellClicked;
        private readonly List<LevelCell> _cells = new();

        public CellFactory(PageContainer container, LevelCell prefab, Action<LevelCell> cellClicked)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            _prefab = prefab != null ? prefab : throw new ArgumentNullException(nameof(prefab));
            _cellClicked = cellClicked;
        }

        public void Dispose() =>
            ClearCells();

        public void CreateCells(int count)
        {
            ClearCells();

            for (int i = 0; i < count; i++)
                Create();
        }

        private void Create()
        {
            LevelCell levelCell = UnityEngine.Object.Instantiate(_prefab);
            levelCell.SetActiveCell(false);
            levelCell.Initialize();
            levelCell.Clicked += _cellClicked;
            _cells.Add(levelCell);
            _container.AddItem(levelCell);
        }

        private void ClearCells()
        {
            for (int i = _cells.Count - 1; i >= 0; i--)
            {
                LevelCell card = _cells[i];

                if (card != null)
                    card.Clicked -= _cellClicked;
            }

            _cells.Clear();
            _container.ClearContent();
        }
    }
}