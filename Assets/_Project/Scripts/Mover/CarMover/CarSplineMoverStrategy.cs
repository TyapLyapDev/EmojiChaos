using System;
using System.Collections.Generic;
using UnityEngine;

public class CarSplineMoverStrategy : IMovementStrategy
{
    private const float BaseRotationSpeed = 10f;
    private const float BaseMovementSpeed = 5f;

    private readonly Transform _transform;
    private readonly List<SplineSegment> _path;
    private float _currentSegmentProgress;
    private Quaternion _targetRotation;
    private Vector3 _targetPosition;

    public CarSplineMoverStrategy(Transform transform, List<SplineSegment> path)
    {
        _transform = transform;
        _path = path;
        _targetRotation = _transform.rotation;
    }

    public event Action<CarSplineMoverStrategy> DestinationReached;

    public void Move(float deltaDistance)
    {
        if (_path == null || _path.Count == 0)
            return;

        float remainingDistance = deltaDistance;

        while (remainingDistance > 0 && _path.Count > 0)
        {
            SplineSegment currentSegment = _path[0];
            float segmentLength = currentSegment.CalculateLength();

            if (segmentLength <= 0)
            {
                RemoveCurrentSegment();

                continue;
            }

            float progressDelta = remainingDistance / segmentLength;
            float newProgress = _currentSegmentProgress + progressDelta;

            if (newProgress >= 1f)
            {
                float usedProgress = 1f - _currentSegmentProgress;
                float usedDistance = usedProgress * segmentLength;
                remainingDistance -= usedDistance;

                RemoveCurrentSegment();

                continue;
            }

            _currentSegmentProgress = newProgress;
            remainingDistance = 0;
            UpdateTargetTransform();
        }

        ApplySmoothTransform(deltaDistance);

        if (_path.Count == 0)
            DestinationReached?.Invoke(this);
    }

    private void RemoveCurrentSegment()
    {
        if (_path.Count > 0)
        {
            _path.RemoveAt(0);
            _currentSegmentProgress = 0;
        }
    }

    private void UpdateTargetTransform()
    {
        if (_path.Count == 0)
            return;

        SplineSegment currentSegment = _path[0];
        float currentSplineProgress = Mathf.Lerp(currentSegment.StartProgress, currentSegment.EndProgress, _currentSegmentProgress);

        _targetPosition = currentSegment.GetWorldPosition(currentSplineProgress);
        Vector3 worldTangent = currentSegment.GetWorldTangent(currentSplineProgress);
        _targetRotation = Quaternion.LookRotation(worldTangent);
    }

    private void ApplySmoothTransform(float deltaDistance)
    {
        _transform.position = Vector3.Lerp(_transform.position, _targetPosition, BaseMovementSpeed * deltaDistance);
        _transform.rotation = Quaternion.Slerp(_transform.rotation, _targetRotation, BaseRotationSpeed * deltaDistance);
    }
}