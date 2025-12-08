using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class ButtonClickHandler<T> : InitializingBehaviour,
    IPointerDownHandler, IPointerUpHandler, IPointerClickHandler,
    IPointerEnterHandler, IPointerExitHandler
    where T : ButtonClickHandler<T>
{
    [SerializeField] private Image _image;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private float _pointerEnterScale = 1.1f;
    [SerializeField] private float _pointerExitScale = 1f;
    [SerializeField] private float _animationEnterDuration = 0.5f;
    [SerializeField] private float _animationExitDuration = 0.25f;
    [SerializeField] private Ease _enterEase = Ease.OutElastic;
    [SerializeField] private Ease _exitEase = Ease.OutBack;

    private Vector3 _originalScale;
    private Tween _currentTween;
    private Color _initialColor;

    public event Action<T> Clicked;
    public event Action<T> Pressed;
    public event Action<T> Unpressed;

    protected T Self => (T)this;

    protected virtual void OnDisable() =>
        KillTween();

    protected virtual void OnDestroy() =>
        KillTween();

    private void KillTween()
    {
        if (_currentTween != null && _currentTween.IsActive())
        {
            _currentTween.Kill();
            _currentTween = null;
        }
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        ValidateInit(nameof(OnPointerEnter));
        PlayEnterAnimation();
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        ValidateInit(nameof(OnPointerExit));
        _image.color = _initialColor;
        PlayExitAnimation();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        ValidateInit(nameof(OnPointerDown));
        Audio.Sfx.PlayPointerDownButton();
        _image.color = _selectedColor;
        Pressed?.Invoke(Self);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        ValidateInit(nameof(OnPointerUp));
        _image.color = _initialColor;
        Unpressed?.Invoke(Self);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        ValidateInit(nameof(OnPointerClick));
        OnClick();
        Clicked?.Invoke(Self);
    }

    public void Show()
    {
        ValidateInit(nameof(Show));
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        ValidateInit(nameof(Hide));
        gameObject.SetActive(false);
    }

    protected override void OnInitialize()
    {
        _originalScale = transform.localScale;
        _initialColor = _image.color;
    }

    protected virtual void OnClick() =>
        Audio.Sfx.PlayPointerUpButton();

    private void PlayEnterAnimation()
    {
        KillTween();

        _currentTween = transform.DOScale(_originalScale * _pointerEnterScale, _animationEnterDuration)
            .SetEase(_enterEase)
            .SetUpdate(true)
            .OnKill(() => _currentTween = null);
    }

    private void PlayExitAnimation()
    {
        KillTween();

        _currentTween = transform.DOScale(_originalScale * _pointerExitScale, _animationExitDuration)
            .SetEase(_exitEase)
            .SetUpdate(true)
            .OnKill(() => _currentTween = null);
    }
}