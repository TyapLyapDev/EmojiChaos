using UnityEngine;
using UnityEngine.UI;

namespace UI.LevelCards
{
    public class Star : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Sprite _opened;
        [SerializeField] private Sprite _closed;

        public void SetStatus(bool isOpen) =>
            _icon.sprite = isOpen ? _opened : _closed;
    }
}