using UnityEngine;
using UnityEngine.UI;

namespace UI.Shop
{
    public class CardButtonTypeSwitcher : MonoBehaviour
    {
        [SerializeField] private Image _frame;
        [SerializeField] private Sprite _openedSprite;
        [SerializeField] private Sprite _selectedSprite;
        [SerializeField] private Sprite _needAdsSprite;
        [SerializeField] private GameObject _selectedIcon;
        [SerializeField] private GameObject _needViewIcon;

        public ShopCardItemButtonType Type { get; private set; }

        public void UpdateSprite(ShopCardItemButtonType type)
        {
            Type = type;

            switch (type)
            {
                case ShopCardItemButtonType.Opened:
                    HideAll();
                    break;

                case ShopCardItemButtonType.Selected:
                    SetSelected();
                    break;

                case ShopCardItemButtonType.NeedViewAds:
                    SetNeedAds();
                    break;

                default:
                    throw new System.ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private void SetSelected()
        {
            _frame.sprite = _selectedSprite;
            _selectedIcon.SetActive(true);
            _needViewIcon.SetActive(false);
        }

        private void SetNeedAds()
        {
            _frame.sprite = _needAdsSprite;
            _selectedIcon.SetActive(false);
            _needViewIcon.SetActive(true);
        }

        private void HideAll()
        {
            _frame.sprite = _openedSprite;
            _selectedIcon.SetActive(false);
            _needViewIcon.SetActive(false);
        }
    }
}