using UnityEngine;
using UnityEngine.UI;

namespace UI.LevelCards
{
    public class Preview : MonoBehaviour
    {
        [SerializeField] private Image _image;

        public void SetPreview(Sprite sprite) =>
            _image.sprite = sprite;
    }
}