using UnityEngine;

namespace EmojiChaos.UI.Panels.LevelsPanel.LevelSelector.MiniCellsLevelSelector.MiniCell.Indicators
{
    public class StarsIndicator : MonoBehaviour
    {
        [SerializeField] private StarUi[] _stars;

        public void OpenStars(int count)
        {
            count = Mathf.Clamp(count, 0, _stars.Length);

            for (int i = 0; i < _stars.Length; i++)
                _stars[i].SetStatus(i < count);
        }
    }
}