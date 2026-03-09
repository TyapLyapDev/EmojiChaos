using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EmojiChaos.UI.Tutorial
{
    using Core.Abstract.MonoBehaviourWrapper;
    using EmojiChaos.Utils.Static;
    using Services.Core;

    [RequireComponent(typeof(Mask))]
    public class TutorialCircle : InitializingBehaviour, IPointerClickHandler
    {
        [SerializeField] private RectTransform _backgroundChild;
        [SerializeField] private TutorialPanelClickHandler _panelClickHandler;

        private RectTransform _rectTransform;
        private Canvas _parentCanvas;
        private RectTransform _canvasRectTransform;
        private Vector2 _initialSize;
        private Camera _camera;

    public event Action AnyClicked;
        public event Action CircleClicked;
        public event Action PanelClicked;

        private void OnDestroy()
        {
            if (_panelClickHandler != null)
                _panelClickHandler.Clicked -= OnPanelClicked;
        }

        protected override void OnInitialize()
        {
            _camera = Camera.main;
            _rectTransform = transform as RectTransform;
            _parentCanvas = GetComponentInParent<Canvas>();
            _canvasRectTransform = _parentCanvas.GetComponent<RectTransform>();
            _initialSize = _rectTransform.sizeDelta;
            Hide();

            _panelClickHandler.Clicked += OnPanelClicked;
        }

        public void Show(float sizeMultiplier, Vector3 worldPosition)
        {
            SetPosition(worldPosition);
            SetSize(sizeMultiplier);

            gameObject.SetActive(true);
            _panelClickHandler.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _panelClickHandler.SetActive(false);
        }

        private void SetPosition(Vector3 worldPosition)
        {
            Vector2 screenPoint = _camera.WorldToScreenPoint(worldPosition);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvasRectTransform,
                screenPoint,
                null,
                out Vector2 localPoint);

            _rectTransform.anchoredPosition = localPoint;
        }

        private void SetSize(float sizeMultiplier) =>
            _rectTransform.sizeDelta = _initialSize * sizeMultiplier;

        public void OnPointerClick(PointerEventData eventData)
        {
            CircleClicked?.Invoke();
            AnyClicked?.Invoke();
        }

        private void OnPanelClicked()
        {
            PanelClicked?.Invoke();
            AnyClicked?.Invoke();
        }
    }
}