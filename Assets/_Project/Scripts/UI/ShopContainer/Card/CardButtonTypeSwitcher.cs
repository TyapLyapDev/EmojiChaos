using UnityEngine;
using UnityEngine.UI;

namespace UI.Shop
{
    public class CardButtonTypeSwitcher : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _opened;
        [SerializeField] private Sprite _selected;
        [SerializeField] private Sprite _needViewAds;

        public ShopCardItemButtonType Type { get; private set; }

        public void UpdateSprite(ShopCardItemButtonType type)
        {
            Type = type;

            Sprite sprite = type switch
            {
                ShopCardItemButtonType.Opened => _opened,
                ShopCardItemButtonType.Selected => _selected,
                ShopCardItemButtonType.NeedViewAds => _needViewAds,
                _ => throw new System.ArgumentOutOfRangeException()
            };

            _image.sprite = sprite;
        }
    }
}