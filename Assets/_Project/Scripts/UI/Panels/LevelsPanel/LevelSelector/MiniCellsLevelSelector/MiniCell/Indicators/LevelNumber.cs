using UnityEngine;
using TMPro;

namespace EmojiChaos.UI.Panels.LevelsPanel.LevelSelector.MiniCellsLevelSelector.MiniCell.Indicators
{
    public class LevelNumber : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        public void SetText(string text) =>
            _text.text = text;
    }
}