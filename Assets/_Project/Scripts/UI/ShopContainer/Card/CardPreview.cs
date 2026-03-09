using UnityEngine;
using UnityEngine.UI;

namespace EmojiChaos.UI.ShopContainer.Card
{

namespace UI.Shop
{
    public class CardPreview : MonoBehaviour
    {
        [SerializeField] private Image _image;

        public void SetSprite(Sprite sprite) =>
            _image.sprite = sprite;
    }
}
}
