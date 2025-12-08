using DG.Tweening;
using System;
using UnityEngine;

public class UiShaker : IDisposable
{
    private readonly RectTransform _targetTransform;
    private Tween _currentShakeTween;
    private Vector2 _originalAnchoredPosition;
    private Vector3 _originalLocalScale;
    private Quaternion _originalLocalRotation;

    public UiShaker(RectTransform targetTransform)
    {
        _targetTransform = targetTransform != null ? targetTransform : throw new ArgumentNullException(nameof(targetTransform));
        SaveOriginalValues();
    }

    public void Dispose() =>
        StopShake();

    public void ShakeRotation(float duration = 0.2f, float strength = 25f, int vibrato = 12, float randomness = 60f)
    {
        StopShake();

        _currentShakeTween = _targetTransform.DOShakeRotation(duration, strength: new Vector3(0, 0, strength), vibrato: vibrato, randomness: randomness)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                _targetTransform.DOLocalRotate(_originalLocalRotation.eulerAngles, 0.1f).SetEase(Ease.InOutQuad);
            });
    }

    public void ShakePosition(float duration = 0.2f, float strength = 20f, int vibrato = 18, float randomness = 70f, bool fadeOut = true)
    {
        StopShake();

        _currentShakeTween = _targetTransform.DOShakeAnchorPos(duration, strength: new Vector2(strength, strength), vibrato: vibrato, randomness: randomness, fadeOut: fadeOut)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                _targetTransform.DOAnchorPos(_originalAnchoredPosition, 0.1f).SetEase(Ease.InOutQuad);
            });
    }

    public void ShakeScale(float duration = 0.2f, float strength = 0.2f, int vibrato = 8, float randomness = 30f)
    {
        StopShake();

        _currentShakeTween = _targetTransform.DOShakeScale(duration, strength: strength, vibrato: vibrato, randomness: randomness)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                _targetTransform.DOScale(_originalLocalScale, 0.1f).SetEase(Ease.InOutQuad);
            });
    }

    public void ShakeCombined(float duration = 0.2f, float rotationStrength = 10f, float positionStrength = 5f, float scaleStrength = 0.05f)
    {
        StopShake();

        Sequence shakeSequence = DOTween.Sequence();

        shakeSequence.Join(_targetTransform.DOShakeRotation(duration, strength: new Vector3(0, 0, rotationStrength), vibrato: 8, randomness: 30));
        shakeSequence.Join(_targetTransform.DOShakeAnchorPos(duration, strength: new Vector2(positionStrength, positionStrength), vibrato: 10, randomness: 40, fadeOut: true));
        shakeSequence.Join(_targetTransform.DOShakeScale(duration, strength: scaleStrength, vibrato: 4, randomness: 15));
        shakeSequence.SetEase(Ease.OutQuad).OnComplete(RestoreOriginalValues);
        _currentShakeTween = shakeSequence;
    }

    public void StopShake()
    {
        if (_currentShakeTween != null && _currentShakeTween.IsActive())
            _currentShakeTween.Kill();

        _currentShakeTween = null;
        RestoreOriginalValues();
    }

    public void SaveOriginalValues()
    {
        if (_targetTransform == null)
            return;

        _originalAnchoredPosition = _targetTransform.anchoredPosition;
        _originalLocalRotation = _targetTransform.localRotation;
        _originalLocalScale = _targetTransform.localScale;
    }

    public void RestoreOriginalValues()
    {
        if (_targetTransform == null)
            return;

        _targetTransform.anchoredPosition = _originalAnchoredPosition;
        _targetTransform.localRotation = _originalLocalRotation;
        _targetTransform.localScale = _originalLocalScale;
    }
}