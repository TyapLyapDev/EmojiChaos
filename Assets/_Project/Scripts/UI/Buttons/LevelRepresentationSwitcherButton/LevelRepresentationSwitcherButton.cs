using UnityEngine;
using UnityEngine.UI;

namespace EmojiChaos.UI.Buttons.LevelRepresentationSwitcherButton
{
    using Core.Abstract.UI;

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