using System;
using System.Collections.Generic;
using System.Linq;
using UI.CustomPageContainer;

namespace UI.CustomMiniCellsLevelSelector
{
    public class PageInfoUpdater
    {
        private readonly PageContainer _container;
        private readonly IPageFlipper _pageFlipper;
        private readonly List<IMiniCellInfo> _infos;

        public PageInfoUpdater(PageContainer container, IPageFlipper pageFlipper, List<IMiniCellInfo> infos)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            _pageFlipper = pageFlipper ?? throw new ArgumentNullException(nameof(pageFlipper));
            _infos = infos ?? throw new ArgumentNullException(nameof(_infos));
        }

        public void DisplayInfo()
        {
            int levelCount = _infos.Count;
            int startLevel = _pageFlipper.CurrentPage * _container.ItemsCount;
            IReadOnlyList<IPagedItem> items = _container.Items;
            List<LevelCell> cards = items.Cast<LevelCell>().ToList();

            for (int i = 0; i < cards.Count; i++)
            {
                int levelNumber = startLevel + i;
                LevelCell levelCell = cards[i];

                if (levelNumber < _infos.Count)
                    levelCell.SetInfo(_infos[levelNumber]);

                levelCell.SetActiveCell(levelNumber < levelCount);
            }
        }
    }
}