using UnityEngine;
using UnityEngine.UI;

namespace EmojiChaos.UI
{
    public class Star : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Color _openColor;
        [SerializeField] private Color _closedColor;

        public void SetStatus (bool isOpen) =>
            _icon.color = isOpen ? _openColor : _closedColor;
    }
}