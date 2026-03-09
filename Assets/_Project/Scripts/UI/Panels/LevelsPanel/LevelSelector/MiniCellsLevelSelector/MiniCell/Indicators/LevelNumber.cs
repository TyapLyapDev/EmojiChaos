using TMPro;
using UnityEngine;

namespace EmojiChaos.UI.Panels.LevelsPanel.LevelSelector.MiniCellsLevelSelector.MiniCell.Indicators
{

namespace UI.CustomMiniCellsLevelSelector
{
    public class LevelNumber : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        public void SetText(string text) =>
            _text.text = text;
    }
}
}
