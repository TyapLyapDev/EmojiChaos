using EmojiChaos.Core.Abstract.UI;
using EmojiChaos.UI.Panels.LevelsPanel.LevelSelector.MiniCellsLevelSelector;
using UnityEngine;

namespace EmojiChaos.UI.Buttons
{
    public class LevelsPreviousPageButton : ButtonClickHandler<LevelsPreviousPageButton>
    {
        [SerializeField] private MiniCellsLevelSelector _selector;
        [SerializeField] private GameObject _model;

        private void Awake() =>
            Initialize();

        private void OnEnable()
        {
            OnPageChanged();
            _selector.PageChanged += OnPageChanged;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _selector.PageChanged -= OnPageChanged;
        }

        protected override void OnClick()
        {
            base.OnClick();
            _selector.PageFlipper.ShowPreviousPage();
        }

        private void OnPageChanged() =>
            _model.SetActive(_selector.PageFlipper.IsFirstPage == false);
    }
}