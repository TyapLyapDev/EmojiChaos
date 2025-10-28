using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class CarSplineMover : ICarMovementStrategy
{
    private readonly Transform _transform;
    private readonly Transform _destination;
    private readonly Transform _splineContainerTransform;
    private readonly MapSplineNodes _nodesMap;

    private List<SplineSegment> _pathSegments;
    private int _currentSegmentIndex;
    private float _currentSegmentProgress;

    public CarSplineMover(Transform transform, MapSplineNodes nodesMap, Transform destination)
    {
        _transform = transform;
        _nodesMap = nodesMap;
        _destination = destination;
        _splineContainerTransform = _nodesMap.SplineContainerTransform;

        CalculatePath();
    }

    public event Action<CarSplineMover> DestinationReached;

    public void Move(float deltaDistance)
    {
        if (_pathSegments == null || _pathSegments.Count == 0)
            return;

        SplineSegment currentSegment = _pathSegments[_currentSegmentIndex];
        float segmentLength = CalculateSegmentLength(currentSegment);

        if (segmentLength <= 0)
        {
            IsGoToNextSegment();
            return;
        }

        float progressDelta = deltaDistance / segmentLength;
        _currentSegmentProgress += progressDelta;

        if (_currentSegmentProgress >= 1f)
        {
            float remainingProgress = _currentSegmentProgress - 1f;
            float remainingDistance = remainingProgress * segmentLength;

            if (IsGoToNextSegment())
                Move(remainingDistance);

            return;
        }

        UpdateCarTransform();
    }

    private void CalculatePath()
    {
        _pathSegments = _nodesMap.GetPathSegments(_transform.position, _destination.position);

        if (_pathSegments.Count > 0)
        {
            _currentSegmentIndex = 0;
            _currentSegmentProgress = 0f;

            UpdateCarTransform();
            return;
        }
    }

    private float CalculateSegmentLength(SplineSegment segment)
    {
        if (segment.IsTransition)
            return 0f;

        float startT = segment.StartT;
        float endT = segment.EndT;
        float segmentLength = segment.Spline.GetLength() * Mathf.Abs(endT - startT);

        return segmentLength;
    }

    private bool IsGoToNextSegment()
    {
        _currentSegmentIndex++;
        _currentSegmentProgress = 0f;

        if (_currentSegmentIndex >= _pathSegments.Count)
        {
            OnDestinationReached();
            return false;
        }

        if (_pathSegments[_currentSegmentIndex].IsTransition)
            return IsGoToNextSegment();

        UpdateCarTransform();

        return true;
    }

    private void UpdateCarTransform()
    {
        if (_currentSegmentIndex >= _pathSegments.Count)
            return;

        SplineSegment currentSegment = _pathSegments[_currentSegmentIndex];

        float currentT = currentSegment.IsTransition ?
            currentSegment.StartT :
            Mathf.Lerp(currentSegment.StartT, currentSegment.EndT, _currentSegmentProgress);

        Vector3 localPosition = currentSegment.Spline.EvaluatePosition(currentT);
        Vector3 worldPosition = _splineContainerTransform.TransformPoint(localPosition);

        Vector3 localTangent = currentSegment.Spline.EvaluateTangent(currentT);
        Vector3 worldTangent = _splineContainerTransform.TransformDirection(localTangent);

        _transform.position = worldPosition;

        if (worldTangent != Vector3.zero)
            _transform.rotation = Quaternion.LookRotation(worldTangent.normalized);
    }

    private void OnDestinationReached()
    {
        DestinationReached?.Invoke(this);
        Debug.Log("Destination reached!");
    }
}