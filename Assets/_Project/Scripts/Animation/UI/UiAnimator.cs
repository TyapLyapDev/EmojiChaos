using DG.Tweening;
using System;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public abstract class UiAnimator : InitializingBehaviour
{
    private CanvasGroup _canvasGroup;
    private Transform _transform;
    private Tween _tween;
    private bool _isPlaying;
    private bool _isDestroyed;

    private void OnDisable() =>
        KillTween();

    private void OnDestroy()
    {
        _isDestroyed = true;
        KillTween();
    }

    private void KillTween()
    {
        if (_tween != null && _tween.IsActive())
        {
            _tween.Kill();
            _tween = null;
        }

        _isPlaying = false;
    }

    public void Play(UiAnimation animation, Action completed = null)
    {
        ValidateInit(nameof(Play));

        if (_isDestroyed || this == null || gameObject == null)
            return;

        if (_isPlaying)
            return;

        _isPlaying = true;

        if (_transform == null || _canvasGroup == null || isActiveAndEnabled == false)
        {
            _isPlaying = false;
            return;
        }

        _transform.localScale = animation.StartScale;
        _transform.localRotation = animation.StartRotation;
        _canvasGroup.alpha = animation.StartAlpha;

        Sequence sequence = DOTween.Sequence();

        if (_transform != null)
        {
            sequence.Join(_transform.DOScale(animation.TargetScale, animation.Duration)
                .SetEase(animation.Ease)
                .OnKill(() => {
                    if (_isDestroyed || _transform == null) 
                        return;
                }));
        }

        if (_transform != null)
        {
            sequence.Join(_transform.DOLocalRotateQuaternion(animation.TargetRotation, animation.Duration)
                .SetEase(animation.Ease)
                .OnKill(() => {
                    if (_isDestroyed || _transform == null) 
                        return;
                }));
        }

        if (_canvasGroup != null)
        {
            sequence.Join(_canvasGroup.DOFade(animation.TargetAlpha, animation.Duration)
                .SetEase(animation.Ease)
                .OnKill(() => {
                    if (_isDestroyed || _canvasGroup == null) 
                        return;
                }));
        }

        sequence.OnComplete(() =>
        {
            if (_isDestroyed || this == null) return;

            _isPlaying = false;
            completed?.Invoke();
        })
        .OnKill(() =>
        {
            if (_isDestroyed) return;

            _tween = null;
            _isPlaying = false;
        })
        .SetLink(gameObject, LinkBehaviour.KillOnDisable)
        .SetUpdate(true);

        _tween = sequence;
    }

    protected override void OnInitialize()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _transform = transform;
    }
}