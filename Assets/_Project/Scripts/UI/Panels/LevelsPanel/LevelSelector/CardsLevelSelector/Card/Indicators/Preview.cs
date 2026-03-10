using UnityEngine;
using UnityEngine.UI;

namespace EmojiChaos.UI.Panels.LevelsPanel.LevelSelector.CardsLevelSelector.Card.Indicators
{
    public class Preview : MonoBehaviour
    {
        [SerializeField] private Image _image;

        public void SetPreview(Sprite sprite) =>
            _image.sprite = sprite;
    }
}