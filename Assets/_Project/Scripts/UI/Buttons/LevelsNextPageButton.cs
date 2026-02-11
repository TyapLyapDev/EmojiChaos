using UnityEngine;

namespace UI.CustomMiniCellsLevelSelector
{
    public class LevelsNextPageButton : ButtonClickHandler<LevelsNextPageButton>
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
            _selector.PageFlipper.ShowNextPage();
        }

        private void OnPageChanged() =>
            _model.SetActive(_selector.PageFlipper.IsLastPage == false);
    }
}