using UnityEngine;
using UnityEngine.UI;

namespace UI.CustomMiniCellsLevelSelector
{
    public class Star : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Color _openColor;
        [SerializeField] private Color _closedColor;

        public void SetStatus(bool isOpen) =>
            _icon.color = isOpen ? _openColor : _closedColor;
    }
}