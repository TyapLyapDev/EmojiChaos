using UnityEngine;
using UnityEngine.Splines;

public class SplineOffsetCalculator
{
    private readonly SplineContainer _splineContainer;
    private readonly float _splineLength;

    public SplineOffsetCalculator(SplineContainer splineContainer)
    {
        if(splineContainer == null)
            throw new System.ArgumentNullException(nameof(splineContainer));

        _splineContainer = splineContainer;
        _splineLength = splineContainer.CalculateLength();
    }

    public float SplineLength => _splineLength;

    public Vector3 CalculatePosition(float distance, float sideOffset)
    {
        float normalizedDistance = distance / _splineLength;
        Vector3 position = _splineContainer.EvaluatePosition(normalizedDistance);
        Vector3 sideDirection = CalculateSideDirection(normalizedDistance);

        return position + sideDirection * sideOffset;
    }

    public Vector3 CalculateSideDirection(float normalizedDistance)
    {
        Vector3 tangent = _splineContainer.EvaluateTangent(normalizedDistance);
        Vector3 up = _splineContainer.EvaluateUpVector(normalizedDistance);

        if (tangent.sqrMagnitude < Mathf.Epsilon)
            return Vector3.zero;

        return Vector3.Cross(tangent.normalized, up.normalized).normalized;
    }
}