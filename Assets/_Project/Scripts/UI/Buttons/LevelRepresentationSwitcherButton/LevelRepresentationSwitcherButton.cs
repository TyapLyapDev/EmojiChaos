using UnityEngine;
using EmojiChaos.Core.Abstract.UI;
using UnityEngine.UI;

namespace EmojiChaos.UI.Buttons.LevelRepresentationSwitcherButton
{
    public class LevelRepresentationSwitcherButton : ButtonClickHandler<LevelRepresentationSwitcherButton>
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private Sprite _cellSprite;
        [SerializeField] private Sprite _cardSprite;

        public void ShowCellIcon() =>
            _iconImage.sprite = _cellSprite;

        public void ShowCardIcon() =>
            _iconImage.sprite = _cardSprite;
    }
}