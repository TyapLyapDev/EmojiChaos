using System;
using EmojiChaos.AudioSpace;
using EmojiChaos.UI.Panels.LevelsPanel.LevelSelector.CardsLevelSelector.Card;

namespace EmojiChaos.UI.Panels.LevelsPanel.LevelSelector.CardsLevelSelector
{
    public class CardClickHandler
    {
        private readonly Action<int> _levelClicked;

        public CardClickHandler(Action<int> levelClicked)
        {
            _levelClicked = levelClicked ?? throw new ArgumentNullException(nameof(levelClicked));
        }

        public void OnClickCard(LevelCard card)
        {
            if (card.IsLock)
            {
                card.Shake();
                Audio.Sfx.PlayLevelClosed();
            }
            else
            {
                _levelClicked?.Invoke(card.LevelNumber);
                Audio.Sfx.PlayLevelSelected();
            }
        }
    }
}