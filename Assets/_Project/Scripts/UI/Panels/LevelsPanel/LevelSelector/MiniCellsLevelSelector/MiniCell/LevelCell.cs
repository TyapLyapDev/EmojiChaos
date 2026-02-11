using UI.CustomPageContainer;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.CustomMiniCellsLevelSelector
{
    public class LevelCell : ButtonClickHandler<LevelCell>, IPagedItem
    {
        [SerializeField] private GameObject _model;
        [SerializeField] private LevelNumber _levelNumber;
        [SerializeField] private StarsIndicator _starsIndicator;
        [SerializeField] private LockIndicator _lockIndicator;
        [SerializeField] private Locker _locker;

        private UiShaker _shaker;
        private IMiniCellInfo _info;

        public bool IsLock => _locker.IsLock;

        public int LevelNumber => _info.LevelNumber;

        public Transform Transform => transform;

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _shaker?.Dispose();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (_locker.IsLock == false)
                base.OnPointerEnter(eventData);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (_locker.IsLock == false)
                base.OnPointerDown(eventData);
        }

        public void SetInfo(IMiniCellInfo info)
        {
            _info = info;
            _levelNumber.SetText(_info.LevelNumber.ToString());
            _starsIndicator.OpenStars(_info.StarCount);
            _lockIndicator.SetLockStatus(_info.IsLock);
            _locker.SetLockStatus(_info.IsLock);
        }

        public void SetActiveCell(bool isActive) =>
            _model.SetActive(isActive);

        public void Shake() =>
            _shaker?.ShakeCombined();

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _shaker = new(_model.GetComponent<RectTransform>());
        }
    }
}