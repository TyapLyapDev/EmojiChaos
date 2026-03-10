using UnityEngine;

namespace EmojiChaos.UI.Tutorial.Explanations
{
    using Core.Abstract.Interface;
    using Entities.Car;

    public class CarSwipeExplanation : TutorialItem
    {
        [SerializeField] private RectTransform _canvas;

        protected override void OnActivated()
        {
            SetPosition(Config.Car.transform.position);
            Config.SwipeStrategy.HasSwipe += OnSwipe;

            Config.EnemySpawner.Pause();
            Config.EnemiesSpeedDirector.Pause();
            Show();
        }

        protected override void OnDeactivated()
        {
            Config.SwipeStrategy.HasSwipe -= OnSwipe;
            Config.EnemySpawner.Resume();
            Config.EnemiesSpeedDirector.Resume();
            Hide();
        }

        private void SetPosition(Vector3 worldPosition)
        {
            if (IsActivated == false)
                return;

            Vector2 screenPoint = Camera.main.WorldToScreenPoint(worldPosition);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas,
                screenPoint,
                null,
                out Vector2 localPoint);

            RectTransform rect = transform as RectTransform;

            rect.anchoredPosition = localPoint;
        }

        private void OnSwipe(ISwipeable swipeable, int count)
        {
            if (IsActivated == false)
                return;

            if (swipeable is Car == false)
                return;

            Config.SwipeStrategy.HasSwipe -= OnSwipe;
            Deactivate();
        }
    }
}
