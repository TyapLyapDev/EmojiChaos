using TMPro;
using UnityEngine;

namespace EmojiChaos.UI.Panels.LevelsPanel.LevelSelector.CardsLevelSelector.Card.Indicators
{
    public class LevelNumber : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        public void SetText(string text) =>
            _text.text = text;
    }
}