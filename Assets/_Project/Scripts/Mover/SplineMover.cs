using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class SplineMover
{
    private const float OffsetLerpDuration = 0.5f;

    private readonly SplineContainer _splineContainer;
    private readonly Transform _transform;
    private readonly float _splineLength;

    private float _currentDistance;
    private float _targetOffset;
    private float _currentOffset;
    private float _offsetLerpTime;
    private bool _isOffsetInitialized;

    public SplineMover(SplineContainer splineContainer, Transform transform)
    {
        if (splineContainer == null)
            throw new ArgumentNullException(nameof(splineContainer));

        if (transform == null)
            throw new ArgumentNullException(nameof(transform));

        _splineContainer = splineContainer;
        _transform = transform;

        _splineLength = splineContainer.CalculateLength();
        _transform.position = CalculatePositionOffset();
    }

    public void ResetParams()
    {
        _currentDistance = 0f;
        _targetOffset = 0f;
        _currentOffset = 0f;
        _isOffsetInitialized = false;
        _offsetLerpTime = 0f;

        _transform.position = _splineContainer.EvaluatePosition(0f);
    }

    public void SetSideOffset(float value)
    {
        _targetOffset = value;
        _isOffsetInitialized = true;
        _offsetLerpTime = 0f;
    }

    public void Move(float speed, float deltaTime)
    {
        if (deltaTime < 0)
            throw new ArgumentOutOfRangeException(nameof(deltaTime), "ќѕј„ »,  ќ—я ! «начение не может быть меньше нул€");

        float tempDistance = _currentDistance + speed * deltaTime;

        if (tempDistance <= 0 || tempDistance >= _splineLength)
            return;

        UpdateOffsetInterpolation(deltaTime);
        _currentDistance = Mathf.Clamp(tempDistance, 0, _splineLength);
        _transform.position = CalculatePositionOffset();
    }

    private void UpdateOffsetInterpolation(float deltaTime)
    {
        if (_isOffsetInitialized == false)
            return;

        _offsetLerpTime += deltaTime;
        float progress = Mathf.Clamp01(_offsetLerpTime / OffsetLerpDuration);
        _currentOffset = Mathf.Lerp(0f, _targetOffset, progress);

        if (progress >= 1f)
        {
            _isOffsetInitialized = false;
            _currentOffset = _targetOffset;
        }
    }

    private Vector3 CalculatePositionOffset()
    {
        Vector3 tangent = _splineContainer.EvaluateTangent(_currentDistance);
        Vector3 up = _splineContainer.EvaluateUpVector(_currentDistance);
        Vector3 side = Vector3.Cross(tangent.normalized, up.normalized);
        Vector3 position = _splineContainer.EvaluatePosition(_currentDistance);

        return position + side * _currentOffset;
    }
}