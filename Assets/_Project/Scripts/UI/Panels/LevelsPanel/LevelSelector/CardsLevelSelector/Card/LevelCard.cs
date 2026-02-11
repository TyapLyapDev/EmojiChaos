using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.LevelCards
{
    public class LevelCard : ButtonClickHandler<LevelCard>, CustomScrollContainer.IScrolledItem
    {
        [SerializeField] private LevelNumber _levelNumber;
        [SerializeField] private StarsIndiator _starsIndicator;
        [SerializeField] private DifficultyIndicator _difficultyIndicator;
        [SerializeField] private CrowdSequenceIndicator _crowdSequenceIndicator;
        [SerializeField] private Preview _preview;
        [SerializeField] private LockIndicator _lockIndicator;
        [SerializeField] private Locker _locker;
        [SerializeField] private RectTransform _center;

        private UiShaker _shaker;
        private ICardInfo _cardInfo;

        public bool IsLock => _locker.IsLock;

        public int LevelNumber => _cardInfo.LevelNumber;

        public Transform Transform => transform;

        public RectTransform Center => _center;

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

        public void SetInfo(ICardInfo cardInfo)
        {
            _cardInfo = cardInfo;            
            _levelNumber.SetText(cardInfo.LevelNumber.ToString());
            _starsIndicator.OpenStars(cardInfo.StarCount);
            _difficultyIndicator.SetDifficulty(cardInfo.Difficulty);
            _crowdSequenceIndicator.SetSequenceType(cardInfo.CrowdSequenceType);
            _preview.SetPreview(cardInfo.Preview);
            _lockIndicator.SetLockStatus(cardInfo.IsLock);
            _locker.SetLockStatus(cardInfo.IsLock);
        }

        public void Shake() =>
            _shaker?.ShakeCombined();

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _shaker = new(_locker.GetComponent<RectTransform>());
        }

        protected override void OnClick()
        {
            if (_locker.IsLock == false)
                base.OnClick();
        }
    }
}