using System;
using UnityEngine;
using UnityEngine.Splines;

public class SplineOffsetCalculator
{
    private const float MinimumTangentMagnitude = 0.001f;

    private readonly SplineContainer _splineContainer;
    private readonly float _splineLength;

    public SplineOffsetCalculator(SplineContainer splineContainer)
    {
        if(splineContainer == null)
            throw new ArgumentNullException(nameof(splineContainer));

        _splineContainer = splineContainer;
        _splineLength = splineContainer.CalculateLength();

        if (_splineLength <= 0f)
            throw new InvalidOperationException("Длина сплайна должна быть больше нуля");
    }

    public float SplineLength => _splineLength;

    public Vector3 CalculatePosition(float splineDistance, float sideOffset)
    {
        float normalizedDistance = CalculateNormalizedDistance(splineDistance);
        Vector3 position = _splineContainer.EvaluatePosition(normalizedDistance);
        Vector3 sideDirection = CalculateSideDirection(normalizedDistance);

        return sideDirection * sideOffset + position;
    }

    public Vector3 CalculateSideDirection(float normalizedDistance)
    {
        Vector3 tangent = _splineContainer.EvaluateTangent(normalizedDistance);
        Vector3 upVector = _splineContainer.EvaluateUpVector(normalizedDistance);

        if (tangent.sqrMagnitude < MinimumTangentMagnitude)
            return Vector3.zero;

        return Vector3.Cross(tangent.normalized, upVector.normalized).normalized;
    }

    private float CalculateNormalizedDistance(float distance)
    {
        float clampedDistance = Mathf.Clamp(distance, 0f, _splineLength);
        
        return _splineLength > 0f ? clampedDistance / _splineLength : 0f;
    }
}