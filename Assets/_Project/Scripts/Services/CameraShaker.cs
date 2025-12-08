using DG.Tweening;
using System;
using UnityEngine;

public class CameraShaker : IDisposable
{
    private const float DefaultDuration = 0.5f;
    private const float DefaultStrength = 0.2f;
    private const int DefaultVibrato = 30;
    private const float DefaultRandomness = 90f;
    private const bool DefaultFadeOut = false;

    private const float RotationDurationMultiplier = 0.8f;
    private const float RotationStrength = 1f;
    private const int RotationVibratoDivider = 2;

    private readonly Transform _cameraTransform;
    private readonly Vector3 _originalPosition;
    private readonly Quaternion _originalRotation;

    private Tween _currentPositionTween;
    private Tween _currentRotationTween;

    public CameraShaker()
    {
        Camera camera = Camera.main;

        if (camera == null)
            throw new ArgumentNullException(nameof(camera));

        _cameraTransform = camera.transform;
        _originalPosition = _cameraTransform.position;
        _originalRotation = _cameraTransform.rotation;
    }

    public void Dispose() =>
        StopShake();

    public void Shake()
    {
        StopShake();

        if (_cameraTransform == null)
            return;

        _currentPositionTween = _cameraTransform.DOShakePosition(
            duration: DefaultDuration,
            strength: DefaultStrength,
            vibrato: DefaultVibrato,
            randomness: DefaultRandomness,
            fadeOut: DefaultFadeOut
        ).SetUpdate(UpdateType.Normal, true);

        _currentRotationTween = _cameraTransform.DOShakeRotation(
            duration: DefaultDuration * RotationDurationMultiplier,
            strength: RotationStrength,
            vibrato: DefaultVibrato / RotationVibratoDivider,
            randomness: DefaultRandomness
        ).SetUpdate(UpdateType.Normal, true);
    }

    private void StopShake()
    {
        if (_currentPositionTween != null && _currentPositionTween.IsActive())
        {
            _currentPositionTween.Kill();
            _currentPositionTween = null;
        }

        if (_currentRotationTween != null && _currentRotationTween.IsActive())
        {
            _currentRotationTween.Kill();
            _currentRotationTween = null;
        }

        if (_cameraTransform != null)
            _cameraTransform.SetPositionAndRotation(_originalPosition, _originalRotation);
    }
}