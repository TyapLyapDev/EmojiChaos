using System;
using UnityEngine;
using UnityEngine.Splines;

public class SplineSegment
{
    private readonly Spline _spline;
    private readonly Transform _splineContainerTransform;
    private readonly float _startProgress;
    private readonly float _endProgress;

    private float _length;
    private bool _isReversedDirectionToRotation;

    public SplineSegment(Spline spline,
        Transform splineContainerTransform,
        float startProgress,
        float endProgress)
    {
        _spline = spline ?? throw new ArgumentNullException(nameof(spline));
        _splineContainerTransform = splineContainerTransform != null ? splineContainerTransform : throw new ArgumentNullException(nameof(spline));
        _startProgress = startProgress;
        _endProgress = endProgress;

        PerformCalculations();
    }

    public float Length => _length;

    public Vector3 GetWorldPositionBySegmentProgress(float segmentProgress)
    {
        float splineProgress = CalculateSplineProgress(segmentProgress);

        return GetWorldPositionBySplineProgress(splineProgress);
    }

    public Vector3 GetWorldTangentBySegmentProgress(float segmentProgress)
    {
        float splineProgress = CalculateSplineProgress(segmentProgress);

        return GetWorldTangentBySplineProgress(splineProgress);
    }

    private Vector3 GetWorldPositionBySplineProgress(float splineProgress)
    {
        float normalizedProgress = NormalizeProgress(splineProgress);
        Vector3 localPosition = _spline.EvaluatePosition(normalizedProgress);
        Vector3 worldPosition = _splineContainerTransform.TransformPoint(localPosition);

        return worldPosition;
    }

    private Vector3 GetWorldTangentBySplineProgress(float splineProgress)
    {
        float normalizedProgress = NormalizeProgress(splineProgress);
        Vector3 localTangent = _spline.EvaluateTangent(normalizedProgress);
        Vector3 worldTangent = _splineContainerTransform.TransformDirection(localTangent);

        if (_isReversedDirectionToRotation)
            worldTangent = -worldTangent;

        if (worldTangent.sqrMagnitude > Mathf.Epsilon)
            worldTangent = worldTangent.normalized;

        return worldTangent;
    }

    private float NormalizeProgress(float progress)
    {
        if (_spline.Closed)
            return Mathf.Repeat(progress, 1f);

        return Mathf.Clamp01(progress);
    }

    private void PerformCalculations()
    {
        DetermineDirectionStatus();
        CalculateLength();
    }

    private void DetermineDirectionStatus() =>
        _isReversedDirectionToRotation = _startProgress > _endProgress;

    private void CalculateLength() =>
        _length = _spline.GetLength() * Mathf.Abs(_endProgress - _startProgress);

    private float CalculateSplineProgress(float segmentProgress) =>
        Mathf.Lerp(_startProgress, _endProgress, segmentProgress);
}