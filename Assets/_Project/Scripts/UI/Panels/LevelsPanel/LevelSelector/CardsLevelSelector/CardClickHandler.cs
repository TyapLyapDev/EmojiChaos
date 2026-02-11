using System;

namespace UI.LevelCards
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