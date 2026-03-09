using UnityEngine;
using UnityEngine.UI;

namespace EmojiChaos.UI.Panels.LevelsPanel.LevelSelector.MiniCellsLevelSelector.MiniCell.Indicators
{

namespace UI.CustomMiniCellsLevelSelector
{
    public class LockIndicator : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _open;
        [SerializeField] private Sprite _closed;

        internal void SetLockStatus(bool isLock) =>
            _image.sprite = isLock ? _closed : _open;
    }
}
}
